using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimation : MonoBehaviour {

	public Animator animator;

	Vector3 previousPosition;
	Vector3 differenceVector;

	public float speedMultiplied = 10.0f;

	// Use this for initialization
	void Start () {
		differenceVector = new Vector3();
		differenceVector.y = 0.0f;
		previousPosition = new Vector3(transform.position.x, transform.position.y, transform.position.y);
		previousPosition = transform.worldToLocalMatrix * previousPosition;
	}
	
	// Update is called once per frame
	void Update () 
	{
		differenceVector.x = transform.position.x - previousPosition.x;
		differenceVector.y = 0.0f;
		differenceVector.z = transform.position.z - previousPosition.z;
		differenceVector = transform.worldToLocalMatrix * differenceVector;
		animator.SetFloat("movX", differenceVector.z);
		animator.SetFloat("movY", differenceVector.x);
		animator.SetFloat("velocity", Mathf.Max(Mathf.Abs(differenceVector.x), Mathf.Abs(differenceVector.z)) * speedMultiplied);

		previousPosition.x = transform.position.x;
		previousPosition.y = transform.position.y;
		previousPosition.z = transform.position.z;		
	}
}
