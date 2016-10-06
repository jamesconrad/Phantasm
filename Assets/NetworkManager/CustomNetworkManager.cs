using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

public class CustomNetworkManager : NetworkManager
{
    public struct NetworkMessages
    {
        public static short SyncTransform = 7000;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        MainMenu.ActivateMainMenu();
        base.OnClientConnect(conn);
    }

    // Called on the client when the connection was lost or you disconnected from the server
    public void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        MainMenu.DeactivateMainMenu();
    }

    public override void OnStopServer()
    {
        MainMenu.DeactivateMainMenu();
        base.OnStopServer();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        if (extraMessageReader != null)
        {
            playerPrefab = spawnPrefabs[extraMessageReader.ReadMessage<IntegerMessage>().value];
            if (playerPrefab == spawnPrefabs[1])
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
