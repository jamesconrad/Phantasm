using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HackerInteractionWindowSetup : MonoBehaviour {

    public CameraPosition cameraPosition;

    public GameObject cameraButtonPrefab;
    private List<Camera> survCameras;
    private List<GameObject> survCameraButtons;

    // Use this for initialization
    void Start () {
        
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
        Camera[] tempCameras = FindObjectsOfType<Camera>();
        survCameras = new List<Camera>();
        for (int i = 0; i < tempCameras.Length; i++)
        {
            if (tempCameras[i].CompareTag("HackerCamera"))
            {
                survCameras.Add(tempCameras[i]);
            }
        }

        survCameraButtons = new List<GameObject>();
        for (int i = 0; i < survCameras.Count; i++)
        {
            GameObject tempButton = Instantiate(cameraButtonPrefab);
            tempButton.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());
            tempButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                UnityEngine.Random.Range(-GetComponent<RectTransform>().rect.width / 2.0f + tempButton.GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.width / 2.0f - tempButton.GetComponent<RectTransform>().rect.width), 
                UnityEngine.Random.Range(-GetComponent<RectTransform>().rect.height / 2.0f + tempButton.GetComponent<RectTransform>().rect.height, GetComponent<RectTransform>().rect.height / 2.0f - +tempButton.GetComponent<RectTransform>().rect.height));
            survCameraButtons.Add(tempButton);
            survCameraButtons[i].GetComponent<CameraButtonManipulation>().associatedCamera = survCameras[i];
        }

    }
}
