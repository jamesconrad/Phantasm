using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour
{

    public Camera playerCamera;
    private Transform playerTransform;
    private Transform gunTransform;

    private Vector2 MouseMovement;
    private Vector2 RecoilMovement;
    private Vector2 ClampMovement;
    public float MaxCameraY;
    public float MinCameraY;

    private Quaternion rot;

    // Use this for initialization
    void Start()
    {
        MouseMovement = new Vector2(0.0f, 0.0f);

        playerCamera = Camera.main;
        playerCamera.transform.parent = transform;
        playerTransform = GetComponent<Transform>();
        GunHandle temp = GetComponent<GunHandle>();
        playerCamera.transform.localPosition = new Vector3(0, 1.38f, 0.25f);
        gunTransform = temp.transform;
        //playerCamera.transform.position = temp.gunReference.transform.position + new Vector3(-0.10f, 0.1f, -0.2f);// + new Vector3(0.0f, 1.5f, 0.5f);
        playerCamera.transform.rotation = temp.gunReference.transform.rotation; // gunTransform.rotation;
    }

    public void AddCameraRotation(Vector2 vector)
    {
        RecoilMovement += vector;
    }

    public void AddCameraRotation(float x, float y)
    {
        AddCameraRotation(new Vector2(x, y));
    }

    public void FixedUpdate()
    {
        RecoilMovement *= 0.965f;
    }

    // Update is called once per frame
    void Update()
    {
        //Fetch mouse movement
        MouseMovement.x += Input.GetAxis("Mouse X");
        MouseMovement.y += Input.GetAxis("Mouse Y");

        MouseMovement.x += Input.GetAxis("GamePad X");
        MouseMovement.y += Input.GetAxis("GamePad Y");

        //Clamp pitch angle
        ClampMovement = MouseMovement;
        ClampMovement += RecoilMovement;
        MouseMovement.y = Mathf.Clamp(MouseMovement.y, MinCameraY, MaxCameraY);
        ClampMovement.y = Mathf.Clamp(ClampMovement.y, MinCameraY, MaxCameraY);

        //Generate rotation quaternion
        rot = Quaternion.Euler(-ClampMovement.y, ClampMovement.x, 0.0f);
        gunTransform.rotation = rot;
        //playerTransform.rotation = Quaternion.Euler(0.0f, MouseMovement.x, 0.0f);

        if (Input.GetMouseButtonDown(2))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void removeCamera()
    {
        playerCamera.transform.parent = null;
        playerCamera.transform.position = transform.position;
        playerCamera.transform.rotation = GetComponent<GunHandle>().gunReference.transform.rotation;
        playerCamera.GetComponent<AudioListener>().enabled = true;
        playerCamera.GetComponent<TAA>().enabled = true;
        playerCamera.GetComponent<FXAAScript>().enabled = true;
        playerCamera.GetComponent<Bloom>().enabled = true;
    }

    // This function is called when the MonoBehaviour will be destroyed
    public void OnDestroy()
    {
        playerCamera.transform.parent = null;
        playerCamera.transform.position = transform.position;
        playerCamera.transform.rotation = GetComponent<GunHandle>().gunReference.transform.rotation;
        playerCamera.GetComponent<AudioListener>().enabled = true;
        playerCamera.GetComponent<TAA>().enabled = true;
        playerCamera.GetComponent<FXAAScript>().enabled = true;
        playerCamera.GetComponent<Bloom>().enabled = true;

    }
}
