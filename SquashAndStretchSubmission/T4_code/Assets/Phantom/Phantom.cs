using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Phantom : NetworkBehaviour
{
    public float attackDamage;

    public GameObject vanishParticleEffect;

    private PhantomSpawnLocation[] respawnPoints;
    public PhantomSpawnLocation previousSpawnLocation = null;

    // Start is called just before any of the Update methods is called the first time
    public void Start()
    {
        if (!isLocalPlayer)
        {
            //Change to apply the correct shaders when Stephen gets them done.
            //GetComponent<MeshRenderer>().enabled = false;

        }

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
            collision.rigidbody.AddForce((collision.transform.position - transform.position).normalized * 25.0f, ForceMode.Impulse);
            Respawn();
        }
    }

    public void Respawn()
    {
        Destroy(Instantiate(vanishParticleEffect, transform.position, vanishParticleEffect.transform.rotation), vanishParticleEffect.GetComponent<ParticleSystem>().duration);
        PhantomSpawnLocation spawnLoc = previousSpawnLocation;
        do
        {
            spawnLoc = respawnPoints[Random.Range(0, respawnPoints.Length)];
            transform.position = spawnLoc.transform.position;
            if (respawnPoints.Length == 1)
            {
                break;
            }
        } while (spawnLoc == previousSpawnLocation);
        previousSpawnLocation = spawnLoc;

        GetComponent<Health>().currentHealth = GetComponent<Health>().health;
    }

    // This function is called when the MonoBehaviour will be destroyed
    public void OnDestroy()
    {
        GameState.StaticEndGame();
    }
}
