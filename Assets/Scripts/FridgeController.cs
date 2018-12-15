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
        isChecking = true;
        pizza = pizzaDrop.highlightObjectPrefab;
        StartCoroutine(Delay(1));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BeginCheckRot()
    {
        if (!isChecking)
        {
            checkRoutine = StartCoroutine(CheckRot());
        }
    }

    public void StopCheckRot()
    {
        if (checkRoutine != null)
        {
            StopCoroutine(checkRoutine);
            isChecking = false;
            checkRoutine = null;
        }
    }

    IEnumerator CheckRot()
    {
        isChecking = true;
        float doorRot = 0;
        while (true)
        {
            doorRot = fridgeDoor.GetNormalizedValue();
            if (doorRot >= 0.9)
            {
                SpawnFood(pizza);
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        checkRoutine = null;
    }
    
    void SpawnFood(GameObject food)
    {
        GameObject pizzaClone = Instantiate(pizza, pizzaDrop.transform);
        pizzaDrop.ForceSnap(pizzaClone);
        isChecking = false;
    }

    IEnumerator Delay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        isChecking = false;
    } //To overcome the weird bug that causes the buttons to be pushed at the start of the scene 

}
