using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour {

    public TextMeshPro nameText;
    public TextMeshPro dialogueText;
    private Queue<string> sentences;

    public bool next;

    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(dialogue);
    }

    public void DisplayNextSentence(Dialogue dialogue)
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        dialogueText.text = sentence;
    }

    public void UpdateNameText(string title)
    {
        nameText.text = title;
    }

    //IEnumerator TypeSentence(string sentence)
    //{
    //    dialogueText.text = "";
    //    foreach (char letter in sentence.ToCharArray())
    //    {
    //        dialogueText.text += letter;
    //        yield return null;
    //    }
    //}

    void EndDialogue()
    {
       
    }
}
