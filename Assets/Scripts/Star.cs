using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{

    private string name;
    private TradeItem tradeItem;
    private float purchasePower;

    private long factories, freighters, workers, food, parts, fuel, luxuries;
    private List<Path> outgoingTrades, incomingTrades;

    public string GetName() {
        return name;
    }

    public TradeItem GetTradeItem() {
        return tradeItem;
    }

    public void SetAsStarter() {
        parts = 3;
        factories = 3;
        workers = 4;
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
        factories = 0;
        freighters = 0;
        workers = 0;
        food = 0;
        parts = 0;
        CalculatePurchasePower();
        outgoingTrades = new List<Path>();
        incomingTrades = new List<Path>();
    }

    private void CalculatePurchasePower() {
        purchasePower = 1.0f;
    }

    public long ProductionRate() {
        if (tradeItem == TradeItem.WORKERS) {
            return factories;
        } else {
            if (workers > 0) {
                return factories * workers;
            } else {
                return factories;
            }
        }
    }

    public void RunProduction() {
        long count = ProductionRate();
        switch (tradeItem) {
            case TradeItem.FUEL:
                fuel += count;
                break;
            case TradeItem.PARTS:
                parts += count;
                break;
            case TradeItem.LUXURY:
                luxuries += count;
                break;
            case TradeItem.FOOD:
                food += count;
                break;
            case TradeItem.WORKERS:
                workers += count;
                break;
            default:
                throw new System.Exception("Not expecting TradeItem to be None");
        }
    }

    public int DeliverShipments(TradeItem ti, int amount) {
        switch (ti) {
            case TradeItem.FOOD:
                food += amount;
                break;
            case TradeItem.PARTS:
                parts += amount;
                break;
            case TradeItem.WORKERS:
                workers += amount;
                break;
            case TradeItem.FUEL:
                fuel += amount;
                break;
            case TradeItem.LUXURY:
                return PurchasePrice(ti, amount);
            default:
                throw new System.Exception("Did not get expected shipment");
        }
        return PurchasePrice(ti, amount);
    }

    public int PurchasePrice(TradeItem ti, int amount) {
        return (int)purchasePower * amount;
    }

    public string GetFactoryRequirement() {
        if (tradeItem == TradeItem.WORKERS) {
            return "Food";
        } else {
            return "Parts";
        }
    }

    public string GetFactoryName() {
        if (tradeItem == TradeItem.WORKERS) {
            return "Farm";
        } else {
            return "Factory";
        }
    }

    public long GetBuildableFactories() {
        if (tradeItem == TradeItem.WORKERS) {
            return food;
        } else {
            return parts;
        }
    }

    public long GetFactories() {
        return factories;
    }

    public bool IsAutoProduction() {
        return workers > 0;
    }

    public void AddFactories() {
        if (tradeItem == TradeItem.WORKERS) {
            factories += food;
            food = 0;
        } else {
            factories += parts;
            parts = 0;
        }
    }

    public void AutoProduce() {
        if (!IsAutoProduction()) {
            return;
        }
        RunProduction();
    }

    public long GetResource(TradeItem ti) {
        switch (ti) {
            case TradeItem.FOOD:
                return food;
            case TradeItem.PARTS:
                return parts;
            case TradeItem.WORKERS:
                return workers;
            case TradeItem.FUEL:
                return fuel;
            case TradeItem.LUXURY:
                return luxuries;
            default:
                throw new System.Exception("Did not try and get NONE");
        }
    }
}
