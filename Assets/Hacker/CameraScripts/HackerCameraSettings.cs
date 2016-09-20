using UnityEngine;
using System.Collections;

public class HackerCameraSettings : MonoBehaviour
{
    
    public RenderTexture renderTarget;
    private int timesUsed = 0;
    // Use this for initialization
    void Start()
    {
        renderTarget = new RenderTexture(Screen.width / 2, Screen.height / 2, 24);
        GetComponent<Camera>().targetTexture = renderTarget;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool SetCameraUsed(bool CameraIsUsed)
    {
        if (CameraIsUsed)
        {
            timesUsed++;
        }
        else
        {
            timesUsed--;
        }
        if (timesUsed > 0)
        {
            GetComponent<Camera>().enabled = true;
        }
        else if (timesUsed == 0)
        {
            GetComponent<Camera>().enabled = false;
        }
        else if (timesUsed < 0)
        {
            Debug.Log(gameObject + "Has been turned off too many times. There's a logic issue somewhere");
        }

        return enabled;
    }


}
