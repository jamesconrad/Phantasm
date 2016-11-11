using UnityEngine;
using System.Collections;

public class FirstPersonMovement : MonoBehaviour
{

    private Transform playerTransform;

    private Rigidbody playerRigidBody;
    private Vector3 movementDirection;

    public AudioSource footstepSound;

    public float movementSpeed;
    // Use this for initialization
    void Start()
    {
        movementDirection = new Vector3(0.0f, 0.0f, 0.0f);

        playerTransform = GetComponent<Transform>();
        playerTransform.position = FindObjectOfType<PlayerStartLocation>().transform.position;
        playerTransform.rotation = FindObjectOfType<PlayerStartLocation>().transform.rotation;
        playerRigidBody = GetComponent<Rigidbody>();        
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = (Camera.main.transform.forward /*playerTransform.forward*/ * Input.GetAxis("Vertical") + Camera.main.transform.right /*playerTransform.right*/ * Input.GetAxis("Horizontal"));
        movementDirection.y = 0.0f;

        if(Vector3.Distance(Vector3.zero, movementDirection) > 0.5f)
        {
            if(!footstepSound.isPlaying)
            { 
                footstepSound.Play();
            }
        }
        else
        {
            footstepSound.Stop();
        }

        playerTransform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);
    }
}
