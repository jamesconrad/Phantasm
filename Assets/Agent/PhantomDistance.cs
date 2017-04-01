using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomDistance : MonoBehaviour 
{
    GameObject AgentObject;
    GameObject[] PhantomObject;


	// Use this for initialization
	void Start () 
	{
		Search();
	}
	
	void Search()
	{
		AgentObject = GameObject.FindGameObjectWithTag("Player");
        PhantomObject = GameObject.FindGameObjectsWithTag("Enemy");
	}
	
	public GameObject AgentReference()
	{
		return AgentObject;
	}

	public GameObject[] PhantomReference()
	{
		return PhantomObject;
	}

	const float timeToWaitTillSearch = 10.0f;
    float timeSinceLastSearch = timeToWaitTillSearch;
	// Update is called once per frame
	void FixedUpdate () 
	{
		timeSinceLastSearch += Time.fixedDeltaTime;
        if(timeSinceLastSearch > timeToWaitTillSearch)
        {
            Search();
            timeSinceLastSearch = 0.0f;
        }
	}
}
