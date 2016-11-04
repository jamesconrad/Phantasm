using UnityEngine;
using System.Collections;

public class GlobalParameters : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //Debug.Log("Update");
        if (Input.GetKeyDown(KeyCode.F12))
        {
            Application.CaptureScreenshot("./Screenshot.png", 16);
            Debug.Log("Screenshot captured");
        }
    }
}
