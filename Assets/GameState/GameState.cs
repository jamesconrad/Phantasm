using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;

public class GameState : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EndGame()
    {
        if (!isServer)
        {
            return;
        }
        CustomNetworkManager.singleton.StopHost();
        NetworkServer.Shutdown();
        Cursor.lockState = CursorLockMode.None;
    }

    public static void StaticEndGame()
    {
        CustomNetworkManager.singleton.StopHost();
        NetworkServer.Shutdown();
        Cursor.lockState = CursorLockMode.None;
    }
}
