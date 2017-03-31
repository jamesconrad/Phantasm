using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NetworkedMovement : NetworkedBehaviour
{
    public float maxDistance;
    public bool isRemote = false;

    [Range(0.0f, 2.0f)]
    public float SendInterval = 0.5f;
    private float TimeSinceSent = 0.0f;

    private Transform objectTransform;
    private Rigidbody objectRigidBody;
    private Transform simulatedTransform;
    private Vector3 simulatedPosition;
    private Vector3 simulatedVelocity;
    private Quaternion simulatedRotation;

    private float ReceiveTime = 0.0f;
    public Vector3 receivedPosition;
    private Quaternion receivedRotation;
    private Vector3 receivedVelocity;

    public GameObject sendTimeUI; Text uiText;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        objectTransform = transform;
        objectRigidBody = GetComponent<Rigidbody>();  
        if (isRemote)
        {
            simulatedPosition = transform.position;        
            simulatedVelocity = objectRigidBody.velocity;
        }
        else
        {
            simulatedPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            simulatedPosition = new Vector3(objectRigidBody.velocity.x, objectRigidBody.velocity.y, objectRigidBody.velocity.z);
            simulatedRotation = new Quaternion(objectRigidBody.rotation.x, objectRigidBody.rotation.y, objectRigidBody.rotation.z, objectRigidBody.rotation.w);
            sendTimeUI = GameObject.Instantiate(sendTimeUI);
            uiText = sendTimeUI.GetComponentInChildren<Text>();
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (isRemote)
        {//Actually update the object
            objectTransform.position = Vector3.Lerp(objectTransform.position, receivedPosition + receivedVelocity * (Time.time - ReceiveTime), 0.3f);
        }
        else
        {//Just simulate the updating.
            simulatedPosition = Vector3.Lerp(objectTransform.position, receivedPosition + receivedVelocity * (Time.time - ReceiveTime), 0.3f);
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SendInterval += Time.fixedDeltaTime;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SendInterval -= Time.fixedDeltaTime;
            }
            uiText.text = "Send interval: " + SendInterval;
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        TimeSinceSent += Time.fixedDeltaTime;
        if (!isRemote && ((simulatedVelocity != objectRigidBody.velocity || simulatedRotation != objectTransform.rotation) || Vector3.Distance(objectTransform.position, simulatedPosition) > maxDistance) && TimeSinceSent >= SendInterval)
        {//Sending
            SendPlayerUpdate(objectTransform.transform.position, objectRigidBody.velocity, objectTransform.transform.rotation, PhaNetworkingAPI.targetIP);
            simulatedVelocity = objectRigidBody.velocity;
            simulatedRotation = objectTransform.rotation;
            TimeSinceSent = 0.0f;
        }
    }

    public override void ReceiveBuffer(ref StringBuilder buffer)
    {
        string[] message = buffer.ToString().Split(' ');

		receivedPosition.x = float.Parse(message[1]);
		receivedPosition.y = float.Parse(message[2]);
		receivedPosition.z = float.Parse(message[3]);

		receivedVelocity.x = float.Parse(message[4]);
		receivedVelocity.y = float.Parse(message[5]);
		receivedVelocity.z = float.Parse(message[6]);

		receivedRotation.w = float.Parse(message[7]);
		receivedRotation.x = float.Parse(message[8]);
		receivedRotation.y = float.Parse(message[9]);
		receivedRotation.z = float.Parse(message[10]);

        simulatedRotation = objectTransform.rotation = receivedRotation;

        ReceiveTime = Time.time;

		return;
    }
}