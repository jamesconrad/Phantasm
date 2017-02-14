using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCameraOrientation : MonoBehaviour {

    private Transform cam;
    private Vector3 brot;

	// Use this for initialization
	void Start () {
        cam = transform.parent.transform.parent.transform.parent.transform;
        brot = new Vector3(90, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 crot = cam.eulerAngles;
        Vector3 rot = crot + brot;
        transform.rotation.Equals(Quaternion.Euler(rot));
	}
}
