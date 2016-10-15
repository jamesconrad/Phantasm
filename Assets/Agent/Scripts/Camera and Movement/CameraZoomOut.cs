using UnityEngine;
using System.Collections;

public class CameraZoomOut : MonoBehaviour {

    public Vector3 relativeTransform;
    private Vector3 finalPosition;
    [Range(0.0f, 1.0f)]
    public float movementSpeed;
    private float currentDelta;

    public Transform transformToLookAt;
    [Range(0.0f, 1.0f)]
    public float rotationSpeed;
    private float currentRotationDelta;

    private bool isPlaying = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (isPlaying)
        {
            if (transform.rotation != Quaternion.LookRotation((transformToLookAt.position - transform.position).normalized, Vector3.up))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((transformToLookAt.position - transform.position).normalized, Vector3.up), currentRotationDelta += Time.deltaTime * rotationSpeed);
            }
            if(transform.position != finalPosition)
            {
                transform.position = Vector3.Lerp(transform.position, finalPosition, currentDelta += Time.deltaTime * movementSpeed);

            }
            else
            {
                isPlaying = false;
            }
        }
	}

    public void PlayZoomOut()
    {
        isPlaying = true;
        finalPosition = transform.position + relativeTransform;
    }
}
