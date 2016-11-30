using System.Collections;
using UnityEngine;

public class HackerCameraRotate : MonoBehaviour
{
	float t = 0;
	public float tSpeed = 0.1f;
	private Quaternion startRotation;

	//public float lengthPause = 4.0f;
	//float tPause;

	public Vector3 rotationBegin = new Vector3(0.0f, 10.0f, 0.0f);
	public Vector3 rotationEnd = new Vector3(0.0f, -10.0f, 0.0f);

	

	Quaternion rotationBeginQ;
	Quaternion rotationEndQ;

	// Use this for initialization
	void Start () 
	{
		startRotation = this.GetComponent<Camera>().transform.rotation;
		rotationBeginQ = Quaternion.Euler(rotationBegin);
		rotationEndQ = Quaternion.Euler(rotationEnd);

		t = Random.Range(0.0f, 2.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		t = (float)System.Math.IEEERemainder(t + tSpeed * Time.deltaTime, 2.0);


		float interpParam = t + 1.0f;
		if (interpParam > 1.0f)
			interpParam = 1.0f - (interpParam - 1.0f);


		this.GetComponent<Camera>().transform.rotation = startRotation * Quaternion.Slerp(rotationBeginQ, rotationEndQ, interpParam);
	}
}
