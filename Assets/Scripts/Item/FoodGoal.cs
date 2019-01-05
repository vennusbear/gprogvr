using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGoal: MonoBehaviour {

    public string prefix = "";
    public string foodName;

    public FoodGoal(string name, string pfx)
    {
        foodName = name;
        prefix = pfx;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
