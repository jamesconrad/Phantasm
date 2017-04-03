using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotkeyCameraSwap : MonoBehaviour {

    public RawImage activationGlow;
    public Vector3 activationGlowHalfsize;
    public int cameraActivationButton = 0;
    public bool cameraActivationUpdate = false;

    // Use this for initialization
    void Start ()
    {
        activationGlowHalfsize.x = activationGlow.rectTransform.rect.width / 2;
        activationGlowHalfsize.y = activationGlow.rectTransform.rect.height / 2;
        cameraActivationUpdate = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            cameraActivationUpdate = true;
            cameraActivationButton = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            cameraActivationUpdate = true;
            cameraActivationButton = 2;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            cameraActivationUpdate = true;
            cameraActivationButton = 3;
        }
        else if (cameraActivationButton != 0)
        {
            cameraActivationUpdate = true;
            cameraActivationButton = 0;
        }

        if (cameraActivationUpdate)
        {
            if (cameraActivationButton == 0)
            {
                activationGlow.transform.position = new Vector3(-1000000, -1000000, 0);
            }
            else if (cameraActivationButton == 1)
            {
                activationGlow.transform.position = new Vector3(activationGlowHalfsize.x, activationGlowHalfsize.y * 3, 0);
            }
            else if (cameraActivationButton == 2)
            {
                activationGlow.transform.position = new Vector3(activationGlowHalfsize.x * 3, activationGlowHalfsize.y * 3, 0);
            }
            else if (cameraActivationButton == 3)
            {
                activationGlow.transform.position = new Vector3(activationGlowHalfsize.x * 3, activationGlowHalfsize.y, 0);
            }
            cameraActivationUpdate = false;
        }
    }

    public void SwapCamera(Camera cam)
    {
        HackerCameraView hcv = null;
        RawImage ri = null;
        
        if (cameraActivationButton == 1)
        {
            hcv = GameObject.FindGameObjectWithTag("HackerCamera1").GetComponent<HackerCameraView>();
            ri = GameObject.FindGameObjectWithTag("HackerCamera1").GetComponent<RawImage>();
        }
        else if (cameraActivationButton == 2)
        {
            hcv = GameObject.FindGameObjectWithTag("HackerCamera2").GetComponent<HackerCameraView>();
            ri = GameObject.FindGameObjectWithTag("HackerCamera2").GetComponent<RawImage>();
        }
        else if (cameraActivationButton == 3)
        {
            hcv = GameObject.FindGameObjectWithTag("HackerCamera3").GetComponent<HackerCameraView>();
            ri = GameObject.FindGameObjectWithTag("HackerCamera3").GetComponent<RawImage>();
        }

        if (hcv != null && ri != null)
        {
            hcv.selectedCamera.GetComponent<HackerCameraSettings>().SetCameraUsed(false);
            hcv.selectedCamera = cam;
            hcv.selectedCamera.GetComponent<HackerCameraSettings>().SetCameraUsed(true);
            ri.texture = hcv.selectedCamera.GetComponent<HackerCameraSettings>().renderTarget;
        }
    }
}
