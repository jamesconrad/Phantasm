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
        Vector3 crot = cam.eulerAngles;
        Vector3 rot = Vector3.zero;
        rot.z = crot.x;
        rot.x = crot.y;
        rot.y = crot.z;
        //transform.rotation.Equals(Quaternion.Euler(rot));
        transform.localEulerAngles = rot;
	}
}
