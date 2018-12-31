using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public enum GameState { Start, Play, End }
    public GameState currentState;

    // Use this for initialization
    void Start () {
        currentState = GameState.Start;
	}

    public IEnumerator Begin()
    {
        yield return new WaitForSeconds(3f);
        currentState = GameState.Play;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
