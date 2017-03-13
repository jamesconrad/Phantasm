using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaNetworkManager : PhaNetworkingMessager {

	public string OnlineSceneName = ""; 
	private static PhaNetworkManager singleton;
	public static PhaNetworkManager Singleton 
	{ 
		get 
		{ 
			return singleton; 
		} 
	}
	public static bool Ishost = false;

	//0 for agent, 1 for hacker.
	public static int characterSelection = 0;

	public GameObject AgentPrefab;
	public GameObject HackerPrefab;

	/// This function is called when the object becomes enabled and active.
	void OnEnable()
	{
		singleton = this;
		PhaNetworkingAPI.mainSocket = PhaNetworkingAPI.InitializeNetworking();
		PhaNetworkManager.singleton.SendConnectionMessage(new StringBuilder("0.0.0.1"));
		Debug.Log("Networking initialized");

		SceneManager.activeSceneChanged += SpawnPlayer;
	}

	/// This function is called when the MonoBehaviour will be destroyed.
	void OnDestroy()
	{
		PhaNetworkingAPI.ShutDownNetwork(PhaNetworkingAPI.mainSocket);
	}

	public static IPAddress GetLocalHost()
	{
		//Get local IP address. Hope it doesn't change. It could. I should change this to whenever it specically tries to create a game.
		IPAddress[] ipv4Addresses = Dns.GetHostAddresses(Dns.GetHostName());
		for (int i = 0; i < ipv4Addresses.Length; i++)
		{
			if (ipv4Addresses.GetValue(i).ToString() != "127.0.0.1" && (ipv4Addresses.GetValue(i) as IPAddress).AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
			{
				PhaNetworkingAPI.hostAddress = ipv4Addresses.GetValue(i) as IPAddress;
				break;
			}
		}	
		return PhaNetworkingAPI.hostAddress;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void SpawnPlayer(Scene _scene1, Scene _scene2)
	{
		if (characterSelection == 0)
		{
			GameObject.Instantiate(AgentPrefab);
		}
		else if (characterSelection == 1)
		{
			GameObject.Instantiate(HackerPrefab);
		}
	}
}
