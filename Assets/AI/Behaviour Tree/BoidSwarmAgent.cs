using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSwarmAgent : MonoBehaviour {

    public bool drawDebugVectors = false;
    public Vector3 swarmAverage = new Vector3(0,0,0);


    public float maxVelocity;
    public float acceleration;
    public float turnDeceleration;
    public float velocity = 1;
    public float impactWindow;
    public float crazyness;

    private uint numResolves = 0;
    private Vector3 unAveragedResolveDir;
    private float prioTimeToImpact = 0;
    private Vector3 prioResolveDir = Vector3.zero;

    private Vector3 steer = Vector3.zero;
    
	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
	    
	// Update is called once per frame
	void Update ()
    {
        //update velocity and such
        velocity += acceleration * Time.deltaTime;
        if (velocity > maxVelocity)
            velocity = maxVelocity;
        transform.position += transform.forward * Time.deltaTime * velocity;
        //flock navigate



        //steering and random correction
        //steer vector will be resolved for roll rotations
        if (Random.Range(0.0f,10.0f) > 9.9f)
        {
            steer.x += Random.Range(-0.1f, 0.1f);
            steer.y += Random.Range(-0.1f, 0.1f);
            steer.z += Random.Range(-0.1f, 0.1f);
        }



        //collision adapt
        if (numResolves > 0)
        {
            //resolve time
            float dtRangeShifted = Time.deltaTime * prioTimeToImpact;
            float directionShift = Mathf.Lerp(prioTimeToImpact, 0, dtRangeShifted) / prioTimeToImpact * 0.1f;
            Vector3 target = Vector3.Lerp(transform.forward, prioResolveDir, directionShift);
            transform.LookAt(transform.position + target);
            velocity -= turnDeceleration * dtRangeShifted;
        }
        else
        {
            //we are allowed to steer now
            transform.LookAt(transform.position + Vector3.Lerp(transform.forward, steer, Time.deltaTime));
        }


        if (drawDebugVectors)
            DrawDebugVectors();

        
        numResolves = 0;
        prioTimeToImpact = 0;
        unAveragedResolveDir = new Vector3(0, 0, 0);
        prioResolveDir = Vector3.zero;
    }

    void DrawDebugVectors()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * velocity, Color.blue);
        Debug.DrawLine(transform.position, transform.position + transform.right, Color.red);
        Debug.DrawLine(transform.position, transform.position + transform.up, Color.green);
        //Debug.DrawLine(transform.position, transform.position + swarmAverage, Color.cyan);
        Debug.DrawLine(transform.position, transform.position + prioResolveDir, Color.white);
        Debug.DrawLine(transform.position, transform.position + steer * velocity, Color.cyan);
    }

    void AddCollisionResolve(Vector3 target, float timeToImpact)
    {
        numResolves++;
        unAveragedResolveDir += target;
        if (timeToImpact > prioTimeToImpact)
        {
            prioTimeToImpact = timeToImpact;
            prioResolveDir = target;
        }
    }
    
    void OnCollisionStay(Collision collision)
    {
        //Check collider and start rotating each by half the required rotation over the estimated time till collision
        foreach (ContactPoint contact in collision.contacts)
        {
            GameObject hitObject = collision.gameObject;
            BoidSwarmAgent hitAgent = hitObject.GetComponent<BoidSwarmAgent>();
            if (hitAgent == null || hitObject == gameObject)
                continue;
            CollisionType ourType = CheckType(contact.thisCollider.GetType());
            CollisionType theirType = CheckType(contact.thisCollider.GetType());

            //lets start with just predictive analysis and run tests
            if (ourType == CollisionType.BOX && theirType == CollisionType.BOX)
            {
                Vector3 averageNormal = (hitObject.transform.forward + transform.forward) / 2;

                if (averageNormal == transform.forward)
                {
                    averageNormal.x = averageNormal.x + Random.Range(-0.25f, 0.25f);
                    averageNormal.y = averageNormal.y + Random.Range(-0.25f, 0.25f);
                    averageNormal.z = averageNormal.z + Random.Range(-0.25f, 0.25f);
                }
                //estimate time till impact
                //float theirImpactDist = (contact.point - hitObject.transform.position).magnitude;
                float ourImpactDist = (contact.point - transform.position).magnitude;

                //check for timing window
                //float theirTimeTillImpact = theirImpactDist / hitAgent.velocity;
                float ourTimeTillImpact = ourImpactDist / velocity;
                
                //if (Mathf.Abs(theirImpactDist - ourTimeTillImpact) > impactWindow)
                //    continue;

                AddCollisionResolve(averageNormal, ourTimeTillImpact);
            }
            //check and print a failure if sphere v sphere collision happened
            else if (ourType == CollisionType.SPHERE && theirType == CollisionType.SPHERE)
            {
                //unessicary atm, might be required for keeping the flock more spaced out   
            }
            else
            {
                //terrain collision
                //get the normal then set the correction vector accordingly
                Vector3 correction = contact.normal;

            }
        }
    }

    private enum CollisionType { BOX,SPHERE,UNKNOWN };
    private CollisionType CheckType(System.Type collider)
    {
        if (collider == typeof(SphereCollider))
            return CollisionType.SPHERE;
        else if (collider == typeof(BoxCollider))
            return CollisionType.BOX;
        else
            return CollisionType.UNKNOWN;
    }

    public void ResetVariables(float mV, float a, float ta, float iw)
    {
        maxVelocity = mV;
        acceleration = a;
        turnDeceleration = ta;
        impactWindow = iw;
    }
}
