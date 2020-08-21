using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Material m = GetComponent<MeshRenderer>().materials[0];
        Color c = new Color(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
        m.color = c;
        m.SetColor("_EmissionColor", c);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
