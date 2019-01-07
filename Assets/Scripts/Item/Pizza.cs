using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Pizza : Food {

    protected override void Start()
    {
        foodID = 1;
        requiredTime = 30;
        burnBuffer = 5;
        FoodColor(rawColor);
    }
    // Use this for initialization
    //void Start ()
    //   {
    //       VRTKpizza = GetComponent<VRTK_InteractableObject>();
    //       if (currentState == FoodState.Raw)
    //       {
    //           gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    //       }
    //   }

    //   //void OnTriggerExit(Collider collision)
    //   //{
    //   //    if (collision.tag == "Plate")
    //   //    {
    //   //        transform.parent = null;

    //   //        if (cookingRoutine != null)
    //   //        {
    //   //            StopCoroutine(cookingRoutine);
    //   //            cookingRoutine = null;
    //   //        }
    //   //    }

    //   //    if (collision.tag == "CookingArea")
    //   //    {

    //   //    }
    //   //}

    public override IEnumerator CookMicrowave(Transform microwave)
    {
        cookingRoutine = StartCoroutine(CookCheck());
        while (microwave.GetComponent<MicrowaveController>().isCooking)
        {
            cookedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    //   IEnumerator CookCheck()
    //   {
    //       while (currentState == FoodState.Raw)
    //       {
    //           if (cookedTime >= requiredTime)
    //           {
    //               currentState = FoodState.Cooked;
    //               cookedSteam.SetActive(true);
    //               break;
    //           }

    //           yield return null;
    //       }

    //       while (currentState == FoodState.Cooked)
    //       {
    //           if (cookedTime >= requiredTime + burnBuffer)
    //           {
    //               currentState = FoodState.Burned;
    //               cookedSteam.SetActive(false);
    //               burnedSmoke.SetActive(true);
    //               break;
    //           }

    //           yield return null;
    //       }
    //   }
}
