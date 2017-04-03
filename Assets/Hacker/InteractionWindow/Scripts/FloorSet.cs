using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorSet : MonoBehaviour {

    public HackerInteractionWindowSetup hiws;
    public Text t;

	// Use this for initialization
	void Start ()
    {
        t = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        t.text = hiws.viewFloor.ToString();
	}
}
