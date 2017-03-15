using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCameraOrientation : MonoBehaviour {

    public Transform cam;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.rotation = cam.rotation;
        transform.rotation *= Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(90.0f, Vector3.forward);
	}
}
