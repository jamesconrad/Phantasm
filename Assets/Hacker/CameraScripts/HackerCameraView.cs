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
    public Font _Font;
    //public Camera survCameras;

    public List<Camera> survCameras;
    public Camera selectedCamera;

    public CameraPosition cameraPosition;

    private Vector2 WindowSize;


    // Use this for initialization
    void Start() {

        WindowSize = new Vector2(Screen.width, Screen.height);

        //GetComponent<Canvas>().worldCamera = Camera.main;
        //GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        // Number for clock
        //randomTimeAdd = UnityEngine.Random.Range(60000.0f, 100000.0f);

        randomTimeAdd = 79200 + 2460 + 10;

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
    
    void Update()
    {
        if (WindowSize.x != Screen.width || WindowSize.y != Screen.height)
        {
            SetWindowSizes();
        }
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


    float randomTimeAdd;// = UnityEngine.Random.Range(600.0f, 1000.0f);

    void OnGUI()
    {
        int w = Screen.width / 2, h = Screen.height / 2;

        int wOffset = 0;
        int hOffset = 0;

        switch (cameraPosition)
        {
            case CameraPosition.BottomLeft:
                hOffset = h;
                GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 4.0f, Screen.height / 4.0f);
                break;
            case CameraPosition.BottomRight:
                wOffset = w;
                hOffset = h;
                GetComponent<RectTransform>().anchoredPosition = new Vector2((Screen.width / 4.0f) + Screen.width / 2.0f, Screen.height / 4.0f);
                break;
            case CameraPosition.TopLeft:
                GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 4.0f, (Screen.height / 4.0f) + Screen.height / 2.0f);
                break;
            case CameraPosition.TopRight:
                wOffset = w;
                GetComponent<RectTransform>().anchoredPosition = new Vector2((Screen.width / 4.0f) + Screen.width / 2.0f, (Screen.height / 4.0f) + Screen.height / 2.0f);
                break;
            default:
                GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 4.0f, Screen.height / 4.0f);
                break;
        }

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(wOffset + (w * 0.01f), hOffset + (h * 0.01f), w - (w * 0.02f), h - (h * 0.02f));
        style.alignment = TextAnchor.UpperLeft;
        style.font = _Font;
        style.fontSize = h * 6 / 100;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        float timer = Time.timeSinceLevelLoad + randomTimeAdd;
        float msec = Mathf.Floor((timer * 100) % 100); // * 1000.0f;
        float seconds = Mathf.Floor(timer % 60);
        float minutes = Mathf.Floor((timer / 60) % 60);
        float hours = Mathf.Floor((timer / 3600) % 60);

        string milliStr;
        string secondsStr;
        string minutesStr;
        string hoursStr;

        string blinkingColon = ":";
        if (msec > 50)
            blinkingColon = " ";

        hoursStr = DoubleDigitConvert(hours);
        minutesStr = DoubleDigitConvert(minutes);
        secondsStr = DoubleDigitConvert(seconds); 
        milliStr = DoubleDigitConvert(msec);

        string frameNumber = DoubleDigitConvert((Time.time - Mathf.Floor(Time.time)) * 30.0f);
        string text =
            string.Format("{00}", hoursStr) + blinkingColon + 
            string.Format("{00}", minutesStr) + blinkingColon +
            string.Format("{00}.", secondsStr) +
            string.Format("{00}\n", milliStr) +
            string.Format("[{00}]", frameNumber);

        //string text = string.Format("{00}:{00}.{00}, ({00}) Frames", minutes, seconds, msec, frameNumber);
        GUI.Label(rect, text, style);

        GUIStyle styleBottomLeft = style;
        styleBottomLeft.alignment = TextAnchor.LowerLeft;

        text = "15/12/2017";
        GUI.Label(rect, text, styleBottomLeft);


        GUIStyle styleBottomRight = style;
        styleBottomRight.alignment = TextAnchor.LowerRight;

        text = "v0.41";
        GUI.Label(rect, text, styleBottomRight);
    }

    string DoubleDigitConvert(float num)
    {
        if (num < 10)
            return "0" + Mathf.RoundToInt(Mathf.Floor(num)).ToString();
        else
            return Mathf.RoundToInt(Mathf.Floor(num)).ToString();
    }


    void SetWindowSizes()
    {
        WindowSize = new Vector2(Screen.width, Screen.height);

        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
    }
}
