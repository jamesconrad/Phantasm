using UnityEngine;
using System.Collections;

public class FirstPersonMovement : MonoBehaviour
{

    private Transform playerTransform;

    private Rigidbody playerRigidBody;
    private Vector3 movementDirection;

    public float movementSpeed;
    // Use this for initialization
    void Start()
    {
        movementDirection = new Vector3(0.0f, 0.0f, 0.0f);

        playerTransform = GetComponent<Transform>();
        playerRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = (playerTransform.forward * Input.GetAxis("Vertical") + playerTransform.right * Input.GetAxis("Horizontal"));
       
        playerTransform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);
    }
}
