using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComputerController : MonoBehaviour {

    GameController gameScript;
    public TextMeshProUGUI levelText;

    #region //Audio 
    public GameObject speaker;
    AudioSource audioScript;
    public AudioClip bgmAudio;
    #endregion

    // Use this for initialization
    void Start ()
    {
        gameScript = FindObjectOfType<GameController>().gameObject.GetComponent<GameController>();
        audioScript = speaker.GetComponent<AudioSource>();
    }

    public void ChangeLevel()
    {
        if (gameScript.level == 1)
        {
            gameScript.level = 2;
        }

        else if (gameScript.level == 2)
        {
            gameScript.level = 1;
        }

        levelText.text = gameScript.level.ToString();
    }

    public void LoadLevel()
    {
        gameScript.ChangeGameScene(0);
    }

    public void ToggleAudio()
    {
        if (audioScript.isPlaying)
        {
            audioScript.Pause();
        }

        else
        {
            audioScript.UnPause();
        }
    }
}
