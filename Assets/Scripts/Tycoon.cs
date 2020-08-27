using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tycoon
{
    private long credits;
    private LinkedList<Star> starBases;

    public Tycoon() {
        starBases = new LinkedList<Star>();
        credits = 0;
    }

    public void AddStarBase(Star s) {
        if (!starBases.Contains(s)) {
            starBases.AddLast(s);
        }
    }

    public Star NextStar(Star s) {
        if (starBases.Contains(s)) {
            if (s == starBases.Last.Value) {
                return starBases.First.Value;
            } else {
                return starBases.Find(s).Next.Value;
            }
        } else {
            return starBases.First.Value;
        }
    }

    public Star PreviousStar(Star s) {
        if (starBases.Contains(s)) {
            if (s == starBases.First.Value) {
                return starBases.Last.Value;
            } else {
                return starBases.Find(s).Previous.Value;
            }
        } else {
            return starBases.First.Value;
        }
    }

    public bool CanPurchase(long cost) {
        return credits >= cost;
    }

    public void Purchase(long cost) {
        credits -= cost;
    }

    public void GainMoney(long amount) {
        credits += amount;
    }

    public long GetCredits() {
        return credits;
    }
}
