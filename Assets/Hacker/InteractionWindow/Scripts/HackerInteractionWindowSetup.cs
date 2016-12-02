using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HackerInteractionWindowSetup : MonoBehaviour
{

    public CameraPosition cameraPosition;

    public GameObject cameraButtonPrefab;
    private List<Camera> survCameras;
    private List<GameObject> survCameraButtons;

    private Vector2 WindowSize;

    // Use this for initialization
    void Start()
    {
        WindowSize = new Vector2(Screen.width, Screen.height);

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


        Vector3 CameraPositionMax = survCameras[0].transform.position;
        Vector3 CameraPositionMin = survCameras[0].transform.position;

        for (int i = 0; i < survCameras.Count; i++)
        {
            Vector3 CameraPositionTemp = survCameras[i].transform.position;

            if(CameraPositionMax.x < CameraPositionTemp.x)
            {
                CameraPositionMax.x = CameraPositionTemp.x + 0.0f;
            }
            if (CameraPositionMax.y < CameraPositionTemp.y)
            {
                CameraPositionMax.y = CameraPositionTemp.y + 0.0f;
            }
            if (CameraPositionMax.z < CameraPositionTemp.z)
            {
                CameraPositionMax.z = CameraPositionTemp.z + 0.0f;
            }
            
            if (CameraPositionMin.x > CameraPositionTemp.x)
            {
                CameraPositionMin.x = CameraPositionTemp.x - 0.0f;
            }
            if (CameraPositionMin.y > CameraPositionTemp.y)
            {
                CameraPositionMin.y = CameraPositionTemp.y - 0.0f;
            }
            if (CameraPositionMin.z > CameraPositionTemp.z)
            {
                CameraPositionMin.z = CameraPositionTemp.z - 0.0f;
            }
        }

        for (int i = 0; i < survCameras.Count; i++)
        {
            GameObject tempButton = Instantiate(cameraButtonPrefab);
            tempButton.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());

            Vector3 LerpPosition = survCameras[i].transform.position;
            LerpPosition.x = Mathf.InverseLerp(CameraPositionMin.x, CameraPositionMax.x, survCameras[i].transform.position.x);
            LerpPosition.y = Mathf.InverseLerp(CameraPositionMin.y, CameraPositionMax.y, survCameras[i].transform.position.y);
            LerpPosition.z = Mathf.InverseLerp(CameraPositionMin.z, CameraPositionMax.z, survCameras[i].transform.position.z);

            tempButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.width,
                    GetComponent<RectTransform>().rect.width, LerpPosition.x),

                    0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.height,
                    GetComponent<RectTransform>().rect.height, LerpPosition.z));

            //tempButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            //    UnityEngine.Random.Range(
            //        GetComponent<RectTransform>().rect.width / 2.0f,
            //        -GetComponent<RectTransform>().rect.width / 2.0f),
            //
            //    UnityEngine.Random.Range(
            //        GetComponent<RectTransform>().rect.height / 2.0f,
            //        -GetComponent<RectTransform>().rect.height / 2.0f));

            //tempButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            //    UnityEngine.Random.Range(-GetComponent<RectTransform>().rect.width / 2.0f + tempButton.GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.width / 2.0f - tempButton.GetComponent<RectTransform>().rect.width),
            //    UnityEngine.Random.Range(-GetComponent<RectTransform>().rect.height / 2.0f + tempButton.GetComponent<RectTransform>().rect.height, GetComponent<RectTransform>().rect.height / 2.0f - +tempButton.GetComponent<RectTransform>().rect.height));


            survCameraButtons.Add(tempButton);
            survCameraButtons[i].GetComponent<CameraButtonManipulation>().associatedCamera = survCameras[i];
        }

    }

    public void Update()
    {
        if (WindowSize.x != Screen.width || WindowSize.y != Screen.height)
        {
            SetWindowSizes();
        }
    }

    private void FixedUpdate()
    {
        if (!Input.GetMouseButton(0))
        {
            Vector3 CameraPositionMax = survCameras[0].transform.position;
            Vector3 CameraPositionMin = survCameras[0].transform.position;

            for (int i = 0, count = survCameras.Count; i < count; i++)
            {
                Vector3 CameraPositionTemp = survCameras[i].transform.position;

                if (CameraPositionMax.x < CameraPositionTemp.x)
                {
                    CameraPositionMax.x = CameraPositionTemp.x + 0.0f;
                }
                if (CameraPositionMax.y < CameraPositionTemp.y)
                {
                    CameraPositionMax.y = CameraPositionTemp.y + 0.0f;
                }
                if (CameraPositionMax.z < CameraPositionTemp.z)
                {
                    CameraPositionMax.z = CameraPositionTemp.z + 0.0f;
                }

                if (CameraPositionMin.x > CameraPositionTemp.x)
                {
                    CameraPositionMin.x = CameraPositionTemp.x - 0.0f;
                }
                if (CameraPositionMin.y > CameraPositionTemp.y)
                {
                    CameraPositionMin.y = CameraPositionTemp.y - 0.0f;
                }
                if (CameraPositionMin.z > CameraPositionTemp.z)
                {
                    CameraPositionMin.z = CameraPositionTemp.z - 0.0f;
                }
            }

            for (int i = 0, count = survCameras.Count; i < count; i++)
            {
                survCameraButtons[i].GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());

                Vector3 LerpPosition = survCameras[i].transform.position;
                LerpPosition.x = Mathf.InverseLerp(CameraPositionMin.x, CameraPositionMax.x, survCameras[i].transform.position.x);
                LerpPosition.y = Mathf.InverseLerp(CameraPositionMin.y, CameraPositionMax.y, survCameras[i].transform.position.y);
                LerpPosition.z = Mathf.InverseLerp(CameraPositionMin.z, CameraPositionMax.z, survCameras[i].transform.position.z);

                survCameraButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.width,
                        GetComponent<RectTransform>().rect.width, LerpPosition.x),

                        0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.height,
                        GetComponent<RectTransform>().rect.height, LerpPosition.z));
                survCameraButtons[i].GetComponent<CameraButtonManipulation>().associatedCamera = survCameras[i];
            }
        }
    }

    void SetWindowSizes()
    {
        WindowSize = new Vector2(Screen.width, Screen.height);

    }
}
