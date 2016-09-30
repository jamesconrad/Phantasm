using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkedMovement : NetworkBehaviour
{
    public float maxDistance;

    [SyncVar]
    private double sendTime2;

    [SyncVar]
    private Vector3 syncedPosition;

    private Vector3 simulatedPosition;

    [SyncVar]
    private Quaternion syncedRotation;

    [SyncVar]
    private Vector3 syncedVelocity;

    public bool objectIsClient = true;

    //private Rigidbody playerRigidBody;


    // Use this for initialization
    void Start()
    {
        sendTime2 = Network.time;
    }

    // Called on clients for player objects for the local client (only)
    public override void OnStartLocalPlayer()
    {
        CmdSyncMovement(transform.position, GetComponent<Rigidbody>().velocity, transform.rotation, Network.time);
    }

    // Update is called once per frame
    void Update()
    {
        simulatedPosition = syncedPosition + syncedVelocity * (float)(Network.time - sendTime2);
        
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

    [Command]
    private void CmdSyncRotation(Quaternion _rotation)
    {
        syncedRotation = _rotation;
    }

    [Command]
    private void CmdSyncMovement(Vector3 _position, Vector3 _velocity, Quaternion _rotation, double _time)
    {
        syncedPosition = _position;
        syncedVelocity = _velocity;
        //syncedRotation = _rotation;
        sendTime2 = _time;
    }
}
