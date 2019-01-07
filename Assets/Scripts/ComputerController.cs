using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComputerController : MonoBehaviour {

    GameController gameScript;
    public TextMeshProUGUI levelText;

	// Use this for initialization
	void Start ()
    {
        gameScript = FindObjectOfType<GameController>().gameObject.GetComponent<GameController>();
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
}
