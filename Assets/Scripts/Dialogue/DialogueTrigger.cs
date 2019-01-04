using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;
    private GameObject _dialogueManager;
    private DialogueManager dScript;
    bool nextTriggered;
    Coroutine buttonRoutine;
    Coroutine timerRoutine;

    void Start()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>().gameObject;
        dScript = _dialogueManager.GetComponent<DialogueManager>();
    }

    void TriggerDialogue()
    {
        dScript.StartDialogue(dialogue);
    }

    public IEnumerator TutorialTextScrollThrough()
    {
        TriggerDialogue();

        for (int i = 0; i < dialogue.sentences.Length - 1; i++)
        {
            switch (i) //condition for text to proceed depending on which text is showing
            {
                case 0: //unskippable
                    dScript.UpdateNameText("");
                    yield return new WaitForSeconds(3);
                    break;
                case 1:
                    dScript.UpdateNameText("Tutorial");
                    yield return new WaitForSeconds(5);
                    break;
                case 4:
                    yield return new WaitForSeconds(1.5f);
                    nextTriggered = false;
                    yield return new WaitUntil(() => nextTriggered == true);
                    break;
                case 6: //unskippable
                    dScript.UpdateNameText("");
                    yield return new WaitForSeconds(3);
                    break;
                default:
                    yield return new WaitForSeconds(5);
                    break;
            }

            dScript.DisplayNextSentence(dialogue);
        }
    }

    public void NextDialogue()
    {
        nextTriggered = true;
        if (buttonRoutine != null)
        {
            StopCoroutine(buttonRoutine);
        }

        //if (timerRoutine != null)
        //{
        //    StopCoroutine(timerRoutine);
        //}

        buttonRoutine = StartCoroutine(ButtonReleased());
    }

    IEnumerator ButtonReleased()
    {
        yield return new WaitForEndOfFrame();
        nextTriggered = false;
        print(nextTriggered);
    }

    IEnumerator Timer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
