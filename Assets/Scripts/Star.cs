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
        float scale = Random.Range(4f / 5f, 5f / 4f);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
