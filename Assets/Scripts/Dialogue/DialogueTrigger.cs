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
                    yield return new WaitForSeconds(3f);
                    break;
                case 1:
                    dScript.UpdateNameText("Tutorial");
                    StartCoroutine(Timer(5));
                    yield return new WaitUntil(() => nextTriggered == true);
                    break;
                case 5:
                    yield return new WaitForSeconds(1.5f);
                    nextTriggered = false;
                    yield return new WaitUntil(() => nextTriggered == true);
                    break;
                case 7: //unskippable
                    dScript.UpdateNameText("");
                    yield return new WaitForSeconds(3f);
                    break;
                default:
                    StartCoroutine(Timer(5));
                    yield return new WaitUntil(() => nextTriggered == true);
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

        buttonRoutine = StartCoroutine(ButtonReleased());
    }

    IEnumerator ButtonReleased()
    {
        yield return null;
        nextTriggered = false;
    }

    IEnumerator Timer(float seconds)
    {
        nextTriggered = false;
        float time = 0;
        while (!nextTriggered)
        {
            if (time >= seconds)
            {
                nextTriggered = true;
                break;
            }

            time += Time.deltaTime;
            yield return null;
        }
        print(time);
        buttonRoutine = StartCoroutine(ButtonReleased());
    }
}
