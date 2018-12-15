using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Pizza : MonoBehaviour {

    public float cookedTime;
    private int requiredTime = 10;
    private int burnBuffer = 5;

    VRTK_InteractableObject VRTKpizza;

    public FoodState currentState;
    public enum FoodState { Raw, Cooked, Burned }

    public GameObject cookedSteam;
    public GameObject burnedSmoke;

    Coroutine cookingRoutine;

	// Use this for initialization
	void Start ()
    {
        VRTKpizza = GetComponent<VRTK_InteractableObject>();
    }
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter(Collider collision)
    {

    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Plate")
        {
            transform.parent = null;

            if (cookingRoutine != null)
            {
                StopCoroutine(cookingRoutine);
                cookingRoutine = null;
            }
        }

        if (collision.tag == "CookingArea")
        {

        }
    }

    public IEnumerator CookMicrowave(Transform microwave)
    {
        StartCoroutine(CookCheck());
        while (microwave.GetComponent<MicrowaveController>().isCooking)
        {
            cookedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CookCheck()
    {
        while (currentState == FoodState.Raw)
        {
            if (cookedTime >= requiredTime)
            {
                currentState = FoodState.Cooked;
                cookedSteam.SetActive(true);
                break;
            }

            yield return null;
        }

        while (currentState == FoodState.Cooked)
        {
            if (cookedTime >= requiredTime + burnBuffer)
            {
                currentState = FoodState.Burned;
                cookedSteam.SetActive(false);
                burnedSmoke.SetActive(true);
                break;
            }

            yield return null;
        }
    }
}
