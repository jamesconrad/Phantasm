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

    //1 to be agent, 0 to be hacker. For use in getting and selecting matches. This is not a system I shouldn't actually use, as this method is for use with version control, but here we are.
    private int currentSelectionOfCharacter = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnStartClient(NetworkClient client)
    {
        //MainMenu.ActivateMainMenu();
        base.OnStartClient(client);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
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
            NetworkServer.Spawn(Instantiate(phantomSettings.phantomGameObject, Vector3.up * 2, Quaternion.identity) as GameObject);
        }
        else
        {
            for (int i = 0; i < numOfEnemies; i++)
            {
                PhantomSpawnLocation tempPos = spawnLocations[Random.Range(0, spawnLocations.Length - 1)];
                NetworkServer.Spawn(Instantiate(phantomSettings.phantomGameObject, tempPos.transform.position, Quaternion.identity) as GameObject);
                phantomSettings.phantomGameObject.GetComponent<Phantom>().previousSpawnLocation = tempPos;
            }
        }
    }

    public void CreateAsAgent()
    {
        autoCreatePlayer = true;
        playerPrefab = CustomNetworkManager.singleton.spawnPrefabs[0];
        matchMaker.CreateMatch("Default", 2, true, "", "", "", 0, 0, OnMatchCreate);
    }

    public void CreateAsHacker()
    {
        autoCreatePlayer = true;
        playerPrefab = CustomNetworkManager.singleton.spawnPrefabs[1];
        matchMaker.CreateMatch("Default", 2, true, "", "", "", 0, 1, OnMatchCreate);
    }


    public void GetMatchesForAgents()
    {
        CustomNetworkManager.singleton.matchMaker.ListMatches(0, 10, "", true, 0, 1, OnMatchList);
        currentSelectionOfCharacter = 1;
    }

    public void GetMatchesForHackers()
    {
        CustomNetworkManager.singleton.matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);
        currentSelectionOfCharacter = 0;
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

    public void JoinMatch()
    {
        if (CustomNetworkManager.singleton.matches != null)
        {
            CustomNetworkManager.singleton.playerPrefab = CustomNetworkManager.singleton.spawnPrefabs[1 - currentSelectionOfCharacter];
            MatchInfoSnapshot MatchInfo = CustomNetworkManager.singleton.matches[gameCreationSettings.dropDownMatches.value]; // Need to get this value somehow
            CustomNetworkManager.singleton.matchMaker.JoinMatch(MatchInfo.networkId, "", "", "", 0, currentSelectionOfCharacter, CustomNetworkManager.singleton.OnMatchJoined);
        }
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
        if (extraMessageReader != null)
        {
            playerPrefab = spawnPrefabs[extraMessageReader.ReadMessage<IntegerMessage>().value];
            if (playerPrefab == spawnPrefabs[0])
            {
                playerPrefab = spawnPrefabs[1];
            }
            else
            {
                playerPrefab = spawnPrefabs[0];
            }
            
        }
        base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
    }
}
