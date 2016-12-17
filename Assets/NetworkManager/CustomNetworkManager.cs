using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using UnityEngine.Networking.Match;
using System.Collections.Generic;

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
    }
    public GameCreationSettings gameCreationSettings;

	float agentDelay = 1.0f;
	float hackerDelay = 3.0f;

    //1 to be agent, 0 to be hacker. For use in getting and selecting matches. This is not a system I shouldn't actually use, as this method is for use with version control, but here we are.
    private static int currentSelectionOfCharacter = 0;

    // Use this for initialization
    void Start()
    {
		UnityEngine.VR.VRSettings.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.F1))
		{
			UnityEngine.VR.VRSettings.enabled = !UnityEngine.VR.VRSettings.enabled;
		}
    }

    public override void OnStartClient(NetworkClient client)
    {
        //MainMenu.ActivateMainMenu();
        

        base.OnStartClient(client);
    } 

    public override void OnClientConnect(NetworkConnection conn)
    {
        CustomNetworkManager.singleton.playerPrefab = CustomNetworkManager.singleton.spawnPrefabs[1 - CustomNetworkManager.currentSelectionOfCharacter];
        ClientScene.AddPlayer(conn, 0, new IntegerMessage(1 - CustomNetworkManager.currentSelectionOfCharacter));
        //MainMenu.ActivateMainMenu();
        base.OnClientConnect(conn);
    }

    public override void OnStopClient()
    {
        //MainMenu.DeactivateMainMenu();
        base.OnStopClient();
    }

    private PhantomSpawnLocation[] spawnLocations;

    public override void OnStartServer()
    {
        //MainMenu.ActivateMainMenu();
        base.OnStartServer();
    }

    // Called on the server whenever a Network.InitializeServer was invoked and has completed
    public void OnServerInitialized()
    {
        int numOfEnemies = Random.Range(phantomSettings.minimumNumberOfEnemies, phantomSettings.maximumNumberOfEnemies);
        spawnLocations = FindObjectsOfType<PhantomSpawnLocation>();
        if (spawnLocations.Length == 0)
        {
            Debug.Log("There are no spawn locations for the phantom.");
            NetworkServer.Spawn(PhantomFactory(Vector3.up * 2, Quaternion.identity) as GameObject);
        }
        else
        {
            for (int i = 0; i < numOfEnemies; i++)
            {
                PhantomSpawnLocation tempPos = spawnLocations[Random.Range(0, spawnLocations.Length - 1)];
                NetworkServer.Spawn(PhantomFactory(tempPos.transform.position, Quaternion.identity) as GameObject);
                phantomSettings.phantomGameObject.GetComponent<Phantom>().previousSpawnLocation = tempPos;
            }
        }
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

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
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






    public GameObject PhantomFactory(Vector3 _position, Quaternion _rotation)
    {
        GameObject newPhantom = Instantiate(phantomSettings.phantomGameObject, _position, _rotation) as GameObject;
        return newPhantom;
    }
}
