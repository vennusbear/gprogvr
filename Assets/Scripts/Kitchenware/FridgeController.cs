using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Controllables.ArtificialBased;

public class FridgeController : MonoBehaviour {

    public VRTK_ArtificialRotator fridgeDoor;
    public Transform dropZoneParent;
    public VRTK_SnapDropZone[] dropZones;
    public GameObject[] foodItems;
    bool ready = false;

    Coroutine checkRoutine;

    // Use this for initialization
    IEnumerator Start()
    {
        dropZones = new VRTK_SnapDropZone[dropZoneParent.childCount];
        foodItems = new GameObject[dropZoneParent.childCount];

        yield return new WaitForSeconds(3);
        for (int i = 0; i < dropZones.Length; i++)
        {
            dropZones[i] = dropZoneParent.GetChild(i).GetComponent<VRTK_SnapDropZone>();
            foodItems[i] = dropZones[i].GetCurrentSnappedObject();
        }

        ready = true;
    }

    public void SpawnFood()
    {
        if (ready)
        {
            for (int i = 0; i < dropZones.Length; i++)
            {
                if (dropZones[i].GetCurrentSnappedObject() == null)
                {
                    GameObject foodClone = Instantiate(foodItems[i], dropZones[i].transform);
                    dropZones[i].snapDuration = 0;
                    dropZones[i].ForceSnap(foodClone);
                    dropZones[i].snapDuration = 0.1f;
                }
            }
        }
    }
}
