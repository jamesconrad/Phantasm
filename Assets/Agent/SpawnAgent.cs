using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SpawnAgent : MonoBehaviour
{
	
    public GameObject agent;

    // OnMouseUpAsButton is only called when the mouse is released over the same GUIElement or Collider as it was pressed
    public void CreateObjectAndDisableMenu()
    {
		ClientScene.AddPlayer (0);
        //Instantiate(agent, new Vector3(0.0f, 0.5f, -22.0f), Quaternion.identity);

        GetComponentInParent<Canvas>().gameObject.SetActive(false);
    }

    // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
    public void OnMouseDown()
    {
		//NetworkManager.Instantiate(agent, new Vector3(0.0f, 0.5f, -22.0f), Quaternion.identity);
		ClientScene.AddPlayer (0);
    }
}
