using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public GunHandle gunHandle;
    public Health healthHandle;

    public void OnCollisionEnter(Collision collision)
    {
        PickupScript tempPickup;
        if ((tempPickup = collision.gameObject.GetComponent<PickupScript>()) != null)
        {
            
            if (tempPickup.itemType == PickupType.Ammo)
            {
                gunHandle.weaponSettings.currentNumberOfClips = Mathf.Min(10, (int)tempPickup.amount + gunHandle.weaponSettings.currentNumberOfClips);
            }
            else if (tempPickup.itemType == PickupType.Health)
            {
                healthHandle.currentHealth = Mathf.Min(healthHandle.health, healthHandle.currentHealth + tempPickup.amount);
            }

            DestroyObject(collision.gameObject);
        }
    }
    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
