using System.Collections.Generic;
using UnityEngine;

public enum TradeItem {
    NONE,
    FUEL,
    PARTS,
    LUXURY,
    FOOD,
    WORKERS
}

class Trading {

    public static Color[] TradeColors = {
        new Color(1f,0.2f,0.2f,0.5f),
        new Color(0.2f,1f,0.2f,0.5f),
        new Color(0.2f,0.2f,1f,0.5f),
        new Color(1f,1f,1f,0.5f),
        Color.yellow,
        Color.cyan
    };
    
    public static int GetTradingWorth(TradeItem ti) {
        switch (ti) {
            case TradeItem.FUEL:
                return 4;
            case TradeItem.PARTS:
                return 2;
            case TradeItem.LUXURY:
                return 10;
            case TradeItem.FOOD:
                return 2;
            case TradeItem.WORKERS:
                return 0;
            default:
                throw new System.Exception("Do not expect to try trading " + ti.ToString());
        }
    }
}