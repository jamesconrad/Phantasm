using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private static Canvas MenuCanvas;
   
    // Use this for initialization
    void Start()
    {
        MenuCanvas = GetComponent<Canvas>();
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

    public static void ActivateMainMenu()
    {
        MenuCanvas.enabled = true;
    }

    public static void DeactivateMainMenu()
    {
        MenuCanvas.enabled = false;
    }
}
