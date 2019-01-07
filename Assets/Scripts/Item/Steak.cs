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

    public override IEnumerator CookStove(Transform stove)
    {
        cookingRoutine = StartCoroutine(CookCheck());
        while (stove.GetComponent<StoveController>().isCooking && currentState != Food.FoodState.Burned)
        {
            cookedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
