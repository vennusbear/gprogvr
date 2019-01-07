using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class CombinerController : MonoBehaviour {

    public LayerMask m_LayerMask;
    public Transform scanArea;
    public Transform references;
    public Transform spawnPos;
    public List<FoodRecipe> foodRecipes;

    [SerializeField] List<int> idInside = new List<int>();
    List<int> missingID = new List<int>();

    GameController gameScript;

    public TextMeshProUGUI textComp;

    [System.Serializable]
    public class FoodRecipe {
        public GameObject foodObject;
        public int[] recipeID;

        public FoodRecipe(int[] numberRecipe, GameObject foodObj)
        {
            recipeID = numberRecipe;
            foodObject = foodObj;
        }
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

    public void GetFoodInRadius()
    {
        print("Combiner Checking");
        if (gameScript.currentState == GameController.GameState.Play)
        {
            idInside.Clear();
            Collider[] hitColliders = Physics.OverlapBox(scanArea.position, scanArea.localScale / 2, Quaternion.identity, m_LayerMask);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.GetComponent<Food>())
                {
                    idInside.Add(hitColliders[i].gameObject.GetComponent<Food>().foodID);
                }
            }

            foreach (FoodRecipe fr in foodRecipes)
            {
                missingID.Clear();
                missingID = fr.recipeID.Except(idInside).ToList();

                if (missingID.Count == 0)
                {
                    GameObject spawnee = Instantiate(fr.foodObject, spawnPos.position, Quaternion.identity);
                    textComp.text = fr.foodObject.name;
                    break;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(scanArea.position, scanArea.localScale / 2);
    }

}
