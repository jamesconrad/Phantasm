using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeleteOnDeath : MonoBehaviour
{
    ParticleSystem particle;

	// Use this for initialization
	void Start ()
    {
        particle = this.GetComponent<ParticleSystem>();
        if(particle == null)
        {
            Destroy(this);
        }
        particle.Play();

    }
	
	// Update is called once per frame
	void Update ()
    {
		if(!particle.isPlaying)
        {
            Destroy(this);
        }
	}
}
