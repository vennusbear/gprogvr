using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using VRTK.Controllables.ArtificialBased;

public class CombinerController : MonoBehaviour {

    public LayerMask m_LayerMask;
    public Transform scanArea;
    public Transform references;
    public Transform spawnPos;
    public VRTK_ArtificialRotator doorScript;
    public List<FoodRecipe> foodRecipes;

    [SerializeField] List<int> idInside = new List<int>();
    List<GameObject> objectInside = new List<GameObject>();

    GameController gameScript;

    public TextMeshProUGUI textComp;

    bool combining;

    Coroutine scanningRoutine;

    AudioSource audioScript;
    public AudioClip[] sound;

    [System.Serializable]
    public class FoodRecipe {
        public GameObject foodObject;
        public List<int> recipeID;

        public FoodRecipe(List<int> numberRecipe, GameObject foodObj)
        {
            recipeID = numberRecipe;
            foodObject = foodObj;
        }
    }

    private void Awake()
    {
        audioScript = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start ()
    {
        gameScript = FindObjectOfType<GameController>().gameObject.GetComponent<GameController>();
        Transform[] children = new Transform[references.childCount];
        for (int i = 0; i < references.childCount; i++)
        {
            children[i] = references.GetChild(i);
        }

        foreach (Transform child in children)
        {
            FoodRecipe foodRep = new FoodRecipe(child.GetComponent<ComplexFood>().recipeID, child.gameObject);
            foodRecipes.Add(foodRep);
        }


    }

    private void Update()
    {
        if (combining && !audioScript.isPlaying)
        {
            audioScript.loop = true;
            audioScript.clip = sound[0];
            audioScript.Play();
        }
    }

    void DoorClosed()
    {
        if (scanningRoutine != null)
        {
            StopCoroutine(scanningRoutine);
        }
    }


    void GetFoodInRadius()
    {
        if (gameScript.currentState == GameController.GameState.Play)
        {
            idInside.Clear();
            objectInside.Clear();
            Collider[] hitColliders = Physics.OverlapBox(scanArea.position, scanArea.localScale / 2, Quaternion.identity, m_LayerMask);
            for (int i = 0; i < hitColliders.Length; i++) //Add
            {
                if (hitColliders[i].gameObject.GetComponent<Food>() && hitColliders[i].gameObject.GetComponent<Food>().currentState == Food.FoodState.Cooked)
                {
                    objectInside.Add(hitColliders[i].gameObject);
                    idInside.Add(hitColliders[i].gameObject.GetComponent<Food>().foodID);
                }
            }

            foreach (FoodRecipe fr in foodRecipes)
            {
                if (CompareLists(idInside, fr.recipeID))
                {
                    StartCoroutine(Combining(fr));
                }
            }
        }
    }

    IEnumerator Combining(FoodRecipe fr)
    {
        combining = true;
        doorScript.isLocked = true;
        textComp.text = "Combining...";
        yield return new WaitForSeconds(15);
        doorScript.isLocked = false;
        GameObject spawnee = Instantiate(fr.foodObject, spawnPos.position, Quaternion.identity);
        spawnee.transform.SetParent(spawnPos);
        textComp.text = fr.foodObject.name;
        DestroyUsedObjects(fr);
        combining = false;
        audioScript.loop = false;
        audioScript.PlayOneShot(sound[1]);
    }

    void DestroyUsedObjects(FoodRecipe fr)
    {
        for (int i = 0; i < objectInside.Count; i++) //Destroy
        {
            if (fr.recipeID.Contains(objectInside[i].gameObject.GetComponent<Food>().foodID))
            {
                Destroy(objectInside[i].gameObject);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(scanArea.position, scanArea.localScale / 2);
    }

    public static bool CompareLists<T>(List<T> aListA, List<T> aListB)
    {
        if (aListA == null || aListB == null || aListA.Count != aListB.Count)
            return false;
        if (aListA.Count == 0)
            return true;
        Dictionary<T, int> lookUp = new Dictionary<T, int>();
        // create index for the first list
        for (int i = 0; i < aListA.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(aListA[i], out count))
            {
                lookUp.Add(aListA[i], 1);
                continue;
            }
            lookUp[aListA[i]] = count + 1;
        }
        for (int i = 0; i < aListB.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(aListB[i], out count))
            {
                // early exit as the current value in B doesn't exist in the lookUp (and not in ListA)
                return false;
            }
            count--;
            if (count <= 0)
                lookUp.Remove(aListB[i]);
            else
                lookUp[aListB[i]] = count;
        }
        // if there are remaining elements in the lookUp, that means ListA contains elements that do not exist in ListB
        return lookUp.Count == 0;
    }

}
