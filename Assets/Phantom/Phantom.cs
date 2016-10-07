using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Phantom : NetworkBehaviour
{
    public float attackDamage;

    public GameObject vanishParticleEffect;

    private PhantomSpawnLocation[] respawnPoints;

    // Start is called just before any of the Update methods is called the first time
    public void Start()
    {
        if (CustomNetworkManager.singleton.playerPrefab == CustomNetworkManager.singleton.spawnPrefabs[1])
        {
            GetComponent<Collider>().enabled = false;
        }

        respawnPoints = FindObjectsOfType<PhantomSpawnLocation>();
    }



    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().takeDamage(attackDamage);
        }
    }

    public void Respawn()
    {
        Instantiate(vanishParticleEffect, transform.position, Quaternion.identity);

        transform.position = respawnPoints[Random.Range(0, respawnPoints.Length - 1)].transform.position;

        GetComponent<Health>().currentHealth = GetComponent<Health>().health;
    }
}
