using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerTextEntry : MonoBehaviour {
    
    public InputField infield;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)/* && infield.isFocused*/)
        {
            infield.Select();
            infield.ActivateInputField();
        }
	}
}
