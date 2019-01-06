﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : Food {

    // Use this for initialization
    protected override void Start()
    {
        foodID = 2;
        requiredTime = 5;
        burnBuffer = 0;
        currentState = FoodState.Cooked;
        FoodColor(rawColor);
    }

    public override IEnumerator CookMicrowave(Transform microwave)
    {
        StartCoroutine(CookCheck());
        while (microwave.GetComponent<MicrowaveController>().isCooking)
        {
            cookedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    protected override IEnumerator CookCheck()
    {
        while (currentState != FoodState.Burned)
        {
            if (cookedTime >= requiredTime)
            {
                currentState = FoodState.Burned;
                burnedSmoke.SetActive(true);
                break;
            }

            yield return null;
        }
    }
}
