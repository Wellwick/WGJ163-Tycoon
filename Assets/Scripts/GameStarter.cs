using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public Button startGameButton;
    public InputField seedField;
    public float speed;
    public static int seed = 1234567890;
    // Start is called before the first frame update
    void Start()
    {
        startGameButton.onClick.AddListener(TriggerLoad);
    }

    private void TriggerLoad() {
        GameStarter.StartGame(seedField.text.GetHashCode());
    }

    private void Update() {
        transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * speed);
    }

    public static void StartGame(int inputSeed) {
        seed = inputSeed;
        Debug.Log("The seed is " + seed);
        SceneManager.LoadScene("Default");
    }
}
