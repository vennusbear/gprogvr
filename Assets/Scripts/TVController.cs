using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TVController : MonoBehaviour {

    private TutorialTrigger dTrigger;
    private Vector3 tvPos;

    [SerializeField] private GameObject list;
    [SerializeField] private GameObject listTemplate;

    private DialogueManager dScript;

    public GameObject gameObj;
    private GameController gameScript;

    public Button mainButton;
    public GameObject taskButtonObj;
    public GameObject recipeButtonObj;

    public GameObject ButtonObject; 

    // Use this for initialization
    void Start ()
    {
        gameScript = gameObj.GetComponent<GameController>();
        dTrigger = GetComponent<TutorialTrigger>();
        dScript = GetComponent<DialogueManager>();
        tvPos = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
    }

    void LoadTaskMenu()
    {
        dScript.nameText.enabled = true;
        dScript.UpdateNameText("Tasks");
        SwitchButtons(list, true);
        listTemplate.GetComponent<TextMeshProUGUI>().text = 1 + "." + "<indent=10%>Serve a "+ gameScript.goalItems[0].prefix + " <i>" + gameScript.goalItems[0].foodName + "</i>";

        if (gameScript.goalItems.Count > 1)
        {
            for (int i = 1; i < gameScript.goalItems.Count; i++)
            {
                GameObject list = Instantiate(listTemplate) as GameObject;
                list.SetActive(true);
                list.GetComponent<TextMeshProUGUI>().text = i + 1 + "." + "<indent=10%>Serve a " + gameScript.goalItems[i].prefix + " <i>" + gameScript.goalItems[i].foodName + "</i>";
                list.transform.SetParent(listTemplate.transform.parent, false);
            }
        }
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
        SwitchButtons(taskButtonObj, true);
        SwitchButtons(recipeButtonObj, true);
    }

    void SwitchButtons(GameObject obj, bool state)
    {
        obj.SetActive(state);
    }

    public void EnterMode(int i)
    {
        SwitchButtons(taskButtonObj, false);
        SwitchButtons(recipeButtonObj, false);
        switch (i)
        {
            case 1:
                LoadTaskMenu();
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
