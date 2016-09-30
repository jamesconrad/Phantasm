using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BehaviourTree : NetworkBehaviour {

    public float aggroRadius = 5;
    public float sightRange = 10;
    public float maxSight = 15;
    public float chargeSpeed = 1;
    public float transparencyScale = 1;
    private char state = 'I';
    private Vector3 lastKnown;
	// Use this for initialization
	void Start () {
        state = 'I';
        lastKnown = transform.position;
	}
	
    char State () {
        //state C is charging, the ai is aware of players current location
        //state W is walking, the ai knows where the player was last and is checking at that location
        //state I is idle, the ai has no information at all
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            return 'I';
        }
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Vector3 player = playerGO.GetComponent<Transform>().position;
        Vector3 me = transform.position;

        Vector3 dir = player - me;
        Vector3 dirN = dir.normalized;

        //Actual math
        if (dir.magnitude < aggroRadius) //Within sound aggro range, auto pull
        {
            lastKnown = player;
            return 'C';
        }
        else
        {
            RaycastHit hitInfo;
            //Physics.Raycast(me, dir.normalized, out hitInfo, maxSight);
            if (playerGO != null && Physics.Raycast(me, dir.normalized, out hitInfo, maxSight) && hitInfo.collider.gameObject == playerGO)
            {
                lastKnown = player;
                return 'C';
            }
            else if ((me - (lastKnown - dirN)).magnitude < 4.5)
                //constant (4.5) is based a range from point to model,
                //designed to not be exact but close (with original model this was roughly 1.2 units away from origin)
            {
                return 'I';
            }
            else
                return 'W';
        }
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
        
        if (state == 'I')
        {
            //print("Idle");
            agent.destination = me;
            state = State();
        }
        else if (state == 'W')
        {
            //print("Walking");
            agent.destination = lastKnown;
            state = State();
        }
        else if (state == 'C')
        {
            //print("Charge");
            agent.destination = lastKnown;// swap to player location for perma charge
            state = State();// remove for perma charge
        }

        
	}
}
