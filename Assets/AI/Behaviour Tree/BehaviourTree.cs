using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BehaviourTree : NetworkBehaviour {

    //Implementation Status:
    //Listener : Current Scope Restriction
    //Basic : Missing Attack Information
    //Hiding : Missing Attack Information
    public enum AI_TYPE {  Listener, Basic, Hiding, Wallhack };

    public float maxSight = 50; //how far the ai can see, for raycasting line of sight
    public float patience = 3; //how long the ai will wait at the lastknown position before returning back to 
    public float arrivalDistance = 0.1f; //accuracy for arrival points, if within x units it considers it as arrived
    public bool chargeAttack = false; //wether the ai can do a pounce like attack or melee
    public bool alternatesVisibility = false; //wether the ai alternates between agent vision to hacker vision and back
    public Patrol patrolPath; //path the ai will idly patrol around
    public AI_TYPE type = AI_TYPE.Wallhack; //the class of ai
    public bool triggered = true; //for the inactive/whatever bonnyman/jacob wanted

    public Vector3 lk;
    public RaycastHit hi;

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
        public float maxSight;
        public float patience;
        public float arrivalDistance;
        public bool chargeAttack;
        public bool alternatesVisibility;
        public Patrol patrolPath;
        public AI_TYPE type;
        public bool triggered;
    }

    public AISettings aiSettings;
    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 lastKnown;

    private AIState ai;

    //static GameObject playerGO;

    public enum AI_STATE
    {
        Attack = 0,
        Chase = 1,
        Wait = 2,
        Patrol = 3,
        ReturnToPatrol = 4
    }

    private AI_STATE aistate = AI_STATE.Wait;
    
    void Start ()
    {
        aiSettings.alternatesVisibility = alternatesVisibility;
        aiSettings.arrivalDistance = arrivalDistance;
        aiSettings.chargeAttack = chargeAttack;
        aiSettings.maxSight = maxSight;
        aiSettings.patience = patience;
        aiSettings.patrolPath = patrolPath;
        aiSettings.type = type;
        aiSettings.triggered = triggered;
        
        //GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        //
        //Debug.Log(playerGO.name);
        
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (aiSettings.type == AI_TYPE.Basic)
            ai = new AIPatrol(ref aiSettings, transform, ref agent);
        else if (aiSettings.type == AI_TYPE.Hiding)
            ai = new AIWait(ref aiSettings, transform, ref agent);
        else if (aiSettings.type == AI_TYPE.Wallhack)
            ai = new AIChase(ref aiSettings, transform, ref agent);
        else
            print("Unimplemented AI Type.");
        AIState.playerGO = GameObject.FindGameObjectWithTag("Player");
    }

    public void UpdateSettings(AISettings s)
    {
        aiSettings = s;

        if (aiSettings.type == AI_TYPE.Basic)
            ai = new AIPatrol(ref aiSettings, transform, ref agent);
        else if (aiSettings.type == AI_TYPE.Hiding)
            ai = new AIWait(ref aiSettings, transform, ref agent);
        else if (aiSettings.type == AI_TYPE.Wallhack)
            ai = new AIChase(ref aiSettings, transform, ref agent);
        else
            print("Unimplemented AI Type.");
    }

    public void RestartWithoutDefaultSettings()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (aiSettings.type == AI_TYPE.Basic)
            ai = new AIPatrol(ref aiSettings, transform, ref agent);
        else if (aiSettings.type == AI_TYPE.Hiding)
            ai = new AIWait(ref aiSettings, transform, ref agent);
        else if (aiSettings.type == AI_TYPE.Wallhack)
            ai = new AIChase(ref aiSettings, transform, ref agent);
        else
            print("Unimplemented AI Type.");
        AIState.playerGO = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update ()
    {
        //BehavTree
        if (!isServer)
        {
            return;
        }

        if (triggered)
        {
            ai.update();
            AI_STATE nextstate = ai.nextstate();
            if (aistate != nextstate)
            {
                if (nextstate == AI_STATE.Attack)
                    ai = new AIAttack(ref aiSettings, transform, ref agent);
                else if (nextstate == AI_STATE.Chase)
                    ai = new AIChase(ref aiSettings, transform, ref agent);
                else if (nextstate == AI_STATE.Wait)
                    ai = new AIWait(ref aiSettings, transform, ref agent);
                else if (nextstate == AI_STATE.Patrol)
                    ai = new AIPatrol(ref aiSettings, transform, ref agent);
                else if (nextstate == AI_STATE.ReturnToPatrol)
                    ai = new AIReturnToPatrol(ref aiSettings, transform, ref agent);
            }
            lk = ai.lastKnown;
            hi = ai.hinfo;
        }
	}
    
    //called on trigger through message, used if it needs to be activated or deactivated
    void Trigger()
    {
        triggered = !triggered;
        if (triggered)
            Start();
        else
            ai = new AIWait(ref aiSettings, transform, ref agent);
    }

    public class AIState
    {
        public AIState(ref AISettings settings, Transform transform, ref UnityEngine.AI.NavMeshAgent navAgent)
        { aiS = settings; t = transform; nav = navAgent; }

        public Vector3 lastKnown = new Vector3();
        protected AISettings aiS;
        protected Transform t;
        protected UnityEngine.AI.NavMeshAgent nav;
        public static GameObject playerGO;
        public RaycastHit hinfo;

        public virtual void update() { }
        public virtual AI_STATE nextstate() { return AI_STATE.Wait; }

        public bool hasLineOfSight()
        {
            Vector3 dir = playerGO.transform.position - t.position;
            RaycastHit hitInfo;
            Physics.Raycast(t.position, dir.normalized, out hitInfo, aiS.maxSight);
            hinfo = hitInfo;
            if (playerGO != null && Physics.Raycast(t.position, dir.normalized, out hitInfo, aiS.maxSight) && hitInfo.collider.gameObject == playerGO)
            {
                print("Saw player at: " + hitInfo.point);
                lastKnown = playerGO.transform.position;
                return true;
            }
            return false;
        }
    }
    
    //Complete
    public class AIPatrol : AIState
    {
        public AIPatrol(ref AISettings settings, Transform transform, ref UnityEngine.AI.NavMeshAgent navAgent)
            : base(ref settings, transform, ref navAgent)
        { aiS = settings; t = transform; nav = navAgent; }

        private bool haspoint = false;
        private Vector3 point;

        public override void update()
        {
            if (aiS.type == AI_TYPE.Wallhack)
            {
                lastKnown = playerGO.transform.position;
                nav.SetDestination(lastKnown);
                return;
            }

            //follow path
            if (!haspoint)
            {
                point = aiS.patrolPath.NextPoint(t.position);
                nav.SetDestination(point);
            }

            if ((t.position - point).magnitude >= aiS.arrivalDistance)
            {
                point = aiS.patrolPath.NextPoint(point);
                nav.SetDestination(point);
            }
        }
        public override AI_STATE nextstate()
        {
            if (aiS.type == AI_TYPE.Wallhack)
                return AI_STATE.Chase;

            if (hasLineOfSight())
                return AI_STATE.Chase;
            return AI_STATE.Patrol;
        }
    };
    
    //Missing Attack State Transition
    public class AIChase : AIState
    {
        public AIChase(ref AISettings settings, Transform transform, ref UnityEngine.AI.NavMeshAgent navAgent)
            : base(ref settings, transform, ref navAgent)
        { aiS = settings; t = transform; nav = navAgent; }

        private bool arrived = false;
        private float waitingTime = 0;

        public override void update()
        {
            //chase last known
            //Debug.Log(lastKnown);
            //Debug.Log(playerGO.transform.position);
           

            if (aiS.type == AI_TYPE.Wallhack)
                lastKnown = playerGO.transform.position;

            if (nav.destination != lastKnown)
                nav.SetDestination(lastKnown);
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
        public AIReturnToPatrol(ref AISettings settings, Transform transform, ref UnityEngine.AI.NavMeshAgent navAgent)
            : base(ref settings, transform, ref navAgent)
        { aiS = settings; t = transform; nav = navAgent; nav.SetDestination(aiS.patrolPath.NextPoint(t.position)); }
        public override void update()
        {
            //pathfind to patrol path
            //pathfind set on constructor
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
        public AIWait(ref AISettings settings, Transform transform, ref UnityEngine.AI.NavMeshAgent navAgent)
            : base(ref settings, transform, ref navAgent)
        { aiS = settings; t = transform; nav = navAgent; nav.SetDestination(t.position); lastKnown = t.position; }
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
        public AIAttack(ref AISettings settings, Transform transform, ref UnityEngine.AI.NavMeshAgent navAgent)
            : base(ref settings, transform, ref navAgent)
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
