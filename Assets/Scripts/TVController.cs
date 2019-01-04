using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVController : MonoBehaviour {

    private TutorialTrigger dTrigger;
    private Vector3 tvPos;

    public Button mainButton;
    public GameObject taskButtonObj;
    private Button taskButton;
    public GameObject recipeButtonObj;
    private Button recipeButton;

    public GameObject ButtonObject;

    // Use this for initialization
    void Start ()
    {
        dTrigger = GetComponent<TutorialTrigger>();
        taskButton = taskButtonObj.GetComponent<Button>();
        recipeButton = recipeButtonObj.GetComponent<Button>();
        tvPos = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonPressed()
    {
        StartCoroutine(TVLerpIn());
        ButtonOut();
    }

    IEnumerator TVLerpIn()
    {
        float normalizedTime = 0;
        Vector3 currentPos = transform.position;
        Vector3 targetPos = tvPos;
        while (normalizedTime < 1)
        {
            transform.position = Vector3.Slerp(currentPos, targetPos, normalizedTime);
            normalizedTime += Time.deltaTime * 1.5f;
            yield return null;
        }

        StartCoroutine(dTrigger.TutorialTextScrollThrough());
    }

    public void TVMainMenu()
    {
        mainButton.interactable = false;
        SwitchButtons(taskButtonObj, taskButton, true);
        SwitchButtons(recipeButtonObj, recipeButton, true);
    }

    void SwitchButtons(GameObject obj, Button button, bool state)
    {
        obj.SetActive(state);
        button.interactable = state;
    }

    public void EnterMode(int i)
    {
        SwitchButtons(taskButtonObj, taskButton, false);
        SwitchButtons(recipeButtonObj, recipeButton, false);
        switch (i)
        {
            case 1:
                break;
            case 2:
                break;
        }
    }

    void ButtonOut()
    {
        ButtonObject.SetActive(false);
    }
}
