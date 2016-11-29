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
        Idle = 2,
        Patrol = 3
    }
    private AIState AiState = AIState.Idle;
    // Use this for initialization
    void Start () {
        AiState = AIState.Charge;
        lastKnown = transform.position;
	}
	
    void State () {
        //AiState C is charging, the ai is aware of players current location
        //AiState W is walking, the ai knows where the player was last and is checking at that location
        //AiState P is patrol, randomly navigating the scene
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
        //Debug.DrawRay(me, dirN * maxSight, Color.gray);
        //Debug.DrawRay(me, dirN * sightRange, Color.green);
        //Debug.DrawRay(me, dirN * aggroRadius, Color.red);
        
        if (AiState == AIState.Idle)
        {
            //print("Idle");
            agent.destination = me;
            //AiState = State();
            AiState = AIState.Patrol;
        }
        else if (AiState == AIState.Walking)
        {
            //print("Walking");
            //lastKnown = player;
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
        else if (AiState == AIState.Patrol)
        {
            if ((agent.destination - me).magnitude < 0.25)
            {
                bool ret = false;
                for (int i = 0; i < 10 && !ret; i++)
                {
                    Vector3 random = me + Random.insideUnitSphere * 2;
                    random.y = 0;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(random, out hit, 2, NavMesh.AllAreas))
                    {
                        agent.destination = random;
                        ret = true;
                    }
                }
            }
        }
	}

    public void SetState(int _state)
    {
        AiState = (AIState)_state;
    }
}
