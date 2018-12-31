using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Controllables.ArtificialBased;

public class FridgeController : MonoBehaviour {

    public VRTK_ArtificialRotator fridgeDoor;
    public VRTK_SnapDropZone pizzaDrop;
    private GameObject pizza;
    public bool isChecking;

    Coroutine checkRoutine;

    // Use this for initialization
    void Start ()
    {
        pizza = pizzaDrop.highlightObjectPrefab;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //public void BeginCheckRot()
    //{
    //    if (!isChecking)
    //    {
    //        checkRoutine = StartCoroutine(CheckRot());
    //    }
    //}

    //public void StopCheckRot()
    //{
    //    if (checkRoutine != null)
    //    {
    //        StopCoroutine(checkRoutine);
    //        isChecking = false;
    //        checkRoutine = null;
    //    }
    //}

    //IEnumerator CheckRot()
    //{
    //    isChecking = true;
    //    float doorRot = 0;
    //    while (true)
    //    {
    //        doorRot = fridgeDoor.GetNormalizedValue();
    //        if (doorRot >= 0.9)
    //        {
    //            SpawnFood();
    //            break;
    //        }
    //        yield return new WaitForEndOfFrame();
    //    }

    //    checkRoutine = null;
    //}
    
    public void SpawnFood()
    {
        if (pizzaDrop.GetCurrentSnappedObject() == null)
        {
            GameObject pizzaClone = Instantiate(pizza, pizzaDrop.transform);
            pizzaDrop.ForceSnap(pizzaClone);
        }
    }
}
