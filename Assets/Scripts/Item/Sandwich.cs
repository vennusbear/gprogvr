using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandwich : ComplexFood {

    // Use this for initialization
    protected override void Start()
    {
        recipeID.Clear();
        recipeID.Add(5);
        recipeID.Add(5);
        recipeID.Add(6);
        recipeID.Add(7);
        foodID = 8;

        CookedFood();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
