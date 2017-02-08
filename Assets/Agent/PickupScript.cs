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
	void Update ()
    {
		
	}
}
