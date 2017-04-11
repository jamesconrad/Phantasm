using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CameraRectType
{
    Normal,
    Map,
    Console,
    FloorChanger,
    HeartRate
}

public class HackerInteractionWindowSetup : MonoBehaviour
{
    public Camera cameraMap;
    public CameraPosition cameraPosition;
    public CameraRectType windowType = CameraRectType.Normal;

    public GameObject cameraButtonPrefab;
    public GameObject agentButtonPrefab;
    public GameObject doorButtonPrefab;
    public GameObject speakerButtonPrefab;
    public GameObject pickupButtonPrefab;
    public Texture healthTexture;
    public Texture ammoTexture;
    private List<Camera> survCameras;
    private List<GameObject> survCameraButtons;

    // List of Doors for drawing    
    private List<GoodDoor> survDoors;
    private List<GameObject> survDoorButtons;

    // List of Speakers for drawing    
    private List<CodeVoice> survSpeakers;
    private List<GameObject> survSpeakerButtons;
    

    // List of Pickups for drawing    
    private List<PickupScript> survPickups;
    private List<GameObject> survPickupButtons;

    public bool WindowIsInteractive = true;

    private Vector2 WindowSize;
    
    public int numOfFloors = 2;
    public int viewFloor = 0;
    private float floorHeight;
    private int agentFloor = 0;

