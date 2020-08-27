using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Universe : MonoBehaviour
{
    public GameObject starPrefab;
    public GameObject backgroundStar;
    public float radius;
    // We will multiply this number by a bunch and put more in the background
    public int starCountTouchable;
    public float backgroundStarMultiplier;
    public float timeToProduce;
    public int pathCount;

    public float currentTimeToProduce;

    private GameObject[] stars;
    private List<GameObject> backgroundStars;

    public GameObject pathPrefab;

    private List<Path> paths;

    private Transform background;
    private LookAround lr;
    private SystemInfo si;

    private Tycoon tycoon;
    private TradeItem shipping;

    public Text credits;

    private void Awake() {
        tycoon = new Tycoon();
    }
    // Start is called before the first frame update
    void Start() {
        Random.InitState(GameStarter.seed);
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

        for (int i = 0; i < pathCount; i++) {
            Star start = stars[Random.Range(0, starCountTouchable - 1)].GetComponent<Star>();
            Star end = stars[Random.Range(0, starCountTouchable - 1)].GetComponent<Star>();
            if (start == end) {
                continue;
            }
            SpawnPath(start, end);
        }

        lr = FindObjectOfType<LookAround>();
        if (!lr) {
            return;
        }
        lr.GoToStar(stars[0]);
        stars[0].GetComponent<Star>().SetAsStarter();
        si = FindObjectOfType<SystemInfo>();
        
        shipping = TradeItem.NONE;
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
        if (!lr) {
            return;
        }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 3000f)) {
            Star star = hit.transform.GetComponent<Star>();
            if (star) {
                lr.Aim(star, shipping);
                if (Input.GetMouseButtonDown(0)) {
                    if (shipping == TradeItem.NONE) {
                        lr.GoToStar(star.gameObject);
                        FindObjectOfType<MiniInfo>().Hide();
                    } else {
                        // Let's ship!
                        tycoon.GainMoney(si.ShipTo(star));
                    }
                }
            }
        }

        currentTimeToProduce -= Time.deltaTime;
        while (currentTimeToProduce < 0f) {
            currentTimeToProduce += timeToProduce;
            foreach (GameObject s in stars) {
                s.GetComponent<Star>().AutoProduce();
            }
            si.UpdateOwned();
        }

        Star currentStar = lr.currentStar.GetComponent<Star>();
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (shipping == TradeItem.NONE) {
                lr.GoToStar(tycoon.NextStar(currentStar).gameObject);
            } else {
                if (lr.CurrentDest()) {
                    Star nextDest = tycoon.NextStar(lr.CurrentDest());
                    if (nextDest == currentStar) {
                        nextDest = tycoon.NextStar(nextDest);
                    }
                    lr.Aim(nextDest, shipping);
                } else {
                    lr.Aim(tycoon.NextStar(currentStar), shipping);
                }
            }
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (shipping == TradeItem.NONE) {
                lr.GoToStar(tycoon.PreviousStar(currentStar).gameObject);
            } else {
                if (lr.CurrentDest()) {
                    Star prevDest = tycoon.PreviousStar(lr.CurrentDest());
                    if (prevDest == currentStar) {
                        prevDest = tycoon.PreviousStar(prevDest);
                    }
                    lr.Aim(prevDest, shipping);
                } else {
                    lr.Aim(tycoon.PreviousStar(currentStar), shipping);
                }
            }
        }
        credits.text = tycoon.GetCredits().ToString();
    }
    
    public void AddStarBase(Star s) {
        tycoon.AddStarBase(s);
    }

    public void ChangeShipping(TradeItem ti) {
        shipping = ti;
    }
}
