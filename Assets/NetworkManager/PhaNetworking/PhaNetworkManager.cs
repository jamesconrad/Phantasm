using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhaNetworkManager : PhaNetworkingMessager {

	public bool SkipConnectionForDebug = false;

	public enum MainMenuState
	{
		Menu = 0,
		HostWaiting = 1,
		ClientWaiting = 2,
		CharacterSelect = 3,
		InGame = 4
	} 
	MainMenuState mainMenuState = MainMenuState.Menu;
	public void SetMenuState(MainMenuState state)
	{
		mainMenuState = state;
		switch (mainMenuState)
		{
			case MainMenuState.Menu:
				UnloadLobby();
				break;
		
			case MainMenuState.HostWaiting:
				break;

			case MainMenuState.ClientWaiting:
				LoadLobby();
				waitingScreen.GetComponentInChildren<Text>().text = "Choose your character";
				LoadButtons();
				break;
		
			case MainMenuState.CharacterSelect:
				waitingScreen.SetActive(true);
				waitingScreen.GetComponentInChildren<Text>().text = "Choose your character";
				LoadButtons();
				break;
		
			case MainMenuState.InGame:
				//switch scenes.
				break;
		
			default:
				break;
		}
	}

	public string offlineSceneName = "Menu";
	public string onlineSceneName;

	public GameObject agentObj;
	public GameObject HackerObj;
	GameObject playerPrefab;
	public GameObject otherAgentPrefab;
	public GameObject phantomPrefab;

	public GameObject waitingScreen;
	const int waitBufSize = 256;
	private StringBuilder waitingbuffer = new StringBuilder(256);

	public GameObject selectAgentUI; Button selectAgentButton;
	public GameObject selectHackerUI; Button selectHackerButton;
	public int localSelection = 0; 
	public void SetLocalSelection(int selection) 
	{
		 localSelection = selection; 
		 Debug.Log(PhaNetworkingAPI.targetIP);
		 Debug.Log("Sent characterlock: " + SendCharacterLockMessage(localSelection, PhaNetworkingAPI.targetIP));
	}
	public void GoToClientWait()
	{
		SetMenuState(MainMenuState.ClientWaiting);
	}


	public System.IntPtr secondarysocket;
	// Use this for initialization
	void Start () 
	{
		PhaNetworkingAPI.mainSocket = PhaNetworkingAPI.InitializeNetworking();
		SendConnectionMessage(new StringBuilder("0.0.0.1"));
		
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
		
		ipInput = GetComponentInChildren<InputField>();
		selectAgentButton = selectAgentUI.GetComponentInChildren<Button>();
		selectHackerButton = selectHackerUI.GetComponentInChildren<Button>();

		SceneManager.activeSceneChanged += SpawnPlayers;
	}

	// Update is called once per frame
	void Update () 
	{
		switch (mainMenuState)
		{
			case MainMenuState.Menu:
			break;

			case MainMenuState.HostWaiting:
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SetMenuState(MainMenuState.Menu);
			}
			break;

			case MainMenuState.ClientWaiting:
			break;

			case MainMenuState.CharacterSelect:
			break;
			
			case MainMenuState.InGame:
				SceneManager.LoadScene(onlineSceneName);	
				SceneManager.UnloadSceneAsync(offlineSceneName);		
			break;
			
			default:
			break;
		}
	}

	void FixedUpdate()
	{
		Debug.Log("mainMenu state: " + mainMenuState);
		switch (mainMenuState)
		{
			case MainMenuState.Menu:
			
			break;

			case MainMenuState.HostWaiting:
				//Do some logic to change into CharacterSelect. Recv, check the message, check for confirmation and if yes, go to CharacterSelect.
				//Debug.Log(ReceiveConnectionMessage());
				int Result = 0;
				Result = ReceiveConnectionMessage();
				Debug.Log("Result of the receive: " + Result);
				if (Result == 1)
				{
					PhaNetworkingAPI.targetIP = new StringBuilder(recvBufferSize);
					PhaNetworkingAPI.GetRemoteAddress(PhaNetworkingAPI.mainSocket, PhaNetworkingAPI.targetIP, recvBufferSize);
					Debug.Log("rmeote address: " + PhaNetworkingAPI.targetIP);
					SendStartMessageTo();
					SetMenuState(MainMenuState.CharacterSelect);
				}
			break;
			
			case MainMenuState.ClientWaiting:
				//Hear back from the other player
				if (ReceiveConnectionMessage() == 1)
				{
					SetMenuState(MainMenuState.CharacterSelect);
				}
			break;

			case MainMenuState.CharacterSelect:
				if (SkipConnectionForDebug)
				{
					if (!selectAgentButton.interactable)
					{
						playerPrefab = agentObj;
						SetMenuState(MainMenuState.InGame);
					}
					else if (!selectHackerButton.interactable)
					{
						playerPrefab = HackerObj;
						SetMenuState(MainMenuState.InGame);
					}
				} 
				else
				{
					int ReceiveResult = ReceiveCharacterLockMessage();
					Debug.Log("CharacterLock : " + ReceiveResult);
					if (ReceiveResult == 1)
					{
						selectAgentButton.interactable = false;
					}
					else if (ReceiveResult == 2)
					{
						selectHackerButton.interactable = false;
					}
					if (!selectAgentButton.interactable && !selectHackerButton.interactable)
					{
						if (localSelection == 1)
						{
							playerPrefab = agentObj;
							SetMenuState(MainMenuState.InGame);
						}
						else
						{
							playerPrefab = HackerObj;
							SetMenuState(MainMenuState.InGame);
						}
						SendLoadLevelMessage(new StringBuilder(ipInput.text));
					}
				}
			break;
			
			case MainMenuState.InGame:
				DelegateInGameUpdates();
			break;
			
			default:
			break;
		}
	}

	float posX, posY, posZ, oriW, oriX, oriY, oriZ;
	private void DelegateInGameUpdates()
	{
		MessageType result = (MessageType) ReceiveInGameMessage();
		if (result >= 0)
		{
			string[] message;
			switch (result)
			{
				case MessageType.PlayerUpdate:
					message = receiveBuffer.ToString().Split(' ');
					posX = float.Parse(message[1]);
					posY = float.Parse(message[2]);
					posZ = float.Parse(message[3]);

					otherAgentPrefab.transform.position = new Vector3(posX, posY, posZ);

					oriW = float.Parse(message[4]);
					oriX = float.Parse(message[5]);
					oriY = float.Parse(message[6]);
					oriZ = float.Parse(message[7]);

					otherAgentPrefab.transform.rotation = new Quaternion(oriX, oriY, oriZ, oriW);
					break;

				case MessageType.EnemyUpdate:
					message = receiveBuffer.ToString().Split(' ');
					message = receiveBuffer.ToString().Split(' ');
					posX = float.Parse(message[1]);
					posY = float.Parse(message[2]);
					posZ = float.Parse(message[3]);

					phantomPrefab.transform.position = new Vector3(posX, posY, posZ);

					oriW = float.Parse(message[4]);
					oriX = float.Parse(message[5]);
					oriY = float.Parse(message[6]);
					oriZ = float.Parse(message[7]);

					phantomPrefab.transform.rotation = new Quaternion(oriX, oriY, oriZ, oriW);
					break;

				case MessageType.HealthUpdate:
					//message = receiveBuffer.ToString().Split(' ');
					break;

				default:
					break;
			}
		}
	}

	public InputField ipInput;
	public bool VerifyIP()
	{
		if (mainMenuState != MainMenuState.Menu)
		{
			return true;
		}
		ipInput.text.Trim();
		string targetIP = ipInput.text;
		string[] ipSegments = targetIP.Split('.');
		if (ipSegments.Length != 4) //Check for ip formatting
		{
			return false;
		}
		int result;
		for (int i = 0; i < 4; i++)
		{
			if (int.TryParse(ipSegments[i], out result) == false) // Check for valid integers in the ip address.
			{
				return false;
			}
		}
		if (PhaNetworkingAPI.targetIP.ToString() != targetIP)
		{		
			PhaNetworkingAPI.targetIP = new StringBuilder(targetIP);
		}
		return true;
	}
	//Send message to IPAddress
	public void SendStartMessageTo()
	{
		if (PhaNetworkingAPI.mainSocket.Equals(null))
			{
				//PhaNetworkingAPI.InitializeNetworking(PhaNetworkingAPI.mainSocket);
			}
		if (!VerifyIP())
		{
			return;
		}
		//Then send.
		Debug.Log("Send Connection result: " + SendConnectionMessage(PhaNetworkingAPI.targetIP));
	}

