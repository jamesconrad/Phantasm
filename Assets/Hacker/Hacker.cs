using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Hacker : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
        {
            gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
