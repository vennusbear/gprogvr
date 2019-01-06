using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Egg : MonoBehaviour {

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
	void Start ()
    {
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
                    transform.SetParent(other.transform);
                }
                else
                {
                    eggRigidbody.isKinematic = false;
                    StartCoroutine(DestroyItself());
                }
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
}