//Lobby loading|unloading
	public void LoadLobby()
	{
		if (!SkipConnectionForDebug)
		{
			waitingScreen.SetActive(true);
			waitingScreen.GetComponentInChildren<Text>().text = "Waiting for other player...\n\nYour ip address: " + PhaNetworkingAPI.hostAddress.ToString();
			SetMenuState(MainMenuState.HostWaiting);
		}
		else
		{
			waitingScreen.SetActive(true);
			waitingScreen.GetComponentInChildren<Text>().text = "Choose your character...";
			SetMenuState(MainMenuState.CharacterSelect);
		}

	}
	public void LoadButtons()
	{
		selectAgentUI.SetActive(true);
		selectHackerUI.SetActive(true);
		mainMenuState = MainMenuState.CharacterSelect;
	}
	public void UnloadLobby()
	{
		waitingScreen.SetActive(false);
		selectAgentUI.SetActive(false);
		selectHackerUI.SetActive(false);
	}

	void SpawnPlayers(Scene _scene1, Scene _scene2)
    {
        if (_scene2.name != "Menu")
        {
			Vector3 agentPos = Vector3.zero;
			if (playerPrefab == agentObj)
			{
				agentPos = FindObjectOfType<PlayerStartLocation>().transform.position;
			}
            playerPrefab = Instantiate(playerPrefab, agentPos, Quaternion.identity);
        }
		phantomPrefab = Instantiate(phantomPrefab);
    }  

	/// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy()
	{
		PhaNetworkingAPI.ShutDownNetwork(PhaNetworkingAPI.mainSocket);
	}

	/// <summary>
	/// This function is called when the behaviour becomes disabled or inactive.
	/// </summary>
	void OnDisable()
	{
		PhaNetworkingAPI.ShutDownNetwork(PhaNetworkingAPI.mainSocket);
	}

	/// <summary>
	/// Callback sent to all game objects before the application is quit.
	/// </summary>
	void OnApplicationQuit()
	{
		PhaNetworkingAPI.ShutDownNetwork(PhaNetworkingAPI.mainSocket);
		PhaNetworkingAPI.mainSocket = (System.IntPtr) 0;
		Debug.Log(PhaNetworkingAPI.mainSocket + " from close");
	}

}