    // Use this for initialization
    void Start()
    {
        WindowSize = new Vector2(Screen.width, Screen.height);

        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

        cameraMap = GameObject.Find("HackerMapPrefab").GetComponent<Camera>();

        SetWindowSizes();
        //switch (cameraPosition)
        //{
        //    case CameraPosition.BottomLeft:
        //        GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 4.0f, Screen.height / 4.0f);
        //        break;
        //    case CameraPosition.BottomRight:
        //        GetComponent<RectTransform>().anchoredPosition = new Vector2((Screen.width / 4.0f) + Screen.width / 2.0f, Screen.height / 4.0f);
        //        break;
        //    case CameraPosition.TopLeft:
        //        GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 4.0f, (Screen.height / 4.0f) + Screen.height / 2.0f);
        //        break;
        //    case CameraPosition.TopRight:
        //        GetComponent<RectTransform>().anchoredPosition = new Vector2((Screen.width / 4.0f) + Screen.width / 2.0f, (Screen.height / 4.0f) + Screen.height / 2.0f);
        //        break;
        //    default:
        //        GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 4.0f, Screen.height / 4.0f);
        //        break;
        //}



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

        
        GoodDoor[] tempDoors = FindObjectsOfType<GoodDoor>();
        survDoors = new List<GoodDoor>();
        for (int i = 0; i < tempDoors.Length; i++)
        {
            if (tempDoors[i].roomNumber != 0 && (true || tempDoors[i].code.Length > 0))
            {
                survDoors.Add(tempDoors[i]);
                tempDoors[i].locked = true;
            }
        }
        survDoorButtons = new List<GameObject>();

        CodeVoice[] tempSpeakers = FindObjectsOfType<CodeVoice>();
        survSpeakers = new List<CodeVoice>();
        for (int i = 0; i < tempSpeakers.Length; i++)
        {
            
            survSpeakers.Add(tempSpeakers[i]);
        }
        survSpeakerButtons = new List<GameObject>();

        PickupScript[] tempPickups = FindObjectsOfType<PickupScript>();
        survPickups = new List<PickupScript>();
        for (int i = 0; i < tempPickups.Length; i++)
        {
            if (tempPickups[i].isActiveAndEnabled)
            {
                survPickups.Add(tempPickups[i]);
            }
        }
        survPickupButtons = new List<GameObject>();

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
        
        for (int i = 0; i < survDoors.Count; i++)
        {
            Vector3 DoorPositionTemp = survDoors[i].transform.position;

            if(CameraPositionMax.x < DoorPositionTemp.x)
            {
                CameraPositionMax.x = DoorPositionTemp.x + 0.0f;
            }
            if (CameraPositionMax.y < DoorPositionTemp.y)
            {
                CameraPositionMax.y = DoorPositionTemp.y + 0.0f;
            }
            if (CameraPositionMax.z < DoorPositionTemp.z)
            {
                CameraPositionMax.z = DoorPositionTemp.z + 0.0f;
            }
            
            if (CameraPositionMin.x > DoorPositionTemp.x)
            {
                CameraPositionMin.x = DoorPositionTemp.x - 0.0f;
            }
            if (CameraPositionMin.y > DoorPositionTemp.y)
            {
                CameraPositionMin.y = DoorPositionTemp.y - 0.0f;
            }
            if (CameraPositionMin.z > DoorPositionTemp.z)
            {
                CameraPositionMin.z = DoorPositionTemp.z - 0.0f;
            }
        }

        for (int i = 0; i < survSpeakers.Count; i++)
        {
            Vector3 SpeakerPositionTemp = survSpeakers[i].transform.position;

            if(CameraPositionMax.x < SpeakerPositionTemp.x)
            {
                CameraPositionMax.x = SpeakerPositionTemp.x + 0.0f;
            }
            if (CameraPositionMax.y < SpeakerPositionTemp.y)
            {
                CameraPositionMax.y = SpeakerPositionTemp.y + 0.0f;
            }
            if (CameraPositionMax.z < SpeakerPositionTemp.z)
            {
                CameraPositionMax.z = SpeakerPositionTemp.z + 0.0f;
            }
            
            if (CameraPositionMin.x > SpeakerPositionTemp.x)
            {
                CameraPositionMin.x = SpeakerPositionTemp.x - 0.0f;
            }
            if (CameraPositionMin.y > SpeakerPositionTemp.y)
            {
                CameraPositionMin.y = SpeakerPositionTemp.y - 0.0f;
            }
            if (CameraPositionMin.z > SpeakerPositionTemp.z)
            {
                CameraPositionMin.z = SpeakerPositionTemp.z - 0.0f;
            }
        }

        for (int i = 0; i < survPickups.Count; i++)
        {
            Vector3 SpeakerPositionTemp = survPickups[i].transform.position;

            if(CameraPositionMax.x < SpeakerPositionTemp.x)
            {
                CameraPositionMax.x = SpeakerPositionTemp.x + 0.0f;
            }
            if (CameraPositionMax.y < SpeakerPositionTemp.y)
            {
                CameraPositionMax.y = SpeakerPositionTemp.y + 0.0f;
            }
            if (CameraPositionMax.z < SpeakerPositionTemp.z)
            {
                CameraPositionMax.z = SpeakerPositionTemp.z + 0.0f;
            }
            
            if (CameraPositionMin.x > SpeakerPositionTemp.x)
            {
                CameraPositionMin.x = SpeakerPositionTemp.x - 0.0f;
            }
            if (CameraPositionMin.y > SpeakerPositionTemp.y)
            {
                CameraPositionMin.y = SpeakerPositionTemp.y - 0.0f;
            }
            if (CameraPositionMin.z > SpeakerPositionTemp.z)
            {
                CameraPositionMin.z = SpeakerPositionTemp.z - 0.0f;
            }
        }

        
        floorHeight = (CameraPositionMax.y - CameraPositionMin.y) / numOfFloors;

        if(cameraMap != null)
        {
            CameraPositionMin.x = cameraMap.transform.position.x - cameraMap.orthographicSize;
            CameraPositionMax.x = cameraMap.transform.position.x + cameraMap.orthographicSize;
            CameraPositionMin.z = cameraMap.transform.position.z - cameraMap.orthographicSize;
            CameraPositionMax.z = cameraMap.transform.position.z + cameraMap.orthographicSize;
            Debug.Log("cameraMap bounds set");
        }
        else
        {
            Debug.Log("cameraMap is null");
        }

        for (int i = 0; i < survCameras.Count; i++)
        {
            GameObject tempButton;
            
            if (survCameras[i].transform.parent.CompareTag("Player"))
            {
                tempButton = Instantiate(agentButtonPrefab);
                tempButton.GetComponent<RectTransform>().position -= new Vector3(0.0f, 0.0f, 1.0f);
            }
            else
            {
                tempButton = Instantiate(cameraButtonPrefab);
            }
            
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

           

            survCameraButtons.Add(tempButton);
            survCameraButtons[i].GetComponent<CameraButtonManipulation>().associatedCamera = survCameras[i];
            
            if (!survCameras[i].transform.parent.CompareTag("Player"))
            {
                survCameraButtons[i].GetComponent<RectTransform>().SetSiblingIndex(0);
            }
        }

        // Code to put the doors as buttons


        for (int i = 0; i < survDoors.Count; i++)
        {
            GameObject tempButton = Instantiate(doorButtonPrefab);
            tempButton.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());

            Vector3 LerpPosition = survDoors[i].transform.position;
            LerpPosition.x = Mathf.InverseLerp(CameraPositionMin.x, CameraPositionMax.x, survDoors[i].transform.position.x);
            LerpPosition.y = Mathf.InverseLerp(CameraPositionMin.y, CameraPositionMax.y, survDoors[i].transform.position.y);
            LerpPosition.z = Mathf.InverseLerp(CameraPositionMin.z, CameraPositionMax.z, survDoors[i].transform.position.z);

            tempButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.width,
                    GetComponent<RectTransform>().rect.width, LerpPosition.x),

