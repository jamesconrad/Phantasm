using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Phantom : NetworkBehaviour
{
    public float attackDamage;

    // Start is called just before any of the Update methods is called the first time
    public void Start()
    {
        if (CustomNetworkManager.singleton.playerPrefab == CustomNetworkManager.singleton.spawnPrefabs[1])
        {
            GetComponent<Collider>().enabled = false;
        }
    }



    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().takeDamage(attackDamage);
        }
    }
}
