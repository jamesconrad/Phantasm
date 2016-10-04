using UnityEngine;
using UnityEngine.Networking;
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
}
