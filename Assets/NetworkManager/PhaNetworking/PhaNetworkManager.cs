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

	public GameObject AgentPrefab; Health AgentHealth; Rigidbody AgentRigidBody; NetworkedBehaviour AgentPrediction;
	public GameObject RemoteAgentPrefab;
	public GameObject HackerPrefab; 
	public GameObject RemoteHackerPrefab;
	PhantomManager phantomManager;

	Vector3 PreviousPlayerPosition;

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
			if (characterSelection == 0 && PreviousPlayerPosition != AgentPrefab.transform.position)
			{//Sending
				SendPlayerUpdate(AgentPrefab.transform.position, AgentRigidBody.velocity, AgentPrefab.transform.rotation, PhaNetworkingAPI.targetIP);
				PreviousPlayerPosition = AgentPrefab.transform.position;
			}
			
			MessageType receivedType;
			//So you know, this is a terrible set up, but it'll be functional.
			for (int i = 0; i < 15; i++)
			{//Receiving
				receivedType = (MessageType)ReceiveInGameMessage();
//				Debug.Log("receivedType: " + receivedType);
				switch	(receivedType)
				{
					case MessageType.PlayerUpdate:
					AgentPrediction.ReceiveBuffer(ref receiveBuffer);
					break;

					case MessageType.EnemyUpdate:
					phantomManager.ParsePhantomUpdate(int.Parse(receiveBuffer[2].ToString()), receiveBuffer);
					break;

					case MessageType.HealthUpdate:
					AgentHealth.takeDamage(ParseHealthUpdate(receiveBuffer));
					break;

					case MessageType.ConsoleMessage:
					HackerPrefab.GetComponent<RemoteTextEnter>().ReceiveCode(receiveBuffer);
					break;

					case MessageType.DoorUpdate:
					DoorManager.Singleton.parseDoorUpdate(ref receiveBuffer);
					break;

					default://This may be the first time I've ever had a reachable default statement...
					return; //No more messages, so let's have an early exit.
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
				AgentRigidBody = AgentPrediction.GetComponent<Rigidbody>();
				PreviousPlayerPosition = new Vector3(AgentPrefab.transform.position.x, AgentPrefab.transform.position.y, AgentPrefab.transform.position.z);

				HackerPrefab = GameObject.Instantiate(RemoteHackerPrefab);				
			}
			else if (characterSelection == 1)
			{
				AgentPrefab = GameObject.Instantiate(RemoteAgentPrefab);
				AgentHealth = AgentPrefab.GetComponent<Health>();
				AgentPrefab.transform.position = new Vector3(20.30901f, -0.6f, 13.479f);
				AgentRigidBody = AgentPrefab.GetComponent<Rigidbody>();
				AgentPrediction = AgentPrefab.GetComponent<NetworkedBehaviour>();
				HackerPrefab = GameObject.Instantiate(HackerPrefab); //Local Player is Hacker. The order of instantiation here is important!
			}

			phantomManager = FindObjectOfType(typeof(PhantomManager)) as PhantomManager;
			if (phantomManager == null)
			{
				Debug.LogError("phantomManager not found. Fuck.");
			}
		}

	}
}
