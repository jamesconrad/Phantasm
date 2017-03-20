using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PhantomAnimParam : MonoBehaviour {

    NavMeshAgent agent;
    Animator anim;
    Vector3 last;

	// Use this for initialization
	void Start () {
        agent = transform.parent.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 d = last - transform.position;
        anim.SetFloat("mov", agent.velocity.magnitude);
	}
}