                    0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.height,
                    GetComponent<RectTransform>().rect.height, LerpPosition.z));

            RawImage image = tempButton.GetComponent<RawImage>();
            int doorNum = survDoors[i].roomNumber - 1;
            float doorNumBrightness = (doorNum / 8) * 0.25f;
            doorNum = doorNum % 8;
            Color color = Color.HSVToRGB(doorNum / 8.0f, 1.0f - doorNumBrightness, 1.0f);
            image.color = color;

            survDoorButtons.Add(tempButton);
        }

        for (int i = 0; i < survSpeakers.Count; i++)
        {
            GameObject tempButton = Instantiate(speakerButtonPrefab);
            tempButton.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());

            Vector3 LerpPosition = survSpeakers[i].transform.position;
            LerpPosition.x = Mathf.InverseLerp(CameraPositionMin.x, CameraPositionMax.x, survSpeakers[i].transform.position.x);
            LerpPosition.y = Mathf.InverseLerp(CameraPositionMin.y, CameraPositionMax.y, survSpeakers[i].transform.position.y);
            LerpPosition.z = Mathf.InverseLerp(CameraPositionMin.z, CameraPositionMax.z, survSpeakers[i].transform.position.z);

            tempButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.width,
                    GetComponent<RectTransform>().rect.width, LerpPosition.x),

                    0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.height,
                    GetComponent<RectTransform>().rect.height, LerpPosition.z));

            RawImage image = tempButton.GetComponent<RawImage>();
            int doorNum = survSpeakers[i].roomNumber - 1;
            float doorNumBrightness = (doorNum / 10) * 0.5f;
            doorNum = doorNum % 10;
            Color color = Color.HSVToRGB(doorNum / 10.0f, 1.0f - doorNumBrightness, 1.0f);
            image.color = color;

            survSpeakerButtons.Add(tempButton);
        }

        for (int i = 0; i < survPickups.Count; i++)
        {
            GameObject tempButton = Instantiate(pickupButtonPrefab);
            tempButton.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());

            Vector3 LerpPosition = survPickups[i].transform.position;
            LerpPosition.x = Mathf.InverseLerp(CameraPositionMin.x, CameraPositionMax.x, survPickups[i].transform.position.x);
            LerpPosition.y = Mathf.InverseLerp(CameraPositionMin.y, CameraPositionMax.y, survPickups[i].transform.position.y);
            LerpPosition.z = Mathf.InverseLerp(CameraPositionMin.z, CameraPositionMax.z, survPickups[i].transform.position.z);

            tempButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.width,
                    GetComponent<RectTransform>().rect.width, LerpPosition.x),

                    0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.height,
                    GetComponent<RectTransform>().rect.height, LerpPosition.z));

            RawImage image = tempButton.GetComponent<RawImage>();
            if(survPickups[i].itemType == PickupType.Ammo)
                image.texture = ammoTexture;
            if(survPickups[i].itemType == PickupType.Health)
                image.texture = healthTexture;

            survPickupButtons.Add(tempButton);
        }
    }

    //used for translating map up a floor
    public void FloorUp()
    {
        if (viewFloor < numOfFloors - 1)
            viewFloor++;
    }

    //used for translating map down a floor
    public void FloorDown()
    {
        if (viewFloor > 0)
            viewFloor--;
    }

    //used for translating map to agents floor
    public void FloorAgent()
    {
        viewFloor = agentFloor;
    }

    public void Update()
    {
        if (WindowSize.x != Screen.width || WindowSize.y != Screen.height)
        {
            SetWindowSizes();
        }

        for (int i = 0, count = survCameras.Count; i < count; i++)
        {
            survCameraButtons[i].GetComponent<RectTransform>().rotation = 
                Quaternion.Euler(0.0f, 0.0f, -survCameras[i].transform.rotation.eulerAngles.y);
        }

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

            if(cameraMap != null)
            {
                CameraPositionMin.x = cameraMap.transform.position.x - cameraMap.orthographicSize;
                CameraPositionMax.x = cameraMap.transform.position.x + cameraMap.orthographicSize;
                CameraPositionMin.z = cameraMap.transform.position.z - cameraMap.orthographicSize;
                CameraPositionMax.z = cameraMap.transform.position.z + cameraMap.orthographicSize;
            }
            else
            {
                Debug.Log("cameraMap is null");
            }

            for (int i = 0, count = survCameras.Count; i < count; i++)
            {
                survCameraButtons[i].GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());

                Vector3 LerpPosition = survCameras[i].transform.position;
                LerpPosition.x = Mathf.InverseLerp(CameraPositionMin.x, CameraPositionMax.x, survCameras[i].transform.position.x);
                LerpPosition.y = Mathf.InverseLerp(CameraPositionMin.y, CameraPositionMax.y, survCameras[i].transform.position.y);
                LerpPosition.z = Mathf.InverseLerp(CameraPositionMin.z, CameraPositionMax.z, survCameras[i].transform.position.z);

                survCameraButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        0.5f * Mathf.Lerp(-GetComponent<RectTransform>().rect.width,
                        GetComponent<RectTransform>().rect.width, LerpPosition.x) + 0.0f,
                        0.5f * Mathf.Lerp(-GetComponent<RectTransform>().rect.height,
                        GetComponent<RectTransform>().rect.height, LerpPosition.z) - 0.0f);
                survCameraButtons[i].GetComponent<CameraButtonManipulation>().associatedCamera = survCameras[i];
                survCameraButtons[i].GetComponent<CameraButtonManipulation>().DelayedStart();

                survCameraButtons[i].GetComponent<Button>().interactable = WindowIsInteractive;

                //if (survCameras[i].transform.parent.CompareTag("Player"))
                //{
                //    float curY = survCameras[i].transform.position.y;
                //    for (int f = 0; f < numOfFloors; f++)
                //    {
                //        if (curY >= CameraPositionMin.y + f * floorHeight && curY <= CameraPositionMin.y + (f + 1) * floorHeight)
                //        {
                //            agentFloor = f;
                //            break;
                //        }
                //    }
                //}

                if(survCameras[i].transform.position.y >= CameraPositionMin.y + viewFloor * floorHeight
                && survCameras[i].transform.position.y <= CameraPositionMin.y + (viewFloor + 1) * floorHeight
                && survCameras[i].transform.position.x <= CameraPositionMax.x
                && survCameras[i].transform.position.x >= CameraPositionMin.x
                && survCameras[i].transform.position.z <= CameraPositionMax.z
                && survCameras[i].transform.position.z >= CameraPositionMin.z)
                {
                    survCameraButtons[i].SetActive(true);
                }
                else
                {
                    survCameraButtons[i].SetActive(false);
                }

                
            }

            for (int i = 0; i < survDoors.Count; i++)
            {
                if (survDoors[i].code.Length > 0 && survDoors[i].code != "CHEATER" && !survDoors[i].unlocked)
                {
                    survDoors[i].locked = true;
                }
                else if(survDoors[i].code.Length == 0)
                {
                    survDoors[i].unlocked = true;
                }


                survDoorButtons[i].GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());

                Vector3 LerpPosition = survDoors[i].transform.position;
                LerpPosition.x = Mathf.InverseLerp(CameraPositionMin.x, CameraPositionMax.x, survDoors[i].transform.position.x);
                LerpPosition.y = Mathf.InverseLerp(CameraPositionMin.y, CameraPositionMax.y, survDoors[i].transform.position.y);
                LerpPosition.z = Mathf.InverseLerp(CameraPositionMin.z, CameraPositionMax.z, survDoors[i].transform.position.z);

                survDoorButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.width,
                        GetComponent<RectTransform>().rect.width, LerpPosition.x),

                        0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.height,
                        GetComponent<RectTransform>().rect.height, LerpPosition.z));
                    
                if(survDoors[i].transform.position.y >= CameraPositionMin.y + viewFloor * floorHeight
                && survDoors[i].transform.position.y <= CameraPositionMin.y + (viewFloor + 1) * floorHeight
                && survDoors[i].transform.position.x <= CameraPositionMax.x
                && survDoors[i].transform.position.x >= CameraPositionMin.x
                && survDoors[i].transform.position.z <= CameraPositionMax.z
                && survDoors[i].transform.position.z >= CameraPositionMin.z)
                {
                    survDoorButtons[i].SetActive(true);
                }
                else
                {
                    survDoorButtons[i].SetActive(false);
                }

                if(survDoors[i].unlocked)
                {                    
                    survDoorButtons[i].GetComponent<Button>().interactable = false;
                }
                else
                {

                }
            }


            for (int i = 0; i < survSpeakers.Count; i++)
            {
                if (survSpeakers[i].getCode().Length > 0 && survSpeakers[i].getCode() != "CHEATER")
                {
                    survSpeakers[i].codeGenned = true;
                }

                survSpeakerButtons[i].GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());

                Vector3 LerpPosition = survSpeakers[i].transform.position;
                LerpPosition.x = Mathf.InverseLerp(CameraPositionMin.x, CameraPositionMax.x, survSpeakers[i].transform.position.x);
                LerpPosition.y = Mathf.InverseLerp(CameraPositionMin.y, CameraPositionMax.y, survSpeakers[i].transform.position.y);
                LerpPosition.z = Mathf.InverseLerp(CameraPositionMin.z, CameraPositionMax.z, survSpeakers[i].transform.position.z);

                survSpeakerButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.width,
                        GetComponent<RectTransform>().rect.width, LerpPosition.x),

                        0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.height,
                        GetComponent<RectTransform>().rect.height, LerpPosition.z));
                    
                if(survSpeakers[i].transform.position.y >= CameraPositionMin.y + viewFloor * floorHeight
                && survSpeakers[i].transform.position.y <= CameraPositionMin.y + (viewFloor + 1) * floorHeight
                && survSpeakers[i].transform.position.x <= CameraPositionMax.x
                && survSpeakers[i].transform.position.x >= CameraPositionMin.x
                && survSpeakers[i].transform.position.z <= CameraPositionMax.z
                && survSpeakers[i].transform.position.z >= CameraPositionMin.z)
                {
                    survSpeakerButtons[i].SetActive(true);
                }
                else
                {
                    survSpeakerButtons[i].SetActive(false);
                }

                if(survSpeakers[i].codeGenned)
                {                    
                    survSpeakerButtons[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    survSpeakerButtons[i].GetComponent<Button>().interactable = false;
                }

                /// TODO 
                // remove speakers from the map when their code is entered
                
            }

            for (int i = 0; i < survPickups.Count; i++)
            {
                survPickupButtons[i].GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());

                Vector3 LerpPosition = survPickups[i].transform.position;
                LerpPosition.x = Mathf.InverseLerp(CameraPositionMin.x, CameraPositionMax.x, survPickups[i].transform.position.x);
                LerpPosition.y = Mathf.InverseLerp(CameraPositionMin.y, CameraPositionMax.y, survPickups[i].transform.position.y);
                LerpPosition.z = Mathf.InverseLerp(CameraPositionMin.z, CameraPositionMax.z, survPickups[i].transform.position.z);

                survPickupButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.width,
                        GetComponent<RectTransform>().rect.width, LerpPosition.x),

                        0.45f * Mathf.Lerp(-GetComponent<RectTransform>().rect.height,
                        GetComponent<RectTransform>().rect.height, LerpPosition.z));

                if(survPickups[i].transform.position.y >= CameraPositionMin.y + viewFloor * floorHeight
                && survPickups[i].transform.position.y <= CameraPositionMin.y + (viewFloor + 1) * floorHeight
                && survPickups[i].transform.position.x <= CameraPositionMax.x
                && survPickups[i].transform.position.x >= CameraPositionMin.x
                && survPickups[i].transform.position.z <= CameraPositionMax.z
                && survPickups[i].transform.position.z >= CameraPositionMin.z
                && survPickups[i] != null)
                {
                    survPickupButtons[i].SetActive(true);
                }
                else
                {
                    survPickupButtons[i].SetActive(false);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    void SetWindowSizes()
    {
        WindowSize = new Vector2(Screen.width, Screen.height);

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

        
        switch(windowType)
        {
            case CameraRectType.Normal:
                break;
            case CameraRectType.Map:
                GetComponent<RectTransform>().anchoredPosition += new Vector2((Screen.width / 4.0f * 0.325f / (Screen.height / (float)Screen.width)), Screen.height / 4.0f * 0.25f);
                //GetComponent<RectTransform>().localScale = new Vector3(0.75f, 0.75f, 1.0f);
                GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 2.0f * (Screen.height / (float)Screen.width), Screen.height / 2.0f) * 0.75f;
                //GetComponent<RectTransform>().localScale = Vector3.Scale(GetComponent<RectTransform>().localScale, new Vector3(0.5f, 1.0f, 1.0f));
                break;
            case CameraRectType.Console:
                break;
            case CameraRectType.FloorChanger:
                break;
            case CameraRectType.HeartRate:
                break;
                
        }

    }
}
