using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDeleteAfterTime : MonoBehaviour
{
    float time = 0.0f;
    public float LifeTime = 5.0f;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		time += Time.deltaTime;
        if(time > LifeTime)
        {
            Destroy(this.gameObject);
        }
	}
}
