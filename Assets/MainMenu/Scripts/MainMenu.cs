using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called on the client when the connection was lost or you disconnected from the server
    public void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        GetComponent<Canvas>().enabled = true;
    }
}
