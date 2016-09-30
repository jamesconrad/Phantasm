using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkObjectSpawning : NetworkBehaviour {

	public GameObject phantomGameObject;
	// Use this for initialization
	void Start () {
	
	}


	public override void OnStartServer()
	{
        if (!isServer)
        {
            return;
        }
		base.OnStartServer();

		NetworkServer.Spawn((GameObject)Instantiate(phantomGameObject, Vector3.up * 2, Quaternion.identity));
	}
	// Update is called once per frame
	void Update () {
		
	}

}
