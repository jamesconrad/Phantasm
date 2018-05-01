using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    Health,
    Ammo
};

public class PickupScript : MonoBehaviour
{
    

    public PickupType itemType = PickupType.Health;

    public float amount = 10.0f;


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		transform.Rotate(50.0f * Time.fixedDeltaTime, 0.0f, 0.0f, Space.Self);
        transform.Translate(0.0f, Mathf.Sin(Time.fixedTime) * 0.005f, 0.0f, Space.World);
	}
}
