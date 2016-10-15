using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BehaviourTree : NetworkBehaviour {
    public float aggroRadius = 5;
    public float sightRange = 10;
    public float maxSight = 15;
    public float chargeSpeed = 1;
    public float transparencyScale = 1;
 
    private Vector3 lastKnown;

    public enum AIState
    {
        Charge = 0,
        Walking = 1,
        Idle = 2
    }
    private AIState AiState = AIState.Idle;
    // Use this for initialization
    void Start () {
        AiState = AIState.Idle;
        lastKnown = transform.position;
	}
	
    void State () {
        //AiState C is charging, the ai is aware of players current location
        //AiState W is walking, the ai knows where the player was last and is checking at that location
        //AiState I is idle, the ai has no information at all
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            return;// AIState.I;
        }
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Vector3 player = playerGO.GetComponent<Transform>().position;
        Vector3 me = transform.position;

        Vector3 dir = player - me;
        Vector3 dirN = dir.normalized;

        //Actual math
        //if (dir.magnitude < aggroRadius) //Within sound aggro range, auto pull
        //{
        //    lastKnown = player;
        //    return AIState.Chase;
        //}
        //else
        //{
        //    RaycastHit hitInfo;
        //    //Physics.Raycast(me, dir.normalized, out hitInfo, maxSight);
        //    if (playerGO != null && Physics.Raycast(me, dir.normalized, out hitInfo, maxSight) && hitInfo.collider.gameObject == playerGO)
        //    {
        //        lastKnown = player;
        //        return AIState.Chase;
        //    }
        //    else if ((me - (lastKnown - dirN)).magnitude < 4.5)
        //        //constant (4.5) is based a range from point to model,
        //        //designed to not be exact but close (with original model this was roughly 1.2 units away from origin)
        //    {
        //        return AIState.I;
        //    }
        //    else
        //        return AIState.Wait;
        //}
        //we done
    }

	// Update is called once per frame
	void Update () {
        //BehavTree
        //Search/Attack
        if (!isServer)
        {
            return;
        }

        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            return;
        }
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Vector3 player = playerGO.GetComponent<Transform>().position;
        Vector3 me = transform.position;

        Vector3 dir = player - me;
        Vector3 dirN = dir.normalized;

        //draw auto aggro
        //Debug.DrawRay(me, dir, Color.red);
        Debug.DrawRay(me, dirN * maxSight, Color.gray);
        Debug.DrawRay(me, dirN * sightRange, Color.green);
        Debug.DrawRay(me, dirN * aggroRadius, Color.red);
        
        if (AiState == AIState.Idle)
        {
            //print("Idle");
            agent.destination = me;
            //AiState = State();
        }
        else if (AiState == AIState.Walking)
        {
            //print("Walking");
            lastKnown = player;
            agent.destination = lastKnown;
            //AiState = State();
        }
        else if (AiState == AIState.Charge)
        {
            //print("Charge");
            lastKnown = player;
            agent.destination = lastKnown;// swap to player location for perma charge
            //AiState = State();// remove for perma charge
        }

        
	}

    public void SetState(int _state)
    {
        AiState = (AIState)_state;
    }
}
