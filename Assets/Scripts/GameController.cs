using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {

    public enum GameState { Start, Play, End }
    public GameState currentState;

    //public GameObject TVObject;
    //private Vector3 tvPos;

    //public GameObject ButtonObject;

    private TutorialTrigger dTrigger;

    public TextMeshPro welcomeText;
    public TextMeshPro grabText;
    public TextMeshPro doorText;
    public TextMeshPro buttonText;
    public TextMeshPro walkText;

    private ClockController clockScript;

    void Awake()
    {
        clockScript = FindObjectOfType<ClockController>();
    }
    
    IEnumerator Start () {
        currentState = GameState.Start;
        welcomeText.enabled = true;
        grabText.enabled = true;
        doorText.enabled = true;
        buttonText.enabled = true;
        walkText.enabled = true;
        //dTrigger = GetComponent<TutorialTrigger>();
        //tvPos = TVObject.transform.position;
        //TVObject.transform.position = new Vector3(TVObject.transform.position.x, TVObject.transform.position.y - 1, TVObject.transform.position.z);
        yield return new WaitForSeconds(5f);
        StartCoroutine(TextFade(welcomeText, 1));
	}

    public void Begin()
    {
        if (currentState == GameState.Start)
        {
            currentState = GameState.Play;
            //StartCoroutine(TVLerpIn());
            //ButtonOut();
            StartCoroutine(TextFade(buttonText, 2));
            StartCoroutine(clockScript.HourMoving("default"));
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

    //IEnumerator TVLerpIn()
    //{
    //    float normalizedTime = 0;
    //    Vector3 currentPos = TVObject.transform.position;
    //    Vector3 targetPos = tvPos;
    //    while (normalizedTime < 1)
    //    {
    //        TVObject.transform.position = Vector3.Slerp(currentPos, targetPos, normalizedTime);
    //        normalizedTime += Time.deltaTime * 1.5f;
    //        yield return null;
    //    }

    //    StartCoroutine(dTrigger.TutorialTextScrollThrough());
    //}

    //void ButtonOut()
    //{
    //    ButtonObject.SetActive(false);
    //}

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
