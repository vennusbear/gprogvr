using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class StoveController : MonoBehaviour {

    private Transform cookingArea;

    public bool isCooking;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CheckInside() //Check what Items are inside the microwave & cook it
    {
        Collider[] hitColliders = Physics.OverlapBox(cookingArea.transform.position, cookingArea.transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Items"));
        foreach (Collider item in hitColliders)
        {
            if (item.transform.GetComponent<Egg>())
            {
                StartCoroutine(item.transform.GetComponent<Egg>().CookStove(transform));
            }
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(cookingArea.position, cookingArea.localScale / 2);
    //}
}
