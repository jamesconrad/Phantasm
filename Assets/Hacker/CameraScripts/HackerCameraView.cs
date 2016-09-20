using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public enum CameraPosition
{
    BottomLeft,
    BottomRight,
    TopLeft,
    TopRight
}

public class HackerCameraView : MonoBehaviour, IDropHandler
{

    //public Camera survCameras;

    public List<Camera> survCameras;
    public Camera selectedCamera;

    public CameraPosition cameraPosition;
    
    // Use this for initialization
    void Start() {
        //GetComponent<Canvas>().worldCamera = Camera.main;
        //GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        
        Camera[] tempCameras = FindObjectsOfType<Camera>();
        
        for (int i = 0; i < tempCameras.Length; i++)
        {
            if (tempCameras[i].CompareTag("HackerCamera"))
            {
                survCameras.Add(tempCameras[i]);
            }
        }

        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

        switch (cameraPosition)
        {
            case CameraPosition.BottomLeft:
                GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 4.0f, Screen.height / 4.0f);
                break;
            case CameraPosition.BottomRight:
                GetComponent<RectTransform>().anchoredPosition = new Vector2((Screen.width / 4.0f) + Screen.width / 2.0f, Screen.height / 4.0f);
                break;
            case CameraPosition.TopLeft:
                GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 4.0f, (Screen.height / 4.0f) + Screen.height / 2.0f);
                break;
            case CameraPosition.TopRight:
                GetComponent<RectTransform>().anchoredPosition = new Vector2((Screen.width / 4.0f) + Screen.width / 2.0f, (Screen.height / 4.0f) + Screen.height / 2.0f);
                break;
            default:
                GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 4.0f, Screen.height / 4.0f);
                break;
        }

        

        //Selects the first three cameras available, or just selects the first if there's nothing there.
        //this is a temp idea for the moment, as we need to figure out how we're doing this exactly.
        for (int i = 0; i < survCameras.Count; i++)
        {
            if (!survCameras[i].isActiveAndEnabled)
            {
                selectedCamera = survCameras[i];
                survCameras[i].GetComponent<HackerCameraSettings>().SetCameraUsed(true);
                break;
            }
        }
        if (selectedCamera == null && survCameras.Count > 0)
        {
            selectedCamera = survCameras[0];
            selectedCamera.GetComponent<HackerCameraSettings>().SetCameraUsed(true);
        }
        GetComponent<RawImage>().texture = selectedCamera.GetComponent<HackerCameraSettings>().renderTarget;
        GetComponent<RawImage>().SetNativeSize();
    }
    
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (selectedCamera != eventData.pointerDrag.GetComponent<CameraButtonManipulation>().associatedCamera)
        {
            selectedCamera.GetComponent<HackerCameraSettings>().SetCameraUsed(false);
            selectedCamera = eventData.pointerDrag.GetComponent<CameraButtonManipulation>().associatedCamera;
            selectedCamera.GetComponent<HackerCameraSettings>().SetCameraUsed(true);
            GetComponent<RawImage>().texture = selectedCamera.GetComponent<HackerCameraSettings>().renderTarget;
        }
    }    
}
