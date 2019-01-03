using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {

    public enum GameState { Start, Play, End }
    public GameState currentState;

    public float gameTime;

    public GameObject TVObject;
    //private Vector3 butPos;
    //public GameObject TVButton;
    private Vector3 tvPos;

    public GameObject ButtonObject;

    private DialogueTrigger dTrigger;

    public TextMeshPro welcomeText;
    public TextMeshPro doorText;
    public TextMeshPro buttonText;

    private ClockController clockScript;

    //private bool clicked;

    void Awake()
    {
        clockScript = FindObjectOfType<ClockController>();
    }
    
    IEnumerator Start () {
        currentState = GameState.Start;
        welcomeText.enabled = true;
        doorText.enabled = true;
        buttonText.enabled = true;
        dTrigger = GetComponent<DialogueTrigger>();
        tvPos = TVObject.transform.position;
        TVObject.transform.position = new Vector3(TVObject.transform.position.x, TVObject.transform.position.y - 1, TVObject.transform.position.z);
        //butPos = TVButton.transform.position;
        //TVButton.transform.position = new Vector3(TVButton.transform.position.x, TVButton.transform.position.y - 0.1f, TVButton.transform.position.z);
        yield return new WaitForSeconds(5f);
        StartCoroutine(TextFade(welcomeText, 1));
	}

    public void Begin()
    {
        if (currentState == GameState.Start)
        {
            currentState = GameState.Play;
            StartCoroutine(TVLerpIn());
            ButtonOut();
            StartCoroutine(TextFade(buttonText, 2));
            StartCoroutine(clockScript.HourMoving("default"));
        }
    }

    IEnumerator GameTimer()
    {
        while (currentState == GameState.Play)
        {
            gameTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
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

    IEnumerator TVLerpIn()
    {
        float normalizedTime = 0;
        Vector3 currentPos = TVObject.transform.position;
        Vector3 targetPos = tvPos;
        while (normalizedTime < 1)
        {
            TVObject.transform.position = Vector3.Slerp(currentPos, targetPos, normalizedTime);
            normalizedTime += Time.deltaTime * 1.5f;
            yield return null;
        }

        //currentPos = TVButton.transform.position;
        //targetPos = butPos;
        //TVButton.SetActive(true);
        //normalizedTime = 0;
        //while (normalizedTime < 1)
        //{
        //    TVButton.transform.position = Vector3.Slerp(currentPos, targetPos, normalizedTime);
        //    normalizedTime += Time.deltaTime * 1.5f;
        //    yield return null;
        //}

        StartCoroutine(dTrigger.TutorialTextScrollThrough());
    }

    //IEnumerator TutorialTextScrollThrough()
    //{
    //    dTrigger.TriggerDialogue();

    //    for (int i = 0; i < dTrigger.dialogue.sentences.Length - 1; i++)
    //    {
    //        print(i);
    //        switch (i) //condition for text to proceed depending on which text is showing
    //        {
    //            case 2:
    //                yield return new WaitForSeconds(1);
    //                yield return new WaitUntil(() => clicked == true);
    //                break;
    //            case 3:
    //                yield return new WaitForSeconds(5);
    //                break;
    //            default:
    //                yield return new WaitForSeconds(5);
    //                break;
    //        }

    //        dTrigger.NextSentence();
    //    }
    //}

    void ButtonOut()
    {
        ButtonObject.SetActive(false);
    }

    //public void ButtonPressed(bool value)
    //{
    //    clicked = value;
    //}
}
