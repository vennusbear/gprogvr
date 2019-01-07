using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeJuice : ComplexFood {

    // Use this for initialization
    protected override void Start()
    {
        recipeID.Clear();
        recipeID.Add(10);
        recipeID.Add(10);
        recipeID.Add(10);
        foodID = 11;

        CookedFood();
    }

    protected override void CookedFood()
    {
        currentState = FoodState.Cooked;
    }
}
