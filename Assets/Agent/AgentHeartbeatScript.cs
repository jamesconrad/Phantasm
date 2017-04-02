using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentHeartbeatScript : MonoBehaviour 
{
	GameObject AgentObject;
    GameObject[] PhantomObject;
    PhantomDistance distanceManager;
	Heartbeat heartbeat;

	public float minDistance = 4.0f;
    public float maxDistance = 8.0f;
	public Vector3 distanceBias = new Vector3(1.0f, 1.0f, 1.0f);
	// Use this for initialization
	void Start () 
	{
		heartbeat = GetComponent<Heartbeat>();
		distanceManager = FindObjectOfType<PhantomDistance>();
        if(distanceManager != null)
        {
            AgentObject = distanceManager.AgentReference();
            PhantomObject = distanceManager.PhantomReference();
        }
        else
        {
            Debug.LogWarning("Could not find Distance Manager!");
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float closestPhantom = 100000.0f;// = Vector3.Distance(AgentObject.transform.position, PhantomObject[0].transform.position);
    	for (int i = 0; i < PhantomObject.Length; ++i)
    	{
    	    if(PhantomObject[i].activeSelf)
    	        closestPhantom = Mathf.Min(closestPhantom, Vector3.Distance(AgentObject.transform.position, Vector3.Scale(PhantomObject[i].transform.position, distanceBias)));
    	}
		float interp = Mathf.InverseLerp(minDistance, maxDistance, closestPhantom);
		heartbeat.beatIntensity = 1.0f - Mathf.Clamp(interp, 0.0f, 1.0f);
		heartbeat.beatActive = (closestPhantom < maxDistance);
	}
}
