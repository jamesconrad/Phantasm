using UnityEngine;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour {

    private Camera playerCamera;
    private Transform playerCameraTransform;

    private Vector2 MouseMovement;
    public float MaxCameraY;
    public float MinCameraY;

    private Quaternion rot;

    // Use this for initialization
    void Start()
    {
        MouseMovement = new Vector2(0.0f, 0.0f);

        playerCamera = Camera.main;
        playerCameraTransform = playerCamera.transform;

    }

    // Update is called once per frame
    void Update()
    {
        //Fetch mouse movement
        MouseMovement.x += Input.GetAxis("Mouse X");
        MouseMovement.y += Input.GetAxis("Mouse Y");

        //Clamp pitch angle
        MouseMovement.y = Mathf.Clamp(MouseMovement.y, MinCameraY, MaxCameraY);

        //Generate rotation quaternion
        rot = Quaternion.Euler(-MouseMovement.y, MouseMovement.x, 0.0f);

        playerCameraTransform.rotation = rot;
    }
}
