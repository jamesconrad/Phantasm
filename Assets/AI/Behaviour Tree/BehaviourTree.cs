using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BehaviourTree : NetworkBehaviour {

    //Implementation Status:
    //Listener : Current Scope Restriction
    //Basic : Missing Attack Information
    //Hiding : Missing Attack Information
    public enum AI_TYPE {  Listener, Basic, Hiding };

    //Implementation Status
    //Complete
    //Complete
    //Complete
    //Not Started
    //Not Started
    //Implementating
    //Implementating
    public struct AISettings
    {
        public float maxSight; //how far the ai can see, for raycasting line of sight
        public float patience; //how long the ai will wait at the lastknown position before returning back to 
        public float arrivalDistance; //accuracy for arrival points, if within x units it considers it as arrived
        public bool chargeAttack; //wether the ai can do a pounce like attack or melee
        public bool alternatesVisibility; //wether the ai alternates between agent vision to hacker vision and back
        public Patrol patrolPath; //path the ai will idly patrol around
        public AI_TYPE type; //the class of ai
    }

    public AISettings aiSettings;
 
    private Vector3 lastKnown;

    static GameObject playerGO;
    private UnityEngine.AI.NavMeshAgent agent;

    public enum AI_STATE
    {
        Attack = 0,
        Chase = 1,
        Wait = 2,
        Patrol = 3,
        ReturnToPatrol = 4
    }

    private AI_STATE aistate = AI_STATE.Wait;

    // Use this for initialization
    void Start () {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        //BehavTree
        if (!isServer)
        {
            return;
        }

        
        
	}
    

    public class AIState
    {
        public AIState(ref AISettings settings, ref Transform transform, ref UnityEngine.AI.NavMeshAgent navAgent)
        { aiS = settings; t = transform; nav = navAgent; }

        protected Vector3 lastKnown;
        protected AISettings aiS;
        protected Transform t;
        protected UnityEngine.AI.NavMeshAgent nav;

        public virtual void update() { }
        public virtual AI_STATE nextstate() { return AI_STATE.Wait; }

        public bool hasLineOfSight()
        {
            Vector3 dir = playerGO.transform.position - t.position;
            RaycastHit hitInfo;
            Physics.Raycast(t.position, dir.normalized, out hitInfo, aiS.maxSight);
            if (playerGO != null && Physics.Raycast(t.position, dir.normalized, out hitInfo, aiS.maxSight) && hitInfo.collider.gameObject == playerGO)
            {
                lastKnown = playerGO.transform.position;
                return true;
            }
            return false;
        }
    }
    
    //Missing Patrol Logic
    public class AIPatrol : AIState
    {
        public AIPatrol(ref AISettings settings, ref Transform transform, ref UnityEngine.AI.NavMeshAgent navAgent)
            : base(ref settings, ref transform, ref navAgent)
        { aiS = settings; t = transform; nav = navAgent; }
        public override void update()
        {
            //follow path
        }
        public override AI_STATE nextstate()
        {
            if (hasLineOfSight())
                return AI_STATE.Chase;
            return AI_STATE.Patrol;
        }
    };
    
    //Missing Attack State Transition
    public class AIChase : AIState
    {
        public AIChase(ref AISettings settings, ref Transform transform, ref UnityEngine.AI.NavMeshAgent navAgent)
            : base(ref settings, ref transform, ref navAgent)
        { aiS = settings; t = transform; nav = navAgent; }

        private bool arrived = false;
        private float waitingTime = 0;

        public override void update()
        {
            //chase last known
            nav.destination = lastKnown;
        }
        public override AI_STATE nextstate()
        {
            //if near/at last known and no line of sight for x seconds
            //swap to retrun
            if ((t.position - lastKnown).magnitude <= aiS.arrivalDistance)
                arrived = true;
            if(arrived)
            {
                if (hasLineOfSight())
                    return AI_STATE.Chase;
                else
                    waitingTime += Time.deltaTime;

                if (waitingTime >= aiS.patience)
                    return aiS.type == AI_TYPE.Basic ? AI_STATE.ReturnToPatrol : AI_STATE.Wait;

            }
            return AI_STATE.Chase;
        }
    };
    
    //Complete
    public class AIReturnToPatrol : AIState
    {
        public AIReturnToPatrol(ref AISettings settings, ref Transform transform, ref UnityEngine.AI.NavMeshAgent navAgent)
            : base(ref settings, ref transform, ref navAgent)
        { aiS = settings; t = transform; nav = navAgent; }
        public override void update()
        {
            //pathfind to patrol path
            nav.destination = aiS.patrolPath.transform.GetChild(0).transform.position;
        }
        public override AI_STATE nextstate()
        {
            //swap to patrol on arrival
            //swap to chase on line of sight
            if (hasLineOfSight())
                return AI_STATE.Chase;
            if ((t.position - aiS.patrolPath.transform.GetChild(0).transform.position).magnitude <= aiS.arrivalDistance)
                return AI_STATE.Patrol;
            return AI_STATE.ReturnToPatrol;
        }
    };
    
    //Complete
    public class AIWait : AIState
    {
        public AIWait(ref AISettings settings, ref Transform transform, ref UnityEngine.AI.NavMeshAgent navAgent)
            : base(ref settings, ref transform, ref navAgent)
        { aiS = settings; t = transform; nav = navAgent; }
        public override void update()
        {
            //wait for line of sight or range
        }
        public override AI_STATE nextstate()
        {
            //swap to chase on line of sight or range
            if (hasLineOfSight())
                return AI_STATE.Chase;
            return AI_STATE.Wait;
        }
    };
    
    //Missing All Logic
    public class AIAttack : AIState
    {
        public AIAttack(ref AISettings settings, ref Transform transform, ref UnityEngine.AI.NavMeshAgent navAgent)
            : base(ref settings, ref transform, ref navAgent)
        { aiS = settings; t = transform; nav = navAgent; }
        public override void update()
        {
            //attack or charge
        }
        public override AI_STATE nextstate()
        {
            //reset to chase

            return AI_STATE.Attack;
        }
    };
}
