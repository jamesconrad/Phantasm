using System.Collections;
using System.Text;
using UnityEngine;

public class NetworkedMovement : NetworkedBehaviour
{
    public float maxDistance;

    private Vector3 simulatedPosition;

    private float ReceiveTime = 0.0f;
    public Vector3 receivedPosition;
    public Quaternion receivedRotation;
    public Vector3 receivedVelocity;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        simulatedPosition = GetComponent<Transform>().position;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        simulatedPosition = receivedPosition + receivedVelocity * (Time.time - ReceiveTime);
    }

    override public void ReceiveBuffer(ref StringBuilder buffer)
    {//TODO: Make sure this works.
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

        ReceiveTime = Time.time;

		return;
    }
}