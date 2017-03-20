using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    [System.Serializable]
    public struct PhantomSettings
    {
        public GameObject phantomGameObject;
        public int minimumNumberOfEnemies;
        public int maximumNumberOfEnemies;
    }
    public PhantomSettings phantomSettings;

    [System.Serializable]
    public struct GameCreationSettings
    {
        public UnityEngine.UI.Dropdown dropDownMatches;
        public UnityEngine.UI.Button joinMatchButton;
        
        [TooltipAttribute("This is automatically set to the 'onlinescene' variable. Set that and this will act correctly.")]
        public string sceneToSwitchTo;
    }
    public bool OverridePlayerCountForDebugging = false;

    public GameCreationSettings gameCreationSettings;

	float agentDelay = 1.0f;
	float hackerDelay = 3.0f;

    //1 to be agent, 0 to be hacker. For use in getting and selecting matches. This is not a system I shouldn't actually use, as this method is for use with version control, but here we are.
    private static int currentSelectionOfCharacter = 0;

    // Use this for initialization
    void Start()
    {
		UnityEngine.VR.VRSettings.enabled = false;
        gameCreationSettings.sceneToSwitchTo = onlineScene;
        //TODO: find some way to set this to change whenever the other player connects.
        onlineScene = "";
        SceneManager.activeSceneChanged += SpawnPlayers;
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.F1))
		{
			UnityEngine.VR.VRSettings.enabled = !UnityEngine.VR.VRSettings.enabled;
		}
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);  
    } 

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        
        CustomNetworkManager.singleton.playerPrefab = CustomNetworkManager.singleton.spawnPrefabs[1 - CustomNetworkManager.currentSelectionOfCharacter];

        if (client.serverIp == client.connection.address) //if this is the host, handle the various switch times
        {
            Debug.Log(NetworkServer.connections.Count);
            if (NetworkServer.connections.Count == 2 || (NetworkServer.connections.Count == 1 && OverridePlayerCountForDebugging))
            {
                SwitchScenes();
            }
            if (NetworkServer.connections.Count == 1 && !OverridePlayerCountForDebugging)
            {
                Debug.Log("Reminder: Turn off 'OverridePlayerCountForDebugging' in the network manager to play with less than 2 players.");
            }
        }
        else // Just join.
        {
            SwitchScenes();
        }
        
        
        // MainMenu.ActivateMainMenu();
    }

    private PhantomSpawnLocation[] spawnLocations;

    // Called on the server whenever a Network.InitializeServer was invoked and has completed
    public void OnServerInitialized()
    {
        //int numOfEnemies = Random.Range(phantomSettings.minimumNumberOfEnemies, phantomSettings.maximumNumberOfEnemies);
        //spawnLocations = FindObjectsOfType<PhantomSpawnLocation>();
        //if (spawnLocations.Length == 0)
        //{
        //    Debug.Log("There are no spawn locations for the phantom.");
        //    NetworkServer.Spawn(PhantomFactory(Vector3.up * 2, Quaternion.identity) as GameObject);
        //}
        //else
        //{
        //    for (int i = 0; i < numOfEnemies; i++)
        //    {
        //        PhantomSpawnLocation tempPos = spawnLocations[Random.Range(0, spawnLocations.Length - 1)];
        //        NetworkServer.Spawn(PhantomFactory(tempPos.transform.position, Quaternion.identity) as GameObject);
        //        phantomSettings.phantomGameObject.GetComponent<Phantom>().previousSpawnLocation = tempPos;
        //    }
        //}
    }

    public void CreateAsAgent()
	{
		StartCoroutine(CreateMatchAsAgentDelay(agentDelay));
        
    }

    public void CreateAsHacker()
	{
		StartCoroutine(CreateMatchAsHackerDelay(hackerDelay));
	
    }

	IEnumerator CreateMatchAsAgentDelay(float time = 1.0f)
	{
		yield return new WaitForSeconds(time);


		playerPrefab = CustomNetworkManager.singleton.spawnPrefabs[0];
		currentSelectionOfCharacter = 1;
		matchMaker.CreateMatch("Default", 2, true, "", "", "", 0, 0, OnMatchCreate);        
	}

	IEnumerator CreateMatchAsHackerDelay(float time = 1.0f)
	{
		yield return new WaitForSeconds(time);

		playerPrefab = CustomNetworkManager.singleton.spawnPrefabs[1];
		currentSelectionOfCharacter = 0;
		matchMaker.CreateMatch("Default", 2, true, "", "", "", 0, 1, OnMatchCreate);
	}


	public void GetMatchesForAgents()
    {
        CustomNetworkManager.singleton.matchMaker.ListMatches(0, 10, "", true, 0, 1, OnMatchList);
        currentSelectionOfCharacter = 1;
        gameCreationSettings.joinMatchButton.onClick.AddListener(JoinMatchAsAgent);
    }

    public void GetMatchesForHackers()
    {
        CustomNetworkManager.singleton.matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);
        currentSelectionOfCharacter = 0;
        gameCreationSettings.joinMatchButton.onClick.AddListener(JoinMatchAsHacker);
    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        base.OnMatchList(success, extendedInfo, matchList);
        gameCreationSettings.dropDownMatches.ClearOptions();
        for (int i = 0; i < matchList.Count; i++)
        {
            gameCreationSettings.dropDownMatches.options.Add(new UnityEngine.UI.Dropdown.OptionData(matchList[i].name));
        }
        if (matchList.Count > 0)
        {
            gameCreationSettings.joinMatchButton.interactable = true;
        }
    }

    public void JoinMatchAsAgent()
    {
        if (CustomNetworkManager.singleton.matches != null)
        {
			StartCoroutine(JoinMatchAsAgentDelay(agentDelay));

			//MatchInfoSnapshot MatchInfo = CustomNetworkManager.singleton.matches[gameCreationSettings.dropDownMatches.value];
            //CustomNetworkManager.singleton.matchMaker.JoinMatch(MatchInfo.networkId, "", "", "", 0, currentSelectionOfCharacter, CustomNetworkManager.singleton.OnMatchJoined);
            
        }
    }

    public void JoinMatchAsHacker()
    {
        if (CustomNetworkManager.singleton.matches != null)
        {
			StartCoroutine(JoinMatchAsHackerDelay(hackerDelay));

			//MatchInfoSnapshot MatchInfo = CustomNetworkManager.singleton.matches[gameCreationSettings.dropDownMatches.value];
            //CustomNetworkManager.singleton.matchMaker.JoinMatch(MatchInfo.networkId, "", "", "", 0, currentSelectionOfCharacter, CustomNetworkManager.singleton.OnMatchJoined);
            
        }
    }

	IEnumerator JoinMatchAsAgentDelay(float time = 1.0f)
	{
		yield return new WaitForSeconds(time);

		MatchInfoSnapshot MatchInfo = CustomNetworkManager.singleton.matches[gameCreationSettings.dropDownMatches.value];
		CustomNetworkManager.singleton.matchMaker.JoinMatch(MatchInfo.networkId, "", "", "", 0, currentSelectionOfCharacter, CustomNetworkManager.singleton.OnMatchJoined);

	}

	IEnumerator JoinMatchAsHackerDelay(float time = 2.0f)
	{
		yield return new WaitForSeconds(time);

		MatchInfoSnapshot MatchInfo = CustomNetworkManager.singleton.matches[gameCreationSettings.dropDownMatches.value];
		CustomNetworkManager.singleton.matchMaker.JoinMatch(MatchInfo.networkId, "", "", "", 0, currentSelectionOfCharacter, CustomNetworkManager.singleton.OnMatchJoined);

	}

	public override void OnStopServer()
    {
        //MainMenu.DeactivateMainMenu();
        base.OnStopServer();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        client.Shutdown();
        base.OnClientDisconnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        int intMessage = extraMessageReader.ReadMessage<IntegerMessage>().value;
        if (intMessage == 0)
        {
            playerPrefab = spawnPrefabs[0];
        }
        else
        {
            playerPrefab = spawnPrefabs[1];
        }
        base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
    }

    public void endGame()
    {
        Shutdown();
        Cursor.lockState = CursorLockMode.None;
    }

    private void SwitchScenes()
    {
        onlineScene = gameCreationSettings.sceneToSwitchTo;
        SceneManager.LoadScene(onlineScene);
    }

    public GameObject PhantomFactory(Vector3 _position, Quaternion _rotation)
    {
        GameObject newPhantom = Instantiate(phantomSettings.phantomGameObject, _position, _rotation) as GameObject;
        return newPhantom;
    }

    void SpawnPlayers(Scene _scene1, Scene _scene2)
    {
        if (_scene2.name != "Menu")
        {
            ClientScene.AddPlayer(ClientScene.readyConnection, 0, new IntegerMessage(1 - CustomNetworkManager.currentSelectionOfCharacter));
        }
    }   
}