using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Hacker : NetworkBehaviour {

    public GameObject PauseUI;

	// Use this for initialization
	void Start () {
        PauseUI = Instantiate(PauseUI) as GameObject;
        
        if (!isLocalPlayer)
        {
            gameObject.SetActive(false);
        }
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
