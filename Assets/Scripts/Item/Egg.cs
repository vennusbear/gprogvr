using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Egg : Food {

    private bool canBreak;
    public bool broken;
    Rigidbody eggRigidbody;
    VRTK_InteractableObject eggObj;
    MeshCollider closeColl;
    BoxCollider openColl;
    public GameObject brokenObj;
    public GameObject closeObj;

    float thrownVelocity;

	// Use this for initialization
	protected override void Start ()
    {
        foodID = 4;
        requiredTime = 30;
        burnBuffer = 10;
        eggRigidbody = GetComponent<Rigidbody>();
        eggObj = GetComponent<VRTK_InteractableObject>();
        closeColl = GetComponent<MeshCollider>();
        openColl = GetComponent<BoxCollider>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (canBreak && !broken)
        {
            if (thrownVelocity >= 1 || other.gameObject.CompareTag("Pan"))
            {
                eggRigidbody.isKinematic = true;
                eggRigidbody.velocity = Vector3.zero;
                broken = true;
                closeColl.enabled = false;
                openColl.enabled = true;
                closeObj.SetActive(false);
                brokenObj.SetActive(true);
                eggObj.isGrabbable = false;
                if (other.gameObject.CompareTag("Pan"))
                {
                    eggRigidbody.mass = 0;
                    eggRigidbody.isKinematic = false;
                    FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
                }
                else
                {
                    eggRigidbody.isKinematic = false;
                    StartCoroutine(DestroyItself());
                }
                foodID += 100;
            }
        }
    }

    IEnumerator DestroyItself()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    public void OnGrab()
    {
        canBreak = false;
    }

    public void OnUngrab()
    {
        if (!canBreak)
        {
            canBreak = true;
        }

        thrownVelocity = eggRigidbody.velocity.sqrMagnitude;
    }

    public override IEnumerator CookStove(Transform stove)
    {
        if (broken)
        {
            cookingRoutine = StartCoroutine(CookCheck());
            while (stove.GetComponent<StoveController>().isCooking && currentState != Food.FoodState.Burned)
            {
                cookedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        else
        {
            cookingRoutine = StartCoroutine(CookCheck());
            while (stove.GetComponent<StoveController>().isCooking && currentState != Food.FoodState.Burned)
            {
                cookedTime += Time.deltaTime;

                if (cookedTime > 5)
                {
                    BurnedFood();
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }

    protected override void BurnedFood()
    {
        currentState = FoodState.Burned;
        brokenObj.GetComponent<MeshRenderer>().material.SetColor("_Color", burnedColor);
        cookedSteam.SetActive(false);
        burnedSmoke.SetActive(true);
        StartCoroutine(DestroyBurned());
    }

    protected override void CookedFood()
    {
        currentState = FoodState.Cooked;
        brokenObj.GetComponent<MeshRenderer>().material.SetColor("_Color", cookedColor);
        cookedSteam.SetActive(true);
    }
}
