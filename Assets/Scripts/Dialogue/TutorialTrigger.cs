using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    public Dialogue dialogue;
    private GameObject _dialogueManager;
    private DialogueManager dScript;
    private TVController tvScript;
    bool nextTriggered;
    Coroutine buttonRoutine;

    void Start()
    {
        tvScript = GetComponent<TVController>();
        _dialogueManager = FindObjectOfType<DialogueManager>().gameObject;
        dScript = _dialogueManager.GetComponent<DialogueManager>();
    }

    public IEnumerator TutorialTextScrollThrough()
    {
        dScript.StartDialogue(dialogue);

        for (int i = 0; i < dialogue.sentences.Length; i++)
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
                case 4:
                    yield return new WaitForSeconds(0.5f);
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

        dScript.EndDialogue();
        tvScript.LoadMenuMode();
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
        buttonRoutine = StartCoroutine(ButtonReleased());
    }
}
