using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastReverbSound : MonoBehaviour 
{
	AudioListener listener;
	AudioSource source;
	AudioLowPassFilter lowpass;


	
    RaycastHit rayHit;
    QueryTriggerInteraction hitTriggers;
    public LayerMask whatToCollideWith;
    float distanceMax;
    float distance = 100.0f;


	// Use this for initialization
	void Start () 
	{
		listener = FindObjectOfType<AudioListener>();
		source = GetComponent<AudioSource>();
		lowpass = GetComponent<AudioLowPassFilter>();
		distanceMax = source.maxDistance;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		Vector3 direction = listener.transform.position - source.transform.position;
		if(Vector3.Distance(listener.transform.position, source.transform.position) <= source.maxDistance)
		{
			Ray ray = new Ray(source.transform.position, direction.normalized);
			bool hit = Physics.Raycast(ray, out rayHit, source.maxDistance, whatToCollideWith, hitTriggers);
			if (hit && rayHit.transform.tag == "Player")
			{
				lowpass.cutoffFrequency = Mathf.LerpUnclamped(lowpass.cutoffFrequency, 22000.0f, 0.05f);
			    //distance = Vector3.Distance(ray.origin, rayHit.point);			
			}
			else
			{
				lowpass.cutoffFrequency = Mathf.LerpUnclamped(lowpass.cutoffFrequency, 500.0f, 0.05f);
			}
		}
	}
}
