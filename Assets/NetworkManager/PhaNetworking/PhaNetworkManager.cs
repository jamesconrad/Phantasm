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

	public GameObject AgentPrefab; Health AgentHealth;
	public GameObject RemoteAgentPrefab;
	public GameObject HackerPrefab;
	public GameObject PhantomPrefab;
	public GameObject remotePhantomPrefab;

	private static bool NetworkInitialized = false;
	/// This function is called when the object becomes enabled and active.
	void OnEnable()
	{
		if (!NetworkInitialized)
		{
			singleton = this;
			PhaNetworkingAPI.mainSocket = PhaNetworkingAPI.InitializeNetworking();
			PhaNetworkManager.singleton.SendConnectionMessage(new StringBuilder("0.0.0.1"));
			Debug.Log("Networking initialized");

			SceneManager.activeSceneChanged += SpawnPlayer;
			NetworkInitialized = true;
			DontDestroyOnLoad(this);
		}
	}

	/// This function is called when the MonoBehaviour will be destroyed.
	void OnDestroy()
	{
		if (NetworkInitialized)
		{
			PhaNetworkingAPI.ShutDownNetwork(PhaNetworkingAPI.mainSocket);
			NetworkInitialized = false;
		}
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
		if (SceneManager.GetActiveScene().name != "Menu")
		{
			if (characterSelection == 0)
			{//Sending
				SendPlayerUpdate(AgentPrefab.transform.position, AgentPrefab.transform.rotation, PhaNetworkingAPI.targetIP);
			}
			if (Ishost)
			{
				SendEnemyUpdate(PhantomPrefab.transform.position, PhantomPrefab.transform.rotation, PhaNetworkingAPI.targetIP);
			} 
			//So you know, this is a terrible set up, but it'll be functional.
			for (int i = 0; i < /*numChecks*/ 3; i++)
			{//Receiving
				switch	((MessageType)ReceiveInGameMessage())
				{
					case MessageType.PlayerUpdate:
					ParseObjectUpdate(receiveBuffer, AgentPrefab.transform);
					break;

					case MessageType.EnemyUpdate:
					ParseObjectUpdate(receiveBuffer, PhantomPrefab.transform);
					break;

					case MessageType.HealthUpdate:
					AgentHealth.takeDamage(ParseHealthUpdate(receiveBuffer));
					break;

					default://This may be the first time I've ever had a reachable default statement...
					break;
				}
			}
		}
	}
	
	void SpawnPlayer(Scene _scene1, Scene _scene2)
	{
		if (_scene2.name != "Menu")
		{		
			if (characterSelection == 0)
			{
				AgentPrefab = GameObject.Instantiate(AgentPrefab); //Local player is agent.
				AgentHealth = AgentPrefab.GetComponent<Health>();
			}
			else if (characterSelection == 1)
			{
				HackerPrefab = GameObject.Instantiate(HackerPrefab); //Local Player is Hacker

				AgentPrefab = GameObject.Instantiate(RemoteAgentPrefab);
				AgentHealth = AgentPrefab.GetComponent<Health>();
				//Set to online version of agent.
			}

			if (Ishost)
			{
				PhantomPrefab = GameObject.Instantiate(PhantomPrefab);
			}
			else
			{
				PhantomPrefab = GameObject.Instantiate(remotePhantomPrefab);
			}
		}

	}
}
