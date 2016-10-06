using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

public class SpawnAgent : MonoBehaviour
{

    public GameObject agent;

    // OnMouseUpAsButton is only called when the mouse is released over the same GUIElement or Collider as it was pressed
    public void CreateObjectAndDisableMenu()
    {
        IntegerMessage msg = new IntegerMessage(0);
        ClientScene.AddPlayer(NetworkManager.singleton.client.connection, 0, msg);

        GetComponentInParent<Canvas>().enabled = false;
    }
}
    
