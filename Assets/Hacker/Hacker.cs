using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Hacker : MonoBehaviour {

    public GameObject PauseUI;

	// Use this for initialization
	void Start () {
        PauseUI = Instantiate(PauseUI) as GameObject;
        
 
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUI.SetActive(!PauseUI.activeSelf);
            GetComponentInChildren<HackerInteractionWindowSetup>().WindowIsInteractive = !PauseUI.activeSelf;
        }
	}
}
