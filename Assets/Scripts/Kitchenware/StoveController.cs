using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class StoveController : MonoBehaviour {

    public bool isCooking;
    private bool buttonCD;

    public Transform cookingArea;

    public ParticleSystem fireFx;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CheckInside() //Check what Items are inside the microwave & cook it
    {
        isCooking = !isCooking;

        if (isCooking)
        {
            buttonCD = true;
            fireFx.Play();
            Collider[] hitColliders = Physics.OverlapBox(cookingArea.transform.position, cookingArea.transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Items"));
            foreach (Collider item in hitColliders)
            {
                if (item.transform.GetComponent<Food>())
                {
                    item.transform.GetComponent<VRTK_InteractableObject>().isGrabbable = false;
                    StartCoroutine(item.transform.GetComponent<Food>().CookStove(transform));
                }
            }
        }

        else if (!isCooking)
        {
            isCooking = false;
            fireFx.Stop();

            Collider[] hitColliders = Physics.OverlapBox(cookingArea.transform.position, cookingArea.transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Items"));
            foreach (Collider item in hitColliders)
            {
                if (item.transform.GetComponent<Food>())
                {
                    item.transform.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
                }
            }
        }
    }

    public void StopCooking()
    {
        isCooking = false;
        fireFx.Stop();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(cookingArea.position, cookingArea.localScale / 2);
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.GetComponent<Food>() != null)
    //    {
    //        other.gameObject.GetComponent<Food>().StopCooking();
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.GetComponent<Food>() != null)
    //    {
    //        other.gameObject.GetComponent<Food>().EnterCooking();
    //    }
    //}
}
