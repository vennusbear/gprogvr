﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange : Food {


    // Use this for initialization
    protected override void Start()
    {
        foodID = 10;
        requiredTime = 5;
        burnBuffer = 1;
        CookedFood();
    }

    protected override void CookedFood()
    {
        currentState = FoodState.Cooked;
    }
}
