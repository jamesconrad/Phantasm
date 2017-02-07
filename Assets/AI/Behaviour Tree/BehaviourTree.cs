using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BehaviourTree : NetworkBehaviour {
    public enum AI_TYPE {  Listener, Basic, Hiding };


    public float aggroRadius = 5;
    public float sightRange = 10;
    public float maxSight = 15;
    public float chargeSpeed = 1;
    public float transparencyScale = 1;
    public bool chargeAttack = false;
    public bool alternatesVisibility = false;
    public Patrol patrolPath;

    public AI_TYPE type = AI_TYPE.Basic;
 
    private Vector3 lastKnown;

    GameObject playerGO;

    public enum AI_STATE
    {
        Charge = 0,
        Walking = 1,
        Idle = 2,
        Patrol = 3
    }
    private AI_STATE aistate = AI_STATE.Idle;
    // Use this for initialization
    void Start () {
        aistate = AI_STATE.Charge;
        lastKnown = transform.position;
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
	
    void State () {
        //AI_STATE C is charging, the ai is aware of players current location
        //AI_STATE W is walking, the ai knows where the player was last and is checking at that location
        //AI_STATE P is patrol, randomly navigating the scene
        //AI_STATE I is idle, the ai has no information at all
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            return;// AI_STATE.I;
        }
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
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Vector3 player = playerGO.GetComponent<Transform>().position;
        Vector3 me = transform.position;

        Vector3 dir = player - me;
        Vector3 dirN = dir.normalized;

        //draw auto aggro
        //Debug.DrawRay(me, dir, Color.red);
        //Debug.DrawRay(me, dirN * maxSight, Color.gray);
        //Debug.DrawRay(me, dirN * sightRange, Color.green);
        //Debug.DrawRay(me, dirN * aggroRadius, Color.red);
        
        if (aistate == AI_STATE.Idle)
        {
            //print("Idle");
            agent.destination = me;
            //AI_STATE = State();
            aistate = AI_STATE.Patrol;
        }
        else if (aistate == AI_STATE.Walking)
        {
            //print("Walking");
            //lastKnown = player;
            agent.destination = lastKnown;
            //AI_STATE = State();
        }
        else if (aistate == AI_STATE.Charge)
        {
            //print("Charge");
            lastKnown = player;
            agent.destination = lastKnown;// swap to player location for perma charge
            //AI_STATE = State();// remove for perma charge
        }
        else if (aistate == AI_STATE.Patrol)
        {
            if ((agent.destination - me).magnitude < 0.25)
            {
                bool ret = false;
                for (int i = 0; i < 10 && !ret; i++)
                {
                    Vector3 random = me + Random.insideUnitSphere * 2;
                    random.y = 0;
                    UnityEngine.AI.NavMeshHit hit;
                    if (UnityEngine.AI.NavMesh.SamplePosition(random, out hit, 2, UnityEngine.AI.NavMesh.AllAreas))
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
        aistate = (AI_STATE)_state;
    }

    public class AIState
    {
        private Vector3 lastKnown;
        public virtual void update() { }
        public virtual void nextstate() { }
        public Vector3 getPlayerPosition()
        {
            Vector3 p = lastKnown;

            return p;
        }
        public bool hasLineOfSight()
        {

            return false;
        }
    }
    public class AIPatrol : AIState
    {
        public void update()
        {
            //follow path
        }
        public  void nextstate()
        {
            //swap to chase if line of sight
        }
    };
    public class AIChase : AIState
    {

        public void update()
        {
            //chase last known
        }
        public void nextstate()
        {
            //if near/at last known and no line of sight for x seconds
            //swap to retrun
        }
    };
    public class AIReturnToPatrol : AIState
    {

        public void update()
        {
            //pathfind to patrol path
        }
        public void nextstate()
        {
            //swap to patrol on arrival
            //swap to chase on line of sight
        }
    };
    public class AIWait : AIState
    {

        public void update()
        {
            //wait for line of sight or range
        }
        public void nextstate()
        {
            //swap to chase on line of sight or range
        }
    };
    public class AIAttack : AIState
    {

        public void update()
        {
            //attack or charge
        }
        public void nextstate()
        {
            //reset to chase
        }
    };
}
