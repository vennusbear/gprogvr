using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Food : MonoBehaviour {

    public float cookedTime;
    protected int requiredTime = 10;
    protected int burnBuffer = 5;

    public FoodState currentState;
    public enum FoodState { Raw, Cooked, Burned }

    public GameObject cookedSteam;
    public GameObject burnedSmoke;

    protected Color rawColor = new Color32(184, 255, 241, 255);
    protected Color cookedColor = Color.white;
    protected Color burnedColor = new Color32(80, 80, 80, 255);

    Coroutine cookingRoutine;

    // Use this for initialization
    protected virtual void Start ()
    {
        FoodColor(rawColor);
    }
    
    protected virtual void FoodColor(Color color)
    {
        print(gameObject.GetComponent<MeshRenderer>().material);
        gameObject.GetComponent<MeshRenderer>().material.color = color;
    }

    //protected virtual void OnTriggerExit(Collider collision)
    //{
    //    if (collision.tag == "Plate")
    //    {
    //        transform.parent = null;

    //        if (cookingRoutine != null)
    //        {
    //            StopCoroutine(cookingRoutine);
    //            cookingRoutine = null;
    //        }
    //    }

    //    if (collision.tag == "CookingArea")
    //    {

    //    }
    //}

    public virtual IEnumerator CookMicrowave(Transform microwave)
    {
        StartCoroutine(CookCheck());
        while (microwave.GetComponent<MicrowaveController>().isCooking)
        {
            cookedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    protected virtual IEnumerator CookCheck()
    {
        while (currentState == FoodState.Raw)
        {
            if (cookedTime >= requiredTime)
            {
                currentState = FoodState.Cooked;
                gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", cookedColor);
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
                gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", burnedColor);
                cookedSteam.SetActive(false);
                burnedSmoke.SetActive(true);
                break;
            }

            yield return null;
        }
    }
}
