using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
	public enum MainMenuState
	{
		Menu = 0,
		HostWaiting = 1,
		ClientWaiting = 2,
		CharacterSelect = 3,
		InGame = 4
	} 	

	public GameObject SelectionUI; Button selectAgentButton; Button selectHackerButton;
	public  Text UIText;
	public Button createGameButton;
	public Button JoinGameButton;
	public InputField ipInput;

	MainMenuState mainMenuState = MainMenuState.Menu;

	public InputField localPortInput;
	public InputField targetPortInput;

	public bool DebugForTesting = false;

	// Use this for initialization
	void Start () {
		Button[] buttons = SelectionUI.GetComponentsInChildren<Button>();
		selectAgentButton = buttons[0];
		selectHackerButton = buttons[1];
		UIText = SelectionUI.GetComponentInChildren<Text>();
		ipInput = GetComponentInChildren<InputField>();

		PhaNetworkManager.characterSelection = -1;
		enemyPlayerSelection = -1;
	}

	bool networkingInit = false;
	public void SetNetworking()
	{
		if (networkingInit == false)
		{
			int PortNumber = 0;
			if( int.TryParse(localPortInput.text, out PortNumber))
			{
				localPortInput.text = "Invalid port";
				return; //TODO: close the page if it's wrong, open the options page.
			}
			else
			{
				PhaNetworkingAPI.mainPort = PortNumber;
				PhaNetworkingAPI.mainSocket = PhaNetworkingAPI.InitializeNetworking(PhaNetworkingAPI.mainPort);
			}
			if (int.TryParse(targetPortInput.text, out PortNumber))
			{
				targetPortInput.text = "Invalid Port";
				return;
			}
			else
			{
				PhaNetworkingAPI.targetPort = PortNumber;
				PhaNetworkManager.Singleton.SendConnectionMessage(new StringBuilder("0.0.0.1"));
			}
			networkingInit = true;
		}
		else
		{
			PhaNetworkingAPI.ShutDownNetwork(PhaNetworkingAPI.mainSocket);
			networkingInit = false;
		}
	}
	
	//Handle various changes for each state
	//Try to manage all menu changes from here.
	public void SetMenuState(MainMenuState state)
	{ 
		mainMenuState = state;		
		switch (state)
		{
			case MainMenuState.Menu:
			SelectionUI.SetActive(false);
			selectAgentButton.interactable = false;
			selectHackerButton.interactable = false;
			break;

			case MainMenuState.HostWaiting:
			SelectionUI.SetActive(true);
			selectAgentButton.interactable = false;
			selectHackerButton.interactable = false;
			UIText.text = "Waiting for connection...\nYour ip: " 
			+ PhaNetworkManager
			.GetLocalHost()
			.ToString();

			PhaNetworkManager.Ishost = true;
			break;

			case MainMenuState.ClientWaiting:
			if (VerifyIP(ipInput.text))
			{//If the ip is a valid ip, set the api's target ip and send the connection message to it.
				PhaNetworkingAPI.targetIP = new StringBuilder(ipInput.text);
				PhaNetworkManager.Singleton.SendConnectionMessage(PhaNetworkingAPI.targetIP);
				PhaNetworkManager.Singleton.SendConnectionMessage(PhaNetworkingAPI.targetIP);
				PhaNetworkManager.Singleton.SendConnectionMessage(PhaNetworkingAPI.targetIP);

				//Settings for splash screen.
				SelectionUI.SetActive(true); 
				selectAgentButton.interactable = false;
				selectHackerButton.interactable = false;
				UIText.text = "Waiting for reply...\n";

				PhaNetworkManager.Ishost = false;
			}
			else
			{
				ipInput.text = "INVALID IP";
				SetMenuState(MainMenuState.Menu);
			}
			break;

			case MainMenuState.CharacterSelect:
			SelectionUI.SetActive(true);
			selectAgentButton.interactable = true;
			selectHackerButton.interactable = true;
			UIText.text = "Connection confirmed, Select your character.";
			break;
			
			case MainMenuState.InGame:
			//Load the scene if both players have decided on their choice.
			SceneManager.LoadScene(PhaNetworkManager.Singleton.OnlineSceneName);
			break;

			default:
			break;
		}
	}

	/// method for Create buttton
	public void CreateMethod()
	{
		SetMenuState(MainMenuState.HostWaiting);

		if (DebugForTesting)
		{
			PhaNetworkManager.characterSelection = 0;
			PhaNetworkManager.Ishost = true;
			SetMenuState(MainMenuState.InGame);
		}
	}

	/// method for Join buttton
	public void JoinMethod()
	{
		SetMenuState(MainMenuState.ClientWaiting);

		if (DebugForTesting)
		{
			PhaNetworkManager.characterSelection = 1;
			PhaNetworkManager.Ishost = true;
			SetMenuState(MainMenuState.InGame);
		}
	}

	/// method for Agent selection button
	public void SelectAgent()
	{
		selectAgentButton.interactable = false;
		PhaNetworkManager.characterSelection = 0;
		PhaNetworkManager.Singleton.SendCharacterLockMessage(PhaNetworkManager.characterSelection, PhaNetworkingAPI.targetIP);
		PhaNetworkManager.Singleton.SendCharacterLockMessage(PhaNetworkManager.characterSelection, PhaNetworkingAPI.targetIP);
		PhaNetworkManager.Singleton.SendCharacterLockMessage(PhaNetworkManager.characterSelection, PhaNetworkingAPI.targetIP);
	}

	/// method for Hacker selection button
	public void SelectHacker()
	{
		selectHackerButton.interactable = false;
		PhaNetworkManager.characterSelection = 1;
		PhaNetworkManager.Singleton.SendCharacterLockMessage(PhaNetworkManager.characterSelection, PhaNetworkingAPI.targetIP);
		PhaNetworkManager.Singleton.SendCharacterLockMessage(PhaNetworkManager.characterSelection, PhaNetworkingAPI.targetIP);
		PhaNetworkManager.Singleton.SendCharacterLockMessage(PhaNetworkManager.characterSelection, PhaNetworkingAPI.targetIP);
	}
	
	private int enemyPlayerSelection = -1;
	/// Update is called every frame, if the MonoBehaviour is interactable.
	void Update()
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
			if (PhaNetworkManager.Singleton.ReceiveConnectionMessage() > 0)
			{
				SetMenuState(MainMenuState.CharacterSelect);
				PhaNetworkingAPI.targetIP = new StringBuilder(PhaNetworkManager.recvBufferSize);
				PhaNetworkingAPI.GetRemoteAddress(PhaNetworkingAPI.mainSocket, PhaNetworkingAPI.targetIP, PhaNetworkManager.recvBufferSize);
				PhaNetworkManager.Singleton.SendConnectionMessage(PhaNetworkingAPI.targetIP);
				PhaNetworkManager.Singleton.SendConnectionMessage(PhaNetworkingAPI.targetIP);
				PhaNetworkManager.Singleton.SendConnectionMessage(PhaNetworkingAPI.targetIP);
			}
			break;

			case MainMenuState.ClientWaiting:
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SetMenuState(MainMenuState.Menu);
			}
			if (PhaNetworkManager.Singleton.ReceiveConnectionMessage() > 0)
			{
				SetMenuState(MainMenuState.CharacterSelect);
				PhaNetworkManager.Singleton.SendConnectionMessage(PhaNetworkingAPI.targetIP);
				PhaNetworkManager.Singleton.SendConnectionMessage(PhaNetworkingAPI.targetIP);
				PhaNetworkManager.Singleton.SendConnectionMessage(PhaNetworkingAPI.targetIP);
			}
			break;

			case MainMenuState.CharacterSelect:
			int i = PhaNetworkManager.Singleton.ReceiveCharacterLockMessage();
			if (i > -1)
			{
				enemyPlayerSelection = i;
				if (enemyPlayerSelection == 0)
				{//Select players properly, disable the other 
					selectAgentButton.interactable = false;
					selectAgentButton.targetGraphic.color = Color.black;
				}
				if (enemyPlayerSelection == 1)
				{
					selectHackerButton.interactable = false;
					selectHackerButton.targetGraphic.color = Color.black;
				}
			}

			if (Input.GetKeyDown(KeyCode.Escape))
			{//Return to main menu
				selectAgentButton.targetGraphic.color = Color.white;
				selectHackerButton.targetGraphic.color = Color.white;
				SetMenuState(MainMenuState.Menu);
				
			}
			
			if (PhaNetworkManager.characterSelection != -1 && enemyPlayerSelection != -1)
			{//Go to in game if both players have selected 
				SetMenuState(MainMenuState.InGame);
			}
			break;
			
			case MainMenuState.InGame:
			break;

			default:
			break;
		}
	}

	private bool VerifyIP(string givenIP)
	{
		string[] ipSegments = givenIP.Split('.');
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
		return true; //Otherwise, sure, it's an ip.
	}
}
