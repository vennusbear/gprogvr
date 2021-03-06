﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public enum GameState { Start, Play, End }
    public GameState currentState;

    public float gameTime;

    public int level = 1;
    public List<GameObject> goalItems = new List<GameObject>();
    private List<Food> allFood = new List<Food>();
    private List<GameObject> allItems = new List<GameObject>();

    [SerializeField] private TextMeshPro welcomeText;
    [SerializeField] private TextMeshPro grabText;
    [SerializeField] private TextMeshPro doorText;
    [SerializeField] private TextMeshPro buttonText;
    [SerializeField] private TextMeshPro walkText;

    private AIController aiScript;
    bool aiSpawned;
    int spawnTime;

    private ClockController clockScript;
    private TableController tableScript;

    private void Awake()
    {
        allFood = FindObjectsOfType<Food>().ToList();
        for (int i = 0; i < allFood.Count; i++)
        {
            allItems.Add(allFood[i].gameObject);
        }
        tableScript = FindObjectOfType<TableController>();
        clockScript = FindObjectOfType<ClockController>();
        aiScript = FindObjectOfType<AIController>();
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

    private void Update()
    {
        if (currentState == GameState.Play)
        {
            gameTime += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeGameScene(0);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Time.timeScale = 5;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Time.timeScale = 0.3f;
        }
    }

    public void Begin()
    {
        if (currentState == GameState.Start)
        {
            currentState = GameState.Play;
            LoadLevelItems(level);
            tableScript.GetFoodID();
            StartCoroutine(TextFade(buttonText, 2));
            StartCoroutine(SpawnEnemy());
        }
    }

    void LoadLevelItems(int level)
    {
        switch (level)
        {
            case 1:
                spawnTime = 60;
                StartCoroutine(clockScript.HourMoving("Morning"));
                goalItems.Add(allItems.Where(obj => obj.name == "Sandwich").SingleOrDefault());
                goalItems.Add(allItems.Where(obj => obj.name == "Milk").SingleOrDefault());
                break;
            case 2:
                spawnTime = 30;
                StartCoroutine(clockScript.HourMoving("Afternoon"));
                goalItems.Add(allItems.Where(obj => obj.name == "OrangeJuice").SingleOrDefault());
                goalItems.Add(allItems.Where(obj => obj.name == "Pizza").SingleOrDefault());
                goalItems.Add(allItems.Where(obj => obj.name == "EggToast").SingleOrDefault());
                goalItems.Add(allItems.Where(obj => obj.name == "Steak").SingleOrDefault());
                break;
            default:
                break;
        }
    }

    IEnumerator SpawnEnemy()
    {
        while (gameTime < spawnTime)
        {
            yield return null;
        }
        StartCoroutine(aiScript.StartAI());
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

    public IEnumerator Victory()
    {
        currentState = GameState.End;
        yield return new WaitForSeconds(10);
        ChangeGameScene(0);
    }

    public void ChangeGameScene(int i)
    {
        SceneManager.LoadScene(i);
    }
}
