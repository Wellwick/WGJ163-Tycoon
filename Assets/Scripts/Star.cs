using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private string name;
    private TradeItem tradeItem;

    public string GetName() {
        return name;
    }

    public TradeItem GetTradeItem() {
        return tradeItem;
    }

    // Start is called before the first frame update
    void Awake()
    {
        name = NameGenerator.StarName();
        Material m = GetComponent<MeshRenderer>().materials[0];
        Color c = new Color(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
        m.color = c;
        m.SetColor("_EmissionColor", c);
        float scale = Random.Range(3f / 5f, 5f / 3f);
        transform.localScale = new Vector3(scale, scale, scale);
        tradeItem = (TradeItem)Random.Range(1, 6);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
