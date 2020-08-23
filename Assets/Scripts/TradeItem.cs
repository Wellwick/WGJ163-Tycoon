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
}