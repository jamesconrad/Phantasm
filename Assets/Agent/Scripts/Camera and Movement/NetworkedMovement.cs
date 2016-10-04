using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkedMovement : NetworkBehaviour
{
    public float maxDistance;

    [SyncVar]
    private double sendTime;

    private double startTime = 0.0;

    [SyncVar]
    private Vector3 syncedPosition;

    private Vector3 simulatedPosition;

    [SyncVar]
    private Quaternion syncedRotation;

    [SyncVar]
    private Vector3 syncedVelocity;

    public bool objectIsClient = true;



    public class TransformMessage : MessageBase
    {
        public double sendTimeFromClient;
    }

    //private Rigidbody playerRigidBody;



    // Use this for initialization
    void Start()
    {
        sendTime = Network.time;
    }

    public void OnConnectedToServer()
    {
    }



    // Called on clients for player objects for the local client (only)
    public override void OnStartLocalPlayer()
    {
    }

    // Update is called once per frame
    void Update()
    {
        simulatedPosition = syncedPosition + syncedVelocity * (float)(Network.time - sendTime);

        if (objectIsClient)
        {
            if (!isLocalPlayer)
            {
                transform.position = Vector3.Lerp(transform.position, simulatedPosition, 0.25f);
                transform.rotation = syncedRotation;
            }
            else if (isLocalPlayer)
            {
                CmdSyncRotation(transform.rotation);
                if ((simulatedPosition - transform.position).magnitude >= maxDistance)
                {
                    CmdSyncMovement(transform.position, GetComponentInParent<Rigidbody>().velocity, transform.rotation, Network.time);
                }
            }
        }
        else
        {
            if (!isServer)
            {
                transform.position = Vector3.Lerp(transform.position, simulatedPosition, 0.25f);
                transform.rotation = syncedRotation;
            }
            else if (isServer)
            {
                ServerSyncRotation(transform.rotation);
                if ((simulatedPosition - transform.position).magnitude >= maxDistance)
                {
                    ServerSyncMovement(transform.position, GetComponentInParent<Rigidbody>().velocity, transform.rotation, Network.time);
                }
            }
        }
    }

    [Command]
    private void CmdSyncRotation(Quaternion _rotation)
    {
        syncedRotation = _rotation;
    }

    // Called to gather state to send from the server to clients
    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        return true;
    }

    [Command]
    private void CmdSyncMovement(Vector3 _position, Vector3 _velocity, Quaternion _rotation, double _time)
    {
        syncedPosition = _position;
        syncedVelocity = _velocity;
        //syncedRotation = _rotation;
        sendTime = _time;
    }

    [Server]
    private void ServerSyncRotation(Quaternion _rotation)
    {
        syncedRotation = _rotation;
    }

    [Server]
    private void ServerSyncMovement(Vector3 _position, Vector3 _velocity, Quaternion _rotation, double _time)
    {
        syncedPosition = _position;
        syncedVelocity = _velocity;
        //syncedRotation = _rotation;
        sendTime = _time;
    }
}
