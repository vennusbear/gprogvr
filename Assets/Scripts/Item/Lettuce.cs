using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lettuce : Food{

    protected override void Start()
    {
        foodID = 6;
        requiredTime = 5;
        burnBuffer = 1;

        CookedFood();
    }

    // Update is called once per frame
    void Update () {
		
	}

    protected override void CookedFood()
    {
        currentState = FoodState.Cooked;
    }
}
