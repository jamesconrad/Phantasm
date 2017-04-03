using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimation : MonoBehaviour {

	Animator animator;

	Vector3 previousPosition;

	// Use this for initialization
	void Start () {
		previousPosition = new Vector3(0.0f, 0.0f, 0.0f);
		previousPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		animator.SetFloat("movX", previousPosition.x - transform.position.x);
		animator.SetFloat("movY", previousPosition.y - transform.position.y);
	}
}
