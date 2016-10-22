using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

public class CustomNetworkManager : NetworkManager
{
    public GameObject phantomGameObject;

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
        MainMenu.ActivateMainMenu();
        base.OnClientConnect(conn);
    }

    public override void OnStopClient()
    {
        MainMenu.DeactivateMainMenu();
        base.OnStopClient();
    }

    private PhantomSpawnLocation[] spawnLocations;

    public override void OnStartServer()
    {
        MainMenu.ActivateMainMenu();
        base.OnStartServer();
    }

    // Called on the server whenever a Network.InitializeServer was invoked and has completed
    public void OnServerInitialized()
    {
        spawnLocations = FindObjectsOfType<PhantomSpawnLocation>();
        if (spawnLocations.Length == 0)
        {
            Debug.Log("There are no spawn locations for the phantom.");
            NetworkServer.Spawn(Instantiate(phantomGameObject, Vector3.up * 2, Quaternion.identity) as GameObject);
        }
        else
        {
            PhantomSpawnLocation tempPos = spawnLocations[Random.Range(0, spawnLocations.Length - 1)];
            NetworkServer.Spawn(Instantiate(phantomGameObject, tempPos.transform.position, Quaternion.identity) as GameObject);
            phantomGameObject.GetComponent<Phantom>().previousSpawnLocation = tempPos;
        }
    }

    public override void OnStopServer()
    {
        MainMenu.DeactivateMainMenu();
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

            }
        }
        base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
    }

}
