using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreFetch : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Text t = GetComponent<Text>();
        string score = System.IO.File.ReadAllText("c:\\temp\\Phantasm.score");
        string textstring = "Your score from last round is: " + score;
        t.text = textstring;
        print(t.name);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
