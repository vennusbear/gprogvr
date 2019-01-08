using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Controllables.ArtificialBased;

public class FridgeController : MonoBehaviour {

    public VRTK_ArtificialRotator fridgeDoor;
    public Transform[] dropZoneParent;
    public List<VRTK_SnapDropZone> dropZones;
    public List<GameObject> foodItems = new List<GameObject>();
    bool ready = false;

    private GameController gameScript;

    Coroutine checkRoutine;

    #region //Audio 
    AudioSource audioScript;
    public AudioClip[] fridgeAudio;
    #endregion

    // Use this for initialization
    IEnumerator Start()
    {
        gameScript = FindObjectOfType<GameController>().gameObject.GetComponent<GameController>();
        audioScript = GetComponent<AudioSource>();
        fridgeDoor.isLocked = true;
        for (int i = 0; i < dropZoneParent[0].childCount; i++)
        {
            dropZones.Add(dropZoneParent[0].GetChild(i).GetComponent<VRTK_SnapDropZone>());
        }

        for (int i = 0; i < dropZoneParent[1].childCount; i++)
        {
            dropZones.Add(dropZoneParent[1].GetChild(i).GetComponent<VRTK_SnapDropZone>());
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < dropZones.Count; i++)
        {
            GameObject reference = Instantiate(dropZones[i].GetCurrentSnappedObject(), Vector3.zero, Quaternion.identity);
            foodItems.Add(reference);
        }

        StartCoroutine(WaitPlayToUnlock());
        ready = true;
    }

    IEnumerator WaitPlayToUnlock()
    {
        while (gameScript.currentState != GameController.GameState.Play)
        {
            yield return null;
        }
        fridgeDoor.isLocked = false;
    }

    public void SpawnFood()
    {
        if (ready)
        {
            for (int i = 0; i < dropZones.Count; i++)
            {
                if (dropZones[i].GetCurrentSnappedObject() == null)
                {
                    GameObject foodClone = Instantiate(foodItems[i], dropZones[i].transform);
                    foodClone.GetComponent<Rigidbody>().isKinematic = false;
                    dropZones[i].snapDuration = 0;
                    dropZones[i].ForceSnap(foodClone);
                    dropZones[i].snapDuration = 0.1f;
                }
            }
        }
    }

    public void PlayAudio(int i)
    {
        audioScript.clip = fridgeAudio[i];
        audioScript.Play();
    }
}
