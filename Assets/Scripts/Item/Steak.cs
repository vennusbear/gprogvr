using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steak : Food {

    // Use this for initialization
    protected override void Start()
    {
        foodID = 3;
        requiredTime = 60;
        burnBuffer = 15;
        FoodColor(rawColor);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
