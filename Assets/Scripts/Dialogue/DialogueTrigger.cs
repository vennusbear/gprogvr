using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;
    private GameObject _dialogueManager;
    private DialogueManager dScript;

    private void Start()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>().gameObject;
        dScript = _dialogueManager.GetComponent<DialogueManager>();
    }

    public void TriggerDialogue()
    {
        dScript.StartDialogue(dialogue);
    }

    void Update()
    {

    }

    public void NextSentence()
    {
        dScript.DisplayNextSentence();
    }

    public IEnumerator TutorialTextScrollThrough()
    {
        TriggerDialogue();

        for (int i = 0; i < dialogue.sentences.Length - 1; i++)
        {
            switch (i) //condition for text to proceed depending on which text is showing
            {
                case 2:
                    yield return new WaitForSeconds(1);
                    yield return new WaitUntil(() => dScript.next == true);
                    break;
                case 3:
                    yield return new WaitForSeconds(5);
                    break;
                default:
                    yield return new WaitForSeconds(5);
                    break;
            }

            NextSentence();
        }
    }
}
