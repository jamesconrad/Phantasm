using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkedMovement : NetworkBehaviour
{
    public float maxDistance;

    private Vector3 simulatedPosition;


    private double sendTime;
    private double startTime = 0.0;
    private Vector3 syncedPosition;
    private Quaternion syncedRotation;
    private Vector3 syncedVelocity;

    public bool objectIsClient = true;

    double timeDifference;



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

    // Update is called once per frame
    void Update()
    {
        simulatedPosition = syncedPosition;// + syncedVelocity * (float)((Network.time - timeDifference) - sendTime);

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


    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        writer.Write(syncedPosition);
        writer.Write(syncedVelocity);
        writer.Write(syncedRotation);
        writer.Write(Network.time);
        writer.Write(startTime);
        return true;
    }

    // Called to apply state to objects on clients
    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        syncedPosition = reader.ReadVector3();
        syncedVelocity = reader.ReadVector3();
        syncedRotation = reader.ReadQuaternion();
        sendTime = reader.ReadDouble();
        startTime = reader.ReadDouble();
        
        if (initialState)
        {
            timeDifference = sendTime - Network.time;
        }
    }


    [Command]
    private void CmdSyncRotation(Quaternion _rotation)
    {
        syncedRotation = _rotation;
    }

    [Command]
    private void CmdSyncMovement(Vector3 _position, Vector3 _velocity, Quaternion _rotation, double _time)
    {
        syncedPosition = _position;
        SetDirtyBit(1);
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
