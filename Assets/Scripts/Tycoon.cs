using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tycoon
{
    private int credits;

    public bool CanPurchase(int cost) {
        return credits >= cost;
    }

    public void Purchase(int cost) {
        credits -= cost;
    }
}
