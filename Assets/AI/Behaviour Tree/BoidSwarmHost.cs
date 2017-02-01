using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSwarmHost : MonoBehaviour {

    public bool drawDebugVectors = false;
    public bool resetVariables = true;
    public float swarmNumber = 10;
    public float maxVelocity = 1;
    public float acceleration = 0.25f;
    public float turnDeceleration = 0.4f;
    public float impactWindow = 100.0f;

    public Vector3 swarmAverage;
    public Vector3 swarmAveragePos;

    public Camera focusCam;
    public float camFollowMax;
    public float camFollowMin;


	// Use this for initialization
	void Start () {
		//spawn in boid agents
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (resetVariables)
        {
            resetVariables = false;
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).GetComponent<BoidSwarmAgent>().ResetVariables(maxVelocity, acceleration, turnDeceleration, impactWindow);
        }

        //calc swarm average
        swarmAverage = new Vector3(0, 0, 0);
        for (int i = 0; i < transform.childCount; i++)
        {
            swarmAverage += transform.GetChild(i).GetComponent<BoidSwarmAgent>().transform.forward;
            swarmAveragePos += transform.GetChild(i).GetComponent<BoidSwarmAgent>().transform.position;
        }
        swarmAverage /= transform.childCount;
        swarmAveragePos /= transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<BoidSwarmAgent>().swarmAverage = swarmAverage;

        float boidDist = Mathf.Abs(focusCam.transform.position.magnitude - swarmAveragePos.magnitude);

        //print(boidDist);

        if (boidDist < camFollowMin)
            focusCam.transform.position -= focusCam.transform.forward * boidDist * Time.deltaTime;
        else if (boidDist > camFollowMax)
            focusCam.transform.position += focusCam.transform.forward * boidDist * Time.deltaTime;
        focusCam.transform.LookAt(swarmAverage);
    }
}
