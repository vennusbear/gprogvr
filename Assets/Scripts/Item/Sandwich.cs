using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandwich : ComplexFood {

    // Use this for initialization
    protected override void Start()
    {
        recipeID = new int[3];
        recipeID[0] = 5;
        recipeID[1] = 6;
        recipeID[2] = 7;
        foodID = 8;

        CookedFood();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
