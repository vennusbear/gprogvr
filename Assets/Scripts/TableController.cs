using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TableController : MonoBehaviour {

    private GameController gameScript;
    public GameObject victoryText;
    public bool clear;
    [SerializeField] List<int> requiredID = new List<int>();
    List<int> idInside = new List<int>();
    [SerializeField] List<int> missingID = new List<int>();
    List<GameObject> itemsInside = new List<GameObject>();

    public LayerMask m_LayerMask;
    private Transform scanArea;
    bool m_Started;

	// Use this for initialization
	void Start () {
        gameScript = FindObjectOfType<GameController>().gameObject.GetComponent<GameController>();
        m_Started = true;
        scanArea = transform.GetChild(0);
	}

    public void GetFoodID()
    {
        for (int i = 0; i < gameScript.goalItems.Count; i++)
        {
            requiredID.Add(gameScript.goalItems[i].GetComponent<Food>().foodID);
        }
    }
	

    void GetFoodInRadius()
    {
        if (gameScript.currentState == GameController.GameState.Play)
        {
            Collider[] hitColliders = Physics.OverlapBox(scanArea.position, scanArea.localScale / 2, Quaternion.identity, m_LayerMask);
            itemsInside.Clear();
            idInside.Clear();
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (requiredID.Contains(hitColliders[i].gameObject.GetComponent<Food>().foodID))
                {
                    if (hitColliders[i].gameObject.GetComponent<Food>().currentState == Food.FoodState.Cooked)
                    {
                        itemsInside.Add(hitColliders[i].gameObject);
                    }
                }
            }

            for (int i = 0; i < itemsInside.Count; i++)
            {
                idInside.Add(itemsInside[i].gameObject.GetComponent<Food>().foodID);
            }

            missingID = requiredID.Except(idInside).ToList();

            if (itemsInside.Count >= requiredID.Count)
            {
                if (missingID.Count == 0)
                {
                    StartCoroutine(gameScript.Victory());
                    victoryText.SetActive(true);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (m_Started)
        {
            Gizmos.DrawWireCube(scanArea.position, scanArea.localScale / 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Food>() != null)
        {
            GetFoodInRadius();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Food>() != null)
        {
            Food tempScript = other.gameObject.GetComponent<Food>();
            for (int i = 0; i < itemsInside.Count; i++)
            {
                if (other.gameObject == itemsInside[i])
                {
                    itemsInside.Remove(itemsInside[i]);

                    if (idInside.Contains(tempScript.foodID))
                    {
                        idInside.Remove(tempScript.foodID);
                    }
                }
            }
        }
    }
}
