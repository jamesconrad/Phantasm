using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class CreateGameButton : MonoBehaviour {
    public Dropdown drop;
    private NetworkManager manager;
	// Use this for initialization
	void Start () {
        manager = CustomNetworkManager.singleton;
        manager.StartMatchMaker();       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
