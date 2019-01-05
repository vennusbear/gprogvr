using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodGoal {

    public string prefix;
    public string foodName;

    public FoodGoal(string nm, string pfx)
    {
        prefix = pfx;
        foodName = nm;
    }
}
