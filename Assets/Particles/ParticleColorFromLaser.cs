using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorFromLaser : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        var part = GetComponent<ParticleSystem>().main;
        part.startColor = GunLaserScript.colorHit;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
