using UnityEngine;
using System.Collections;

public class RotateAroundPivot : MonoBehaviour {

    Vector3 pivotPoint;
    Quaternion startRotation;
    Quaternion endRotation;
    float currentDelta;
    public float rotationSpeed;
    public float RotationAngle;

    bool isPlaying = false;

    bool closed = true;

	// Use this for initialization
	void Start () {
       pivotPoint = transform.forward * 1.3f + transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    if (isPlaying)
        {
            if (closed)
            {
                transform.RotateAround(pivotPoint, Vector3.up, RotationAngle);
            }
            if (!closed)
            {
                transform.RotateAround(pivotPoint, Vector3.up, -RotationAngle);
            }
            isPlaying = false;
        }
	}

    public void OpenDoor()
    {
        startRotation = transform.rotation;
        if (closed == true)
        {
            endRotation = startRotation * Quaternion.AngleAxis(RotationAngle, Vector3.up);
        }
        else
        {
            endRotation = startRotation * Quaternion.AngleAxis(-RotationAngle, Vector3.up);
        }
        closed = !closed;
        isPlaying = true;
    }
}
