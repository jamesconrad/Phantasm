using UnityEngine;
using System.Collections;

public class SpawnAgent : MonoBehaviour
{


    public GameObject agent;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnMouseUpAsButton is only called when the mouse is released over the same GUIElement or Collider as it was pressed
    public void CreateObjectAndDisableMenu()
    {
        Instantiate(agent, new Vector3(0.0f, 0.5f, -22.0f), Quaternion.identity);

        GetComponentInParent<Canvas>().gameObject.SetActive(false);
    }

    // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
    public void OnMouseDown()
    {
        Instantiate(agent, new Vector3(0.0f, 0.5f, -22.0f), Quaternion.identity);
        //GetComponentInParent<Canvas>().gameObject.SetActive(false);
        //GetComponent<RectTransform>().root.gameObject.SetActive(false);
    }
}
