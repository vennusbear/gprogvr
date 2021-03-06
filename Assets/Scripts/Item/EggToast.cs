﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggToast : ComplexFood {

    public MeshRenderer breadRend;

    // Use this for initialization
    protected override void Start()
    {
        recipeID.Clear();
        recipeID.Add(104);
        recipeID.Add(5);
        foodID = 9;

        cookedColor = new Color32(255, 167, 0, 255);
        CookedFood();
    }

    // Update is called once per frame
    void Update () {
		
	}

    protected override void CookedFood()
    {
        currentState = FoodState.Cooked;
        breadRend.material.SetColor("_Color", cookedColor);
        cookedSteam.SetActive(true);
    }
}
