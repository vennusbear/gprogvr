using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameController : MonoBehaviour {

    public enum GameState { Start, Play, End }
    public GameState currentState;

    public int level = 1;
    public List<GameObject> goalItems;
    private List<GameObject> allItems;

    [SerializeField] private TextMeshPro welcomeText;
    [SerializeField] private TextMeshPro grabText;
    [SerializeField] private TextMeshPro doorText;
    [SerializeField] private TextMeshPro buttonText;
    [SerializeField] private TextMeshPro walkText;

    private ClockController clockScript;
    private TutorialTrigger dTrigger;

    void Awake()
    {
        allItems = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
        clockScript = FindObjectOfType<ClockController>();
    }
    
    IEnumerator Start () {
        currentState = GameState.Start;
        welcomeText.enabled = true;
        grabText.enabled = true;
        doorText.enabled = true;
        buttonText.enabled = true;
        walkText.enabled = true;
        yield return new WaitForSeconds(5f);
        StartCoroutine(TextFade(welcomeText, 1));
	}

    public void Begin()
    {
        if (currentState == GameState.Start)
        {
            currentState = GameState.Play;
            LoadGoalItems(1);
            StartCoroutine(TextFade(buttonText, 2));
            StartCoroutine(clockScript.HourMoving("default"));
        }
    }

    void LoadGoalItems(int level)
    {
        switch (level)
        {
            case 1:
                goalItems.Add(allItems.Where(obj => obj.name == "Pizza").SingleOrDefault());
                goalItems.Add(allItems.Where(obj => obj.name == "Milk").SingleOrDefault());
                break;
            case 2:
                break;
            default:
                break;
        }
    }

    IEnumerator TextFade(TextMeshPro text, float speed)
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime * speed)
        {
            text.color = new Color(1, 1, 1, i);
            yield return null;
        }

        text.enabled = false;
    }


    public void DoorTextFade()
    {
        if (doorText.color != new Color(1, 1, 1, 0))
        {
            StartCoroutine(TextFade(doorText, 1));
        }

        if (grabText.color != new Color(1, 1, 1, 0))
        {
            StartCoroutine(TextFade(grabText, 1));
        }

        if (walkText.color != new Color(1, 1, 1, 0))
        {
            StartCoroutine(TextFade(walkText, 1));
        }
    }
}
