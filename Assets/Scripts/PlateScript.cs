using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlateScript : MonoBehaviour {

    VRTK_InteractableObject vrtkIntObj;
    Rigidbody plateRB;

	// Use this for initialization
	void Start () {
        vrtkIntObj = GetComponent<VRTK_InteractableObject>();
        plateRB = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
