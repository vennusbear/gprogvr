using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;
    private GameObject _dialogueManager;
    private DialogueManager dScript;
    bool d4;

    private void Start()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>().gameObject;
        dScript = _dialogueManager.GetComponent<DialogueManager>();
    }

    void TriggerDialogue()
    {
        dScript.StartDialogue(dialogue);
    }

    void Update()
    {

    }

    public IEnumerator TutorialTextScrollThrough()
    {
        TriggerDialogue();

        for (int i = 0; i < dialogue.sentences.Length - 1; i++)
        {
            switch (i) //condition for text to proceed depending on which text is showing
            {
                case 0:
                    dialogue.title = "Tutorial"; // title of the message, make sure to put before the actual message by 1 
                    yield return new WaitForSeconds(3);
                    break;
                case 1:
                    yield return new WaitForSeconds(5);
                    break;
                case 4:
                    yield return new WaitForSeconds(1);
                    //d4 = false;
                    yield return new WaitUntil(() => d4 == true);
                    break;
                case 5:
                    dialogue.title = "";
                    yield return new WaitForSeconds(3);
                    break;
                default:
                    yield return new WaitForSeconds(5);
                    break;
            }

            dScript.DisplayNextSentence(dialogue);
        }
    }

    public void DialogueFourNext(bool pressed)
    {
        d4 = pressed; 
    }
}
