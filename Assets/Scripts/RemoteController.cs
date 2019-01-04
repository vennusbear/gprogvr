using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RemoteController : MonoBehaviour {

    VRTK_StraightPointerRenderer sprScript;
    VRTK_UIPointer uipScript;
    VRTK_Pointer pScript;

    public GameObject rightController;

	// Use this for initialization
	void Start ()
    {
        sprScript = rightController.GetComponent<VRTK_StraightPointerRenderer>();
        uipScript = rightController.GetComponent<VRTK_UIPointer>();
        pScript = rightController.GetComponent<VRTK_Pointer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnOffPointer(bool on)
    {
        if (on)
        {
            sprScript.enabled = true;
            uipScript.enabled = true;
            pScript.enabled = true;
        }

        else
        {
            sprScript.enabled = false;
            uipScript.enabled = false; ;
            pScript.enabled = false;
        }
    }
}
