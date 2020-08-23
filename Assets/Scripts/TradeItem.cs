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
        new Color(1f,0.2f,0.2f,1f),
        Color.green,
        Color.blue,
        Color.white,
        Color.yellow,
        Color.cyan
    };
}