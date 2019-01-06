using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TVController : MonoBehaviour {

    private List<FoodGoal> goalItemsName = new List<FoodGoal>();
    private bool listGenerated;

    private TutorialTrigger dTrigger;
    
    private Vector3 tvPos;

    [SerializeField] private GameObject listUI;
    [SerializeField] private GameObject listTemplate;
    [SerializeField] private GameObject recipeUI;

    private DialogueManager dScript;

    private GameController gameScript;

    public Button mainButton;
    public Button backButton;
    public GameObject taskButtonObj;
    public GameObject recipeButtonObj;

    public GameObject ButtonObject; 

    // Use this for initialization
    void Start ()
    {
        gameScript = FindObjectOfType<GameController>().gameObject.GetComponent<GameController>();
        dTrigger = GetComponent<TutorialTrigger>();
        dScript = GetComponent<DialogueManager>();
        tvPos = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
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

    public void LoadMenuMode()
    {
        mainButton.interactable = false;
        dScript.nameText.enabled = false;
        dScript.dialogueText.enabled = false;
        SwitchButtons(listUI, false);
        SwitchButtons(recipeUI, false);

        SwitchButtons(taskButtonObj, true);
        SwitchButtons(recipeButtonObj, true);
    }

    void SwitchButtons(GameObject obj, bool state)
    {
        obj.SetActive(state);
    }

    void ButtonOut()
    {
        ButtonObject.SetActive(false);
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
                LoadRecipeMenu();
                break;
            case 3:
                LoadMenuMode();
                break;
            default:
                LoadMenuMode();
                break;
        }
    }

    void LoadRecipeMenu()
    {
        dScript.nameText.enabled = true;
        dScript.UpdateNameText("Recipes");
        SwitchButtons(recipeUI, true);
        backButton.interactable = true;
    }

    void LoadTaskMenu()
    {
        dScript.nameText.enabled = true;
        dScript.UpdateNameText("Tasks");
        SwitchButtons(listUI, true);
        backButton.interactable = true;

        if (!listGenerated)
        {
            listGenerated = true;
            ConvertGameObjectToFoodGoal();
            listTemplate.GetComponent<TextMeshProUGUI>().text = "1" + "." + "<indent=10%>Serve a " + goalItemsName[0].prefix + " <b>" + goalItemsName[0].foodName + "</b>";
            if (goalItemsName.Count > 1)
            {
                for (int i = 1; i < goalItemsName.Count; i++)
                {
                    GameObject list = Instantiate(listTemplate) as GameObject;
                    list.SetActive(true);
                    list.GetComponent<TextMeshProUGUI>().text = i + 1 + "." + "<indent=10%>Serve a " + goalItemsName[i].prefix + " <b>" + goalItemsName[i].foodName + "</b>";
                    list.transform.SetParent(listTemplate.transform.parent, false);
                }
            }
        }
    }

    void LoadMenu()
    {

    }

    void ConvertGameObjectToFoodGoal()
    {
        string prefix;
        for (int i = 0; i < gameScript.goalItems.Count; i++)
        {
            switch (gameScript.goalItems[i].name)
            {
                case "Pizza":
                    prefix = "left-over";
                    break;
                case "Milk":
                    prefix = "carton of";
                    break;
                default:
                    prefix = "";
                    break;
            }

            FoodGoal food = new FoodGoal(gameScript.goalItems[i].name, prefix);
            goalItemsName.Add(food);
        }
    }
}
