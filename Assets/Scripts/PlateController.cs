using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Food")
    //    {
    //        Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
    //        BoxCollider collider = collision.gameObject.GetComponent<BoxCollider>();
    //        HingeJoint hj = new HingeJoint();
    //        hj = gameObject.AddComponent<HingeJoint>();
    //        hj.connectedBody = collision.rigidbody;
    //        rigidbody.mass = 0.00001f;
    //        collider.material.bounciness = 0;
    //        rigidbody.freezeRotation = true;
    //        rigidbody.velocity = new Vector3(0, 0, 0);
    //    }
    //}
}
