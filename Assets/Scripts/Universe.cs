using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public int seed;
    public GameObject starPrefab;
    public GameObject backgroundStar;
    public float radius;
    // We will multiply this number by a bunch and put more in the background
    public int starCountTouchable;
    public float backgroundStarMultiplier;

    private GameObject[] stars;
    private List<GameObject> backgroundStars;

    public GameObject pathPrefab;

    private List<Path> paths;

    private Transform background;
    private LookAround lr;
    // Start is called before the first frame update
    void Start() {
        Random.InitState(seed);
        foreach (Transform t in transform) {
            if (t.name == "Background") {
                background = t;
            }
        }

        stars = new GameObject[starCountTouchable];
        backgroundStars = new List<GameObject>();

        // First star in the centre of the universe
        GameObject star = Instantiate(starPrefab, new Vector3(), new Quaternion());
        stars[0] = star;

        for (int i = 1; i < starCountTouchable; i++) {
            SpawnStar(i);
        }
        for (int i = 0; i < starCountTouchable*backgroundStarMultiplier; i++) {
            SpawnBackgroundStar();
        }
        lr = FindObjectOfType<LookAround>();
        lr.GoToStar(stars[0]);
        
        for (int i = 0; i< 100; i++) {
            Star start = stars[Random.Range(0, starCountTouchable - 1)].GetComponent<Star>();
            Star end = stars[Random.Range(0, starCountTouchable - 1)].GetComponent<Star>();
            if (start == end) {
                continue;
            }
            SpawnPath(start, end);
        }
    }

    private void SpawnStar(int index) {
        Vector3 position = Random.insideUnitSphere * radius;
        GameObject star = Instantiate(starPrefab, position, new Quaternion());
        stars[index] = star;
    }

    private void SpawnBackgroundStar() {
        Vector3 position = new Vector3(
            Random.Range(-radius, radius),
            Random.Range(-radius, radius),
            Random.Range(-radius, radius)
        );
        Vector3 sphereEdge = position.normalized * radius;
        // Make sure the sprites are nice and far away!
        GameObject star = Instantiate(backgroundStar, sphereEdge * 2 + position*5, new Quaternion(), background);
        star.transform.LookAt(transform);
        backgroundStars.Add(star);
    }

    private void SpawnPath(Star start, Star end) {
        Path path = Instantiate(pathPrefab).GetComponent<Path>();
        path.SetupPath(start, end, start.GetTradeItem());
    }

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 3000f)) {
            Star star = hit.transform.GetComponent<Star>();
            if (star) {
                lr.Aim(star);
                if (Input.GetMouseButtonDown(0)) {
                    lr.GoToStar(star.gameObject);
                }
            }
        }

    }
}
