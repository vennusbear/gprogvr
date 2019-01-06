using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeTrigger : MonoBehaviour {

    public Dialogue dialogue;
    private GameObject _dialogueManager;
    private DialogueManager dScript;
    private TVController tvScript;
    int currentD = -1;
    Coroutine buttonRoutine;

    // Use this for initialization
    void Start ()
    {
        tvScript = GetComponent<TVController>();
        _dialogueManager = FindObjectOfType<DialogueManager>().gameObject;
        dScript = _dialogueManager.GetComponent<DialogueManager>();
    }

    public void RecipeScrollThrough()
    {
        currentD = 0;
        dScript.StartDialogue(dialogue);
        dScript.UpdateNameText(dialogue.title);

        //for (int i = 0; i < dialogue.sentences.Length; i++)
        //{
        //    print("test");
        //    yield return new WaitUntil(() => nextTriggered == true);
        //    dScript.DisplayNextSentence(dialogue);
        //}

        //dScript.EndDialogue();
    }

    public void NextRecipeDialogue()
    {
        if (currentD >= 0)
        {
            if (currentD >= dialogue.sentences.Length - 1)
            {
                tvScript.LoadMenuMode();
            }
            currentD += 1;
            dScript.DisplayNextSentence(dialogue);
        }
    }

    //public void NextDialogue()
    //{
    //    nextTriggered = true;

    //    if (buttonRoutine != null)
    //    {
    //        StopCoroutine(buttonRoutine);
    //    }

    //    buttonRoutine = StartCoroutine(ButtonReleased());
    //}

    //IEnumerator ButtonReleased()
    //{
    //    yield return null;
    //    nextTriggered = false;
    //}
}
