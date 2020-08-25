using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator
{
    private static string[] alpStar = {
        "Alpha ", "Gamma ", "Beta ", "Omega ", "Zeta "
    };

    private static string[] preStar = {
        "Ald", "Alk", "Atl", "Cap", "Cho", "Den", "Ed", "Forn", "Gorg", "Hoed",
        "Mai", "Pha", "Salm", "Sad", "Sir", "Can", "Bet", "Pro", "Rig"
    };

    private static string[] midStar = {
        "sal", "on", "bit", "em", "ell", "ar", "o", "tu", "eba"
    };

    private static string[] endStar = {
        "a", "e", "ath", "leh", "ea", "jam", "fak", "kar", "et", "r", "dun",
        "far", "ius", "air", "nar", "pus", "nus", "ga", "rus", "el"
    };

    private static string[] indice = {
        " A", " B", " C", " I", " II", " III", " IV", " V"
    };

    public static string StarName() {
        string name = "";
        if (Random.value < 0.2f) {
            name += alpStar[Random.Range(0, alpStar.Length - 1)];
        }
        name += preStar[Random.Range(0, preStar.Length - 1)];
        while (Random.value < 0.25f) {
            name += midStar[Random.Range(0, midStar.Length - 1)];
        }
        if (Random.value < 0.85f) {
            name += endStar[Random.Range(0, endStar.Length - 1)];
        }
        if (Random.value < 0.15f) {
            name += indice[Random.Range(0, indice.Length - 1)];
        }
        return name;
    }
}
