using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	public enum MainMenuState
	{
		Menu = 0,
		HostWaiting = 1,
		ClientWaiting = 2,
		CharacterSelect = 3,
		InGame = 4
	} 	

	public GameObject SelectionUI; Button selectAgentButton; Button selectHackerButton; Text UIText;
	public Button createGameButton;
	public Button JoinGameButton;
	public InputField ipInput;

	MainMenuState mainMenuState = MainMenuState.Menu;

	//Handle various changes for each state
	//Try to manage all menu changes from here.
	public void SetMenuState(MainMenuState state)
	{ 
		mainMenuState = state;		
		switch (state)
		{
			case MainMenuState.Menu:
			SelectionUI.SetActive(false);
			selectAgentButton.enabled = false;
			selectHackerButton.enabled = false;
			break;

			case MainMenuState.HostWaiting:
			SelectionUI.SetActive(true);
			selectAgentButton.enabled = false;
			selectHackerButton.enabled = false;
			UIText.text = "Waiting for connection...\nYour ip: " + PhaNetworkManager.GetLocalHost().ToString();
			break;

			case MainMenuState.ClientWaiting:
			if (VerifyIP(ipInput.text))
			{
				PhaNetworkingAPI.targetIP = new StringBuilder(ipInput.text);
				Debug.Log(PhaNetworkingAPI.targetIP);
				Debug.Log(PhaNetworkManager.Singleton);
				PhaNetworkManager.Singleton.SendConnectionMessage(PhaNetworkingAPI.targetIP);

				SelectionUI.SetActive(true);
				selectAgentButton.enabled = false;
				selectHackerButton.enabled = false;
				UIText.text = "Waiting for reply...\n";
			
			}
			else
			{
				ipInput.text = "INVALID IP";
				SetMenuState(MainMenuState.Menu);
			}
			break;

			case MainMenuState.CharacterSelect:
			SelectionUI.SetActive(true);
			selectAgentButton.enabled = true;
			selectHackerButton.enabled = true;
			UIText.text = "Connection confirmed, Select your character.";
			
			break;
			
			case MainMenuState.InGame:
			break;

			default:
			break;
		}
	}

	// Use this for initialization
	void Start () {
		Button[] buttons = SelectionUI.GetComponentsInChildren<Button>();
		selectAgentButton = buttons[0];
		selectHackerButton = buttons[1];
		UIText = SelectionUI.GetComponentInChildren<Text>();
		ipInput = GetComponentInChildren<InputField>();
	}
	
	public void CreateMethod()
	{
		SetMenuState(MainMenuState.HostWaiting);
	}

	public void JoinMethod()
	{
		SetMenuState(MainMenuState.ClientWaiting);
	}

	/// Update is called every frame, if the MonoBehaviour is enabled.
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
			}
			break;

			case MainMenuState.CharacterSelect:
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
