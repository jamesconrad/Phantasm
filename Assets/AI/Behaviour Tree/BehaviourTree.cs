using UnityEngine;
using System.Collections;

public class BehaviourTree : MonoBehaviour {
    
    //Implementation Status:
    //Listener : Current Scope Restriction
    //Basic : Missing Attack Information
    //Hiding : Missing Attack Information
    public enum AI_TYPE {  Listener, Basic, Hiding, Wallhack };
    //
    //public float maxSight = 50; //how far the ai can see, for raycasting line of sight
    //public float patience = 3; //how long the ai will wait at the lastknown position before returning back to 
    //public float arrivalDistance = 0.1f; //accuracy for arrival points, if within x units it considers it as arrived
    //public bool chargeAttack = false; //wether the ai can do a pounce like attack or melee
    //public bool alternatesVisibility = false; //wether the ai alternates between agent vision to hacker vision and back
    //public Patrol patrolPath; //path the ai will idly patrol around
    //public AI_TYPE type = AI_TYPE.Wallhack; //the class of ai
    //public bool triggered = true; //for the inactive/whatever bonnyman/jacob wanted
    public AI_TYPE DebugaiT;
    public AI_STATE DebugaiS;
    public Vector3 lk;

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
        //aiSettings.alternatesVisibility = alternatesVisibility;
        //aiSettings.arrivalDistance = arrivalDistance;
        //aiSettings.chargeAttack = chargeAttack;
        //aiSettings.maxSight = maxSight;
        //aiSettings.patience = patience;
        //aiSettings.patrolPath = patrolPath;
        //aiSettings.type = type;
        //aiSettings.triggered = triggered;
        
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
        {
            print("Unimplemented AI Type. Killing Myself.\nI am most likley the phantom without a spawner parent, this is an issue. YOU SHOULD NOT BE SEEING THIS TEXT");
            Destroy(this);
        }
        AIState.playerGO = GameObject.FindGameObjectWithTag("Player");
    }

    public void RestartWithoutDefaultSettings(AISettings s)
    {
        aiSettings = s;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (aiSettings.type == AI_TYPE.Basic)
            ai = new AIPatrol(ref aiSettings, transform, ref agent);
        else if (aiSettings.type == AI_TYPE.Hiding)
            ai = new AIWait(ref aiSettings, transform, ref agent);
        else if (aiSettings.type == AI_TYPE.Wallhack)
            ai = new AIChase(ref aiSettings, transform, ref agent);
        else
        {
            print("Unimplemented AI Type. Killing Myself.\nI am most likley the phantom without a spawner parent, this is an issue. YOU SHOULD NOT BE SEEING THIS TEXT");
            Destroy(gameObject);
        }
        AIState.playerGO = GameObject.FindGameObjectWithTag("Player");
    }

    public AISettings GetSettings()
    {
        return aiSettings;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (aiSettings.triggered)
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
                aistate = nextstate;
                ai.lastKnown = lk;
            }
            lk = ai.lastKnown;
            DebugaiS = aistate;
            DebugaiT = aiSettings.type;
            //Debug.DrawLine(lk - Vector3.right * 0.5f, lk + Vector3.right * 0.5f, Color.red);
            //Debug.DrawLine(lk - Vector3.forward * 0.5f, lk + Vector3.forward * 0.5f, Color.blue);
            //Debug.DrawLine(lk - Vector3.up * 0.5f, lk + Vector3.up * 0.5f, Color.green);
        }
	}
    
    //called on trigger through message, used if it needs to be activated or deactivated
    void Trigger()
    {
        aiSettings.triggered = !aiSettings.triggered;
        if (aiSettings.triggered)
            RestartWithoutDefaultSettings(aiSettings);
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
            if(playerGO != null)
            {
                Vector3 dir = playerGO.transform.position - t.position;
                RaycastHit hitInfo;
                bool hit = Physics.Raycast(t.position + new Vector3(0, 0.75f, 0), dir.normalized, out hitInfo, aiS.maxSight);
                if (playerGO != null && hit && hitInfo.transform.tag == "Player")
                {
                    //Debug.DrawLine(t.position + new Vector3(0, 0.75f, 0), t.position + dir * aiS.maxSight, Color.cyan);
                    lastKnown = hitInfo.point;
                    return true;
                }
            }
            return false;
        }
    }
    
    //Complete
    public class AIPatrol : AIState
    {
        public AIPatrol(ref AISettings settings, Transform transform, ref UnityEngine.AI.NavMeshAgent navAgent)
            : base(ref settings, transform, ref navAgent)
        { aiS = settings; t = transform; nav = navAgent; lastIndex = 0; }

        private bool haspoint = false;
        private Vector3 point;
        private float lastIndex;

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
                point = aiS.patrolPath.NextPoint(t.position, aiS.arrivalDistance, lastIndex);
                nav.SetDestination(point);
                haspoint = true;
            }

            if ((t.position - point).magnitude <= aiS.arrivalDistance)
            {
                Vector4 ret = aiS.patrolPath.NextPoint(point, aiS.arrivalDistance, lastIndex);
                point = new Vector3(ret.x, ret.y, ret.z);
                lastIndex = ret.w;
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
        private bool haslos = false;
        private float waitingTime = 0;

        public override void update()
        {
            if(playerGO != null)
            {
                //chase last known
                //Debug.Log(lastKnown);
                //Debug.Log(playerGO.transform.position);
                if (hasLineOfSight())
                    haslos = true;
                
                if (aiS.type == AI_TYPE.Wallhack)
                    lastKnown = playerGO.transform.position;
                
                if (nav.destination != lastKnown)
                    nav.SetDestination(lastKnown);
            }
        }
        public override AI_STATE nextstate()
        {
            //if near/at last known and no line of sight for x seconds
            //swap to retrun
            if ((t.position - lastKnown).magnitude <= aiS.arrivalDistance)
                arrived = true;
            if(arrived)
            {
                if (haslos)
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
        { aiS = settings; t = transform; nav = navAgent; nav.SetDestination(aiS.patrolPath.NextPoint(t.position, aiS.arrivalDistance, 0)); }
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
        {
            aiS = settings;
            t = transform;
            nav = navAgent;
            nav.SetDestination(t.position);
            lastKnown = t.position;
        }
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
