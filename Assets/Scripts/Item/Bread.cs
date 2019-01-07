using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : Food {

    protected override void Start()
    {
        cookedColor = new Color32(255, 167, 0, 255);
        foodID = 5;
        requiredTime = 60;
        burnBuffer = 15;

    }

    public override IEnumerator CookMicrowave(Transform microwave)
    {
        cookingRoutine = StartCoroutine(CookCheck());
        while (microwave.GetComponent<MicrowaveController>().isCooking)
        {
            cookedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
