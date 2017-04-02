using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Phantom : MonoBehaviour
{
    public float attackDamage;

	[Space(5)]
	public Plasma.Visibility visibility;
	[Space(5)]

	public bool randomizeVisibility = true;

	static public int numKilled = 80000;

    public GameObject vanishParticleEffect;

    private PhantomSpawnLocation[] respawnPoints;
    public PhantomSpawnLocation previousSpawnLocation = null;
    
    public GameObject audioObject;

    // Start is called just before any of the Update methods is called the first time
    public void Start()
    {
		//setVisibility();


		if (!PhaNetworkManager.Ishost)
		{
			GetComponent<BehaviourTree>().enabled = false;
			GetComponent<NavMeshAgent>().enabled = false;
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
			
			numKilled = Mathf.Max(numKilled - 2, 0);
            Respawn();
			
        }
    }

    IEnumerator Respawn(float respawnTime = 1.0f)
    {
		//setVisibility();
		yield return new WaitForSeconds(respawnTime);

        PhantomSpawnLocation spawnLoc = previousSpawnLocation;
        do
        {
            if (respawnPoints.Length == 1)
            {
                break;
            }
            spawnLoc = respawnPoints[Random.Range(0, respawnPoints.Length)];
        } while (spawnLoc == previousSpawnLocation);

        previousSpawnLocation = spawnLoc;
        transform.position = spawnLoc.transform.position;

		gameObject.GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Health>().currentHealth = GetComponent<Health>().health;
		GetComponent<BehaviourTree>().enabled = true;
		GetComponent<NavMeshAgent>().enabled = true;
    }

    public void die(float respawnTime = 1.0f)
    {
        Score scoreSystem = FindObjectOfType<Score>();
		if(scoreSystem != null)
		{
        	scoreSystem.OnKill();
		}
		else
			Debug.LogWarning("Score error");
        Instantiate(vanishParticleEffect, transform.position, vanishParticleEffect.transform.rotation);

        if (audioObject.GetComponent<AudioSource>() != null)
        {
            GameObject temp = Instantiate(audioObject, transform.position, Quaternion.identity);
            temp.GetComponent<PlayThenDelete>().Play();
        }

		gameObject.GetComponent<Rigidbody>().useGravity = false;
		gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		transform.position = new Vector3(0.0f, -900000.0f, 0.0f);
		GetComponent<BehaviourTree>().enabled = false;
		GetComponent<NavMeshAgent>().enabled = false;

		StartCoroutine(Respawn(respawnTime));
    }

	public void setVisibility()
	{
		if(randomizeVisibility)
		{
			int randomMin = Mathf.Max(0, numKilled / 2 - 1);
			int randomMax = Mathf.Min(8, 2 + numKilled / 2 - 1);
			
			int randomNum = Random.Range(randomMin, randomMax);

			if(numKilled < 1)
				randomNum = 0;
			else if(numKilled < 3)
				randomNum = 1;

			switch(randomNum)
			{
				case 0:
					visibility.agent = Plasma.SeenBy.Agent.Visible;
					visibility.camera = Plasma.SeenBy.Camera.Visible;
					visibility.thermal = Plasma.SeenBy.Thermal.Visible;
					visibility.sonar = Plasma.SeenBy.Sonar.Visible;
					visibility.temperature = 1.0f;
					break;
				case 1:
					visibility.agent = Plasma.SeenBy.Agent.Translucent;
					visibility.camera = Plasma.SeenBy.Camera.Visible;
					visibility.thermal = Plasma.SeenBy.Thermal.Visible;
					visibility.sonar = Plasma.SeenBy.Sonar.Visible;
					visibility.temperature = 1.0f;
					break;
				case 2:
					visibility.agent = Plasma.SeenBy.Agent.Invisible;
					visibility.camera = Plasma.SeenBy.Camera.Visible;
					visibility.thermal = Plasma.SeenBy.Thermal.Visible;
					visibility.sonar = Plasma.SeenBy.Sonar.Visible;
					visibility.temperature = 1.0f;
					break;
				case 3:
					visibility.agent = Plasma.SeenBy.Agent.Invisible;
					visibility.camera = Plasma.SeenBy.Camera.Translucent;
					visibility.thermal = Plasma.SeenBy.Thermal.Visible;
					visibility.sonar = Plasma.SeenBy.Sonar.Visible;
					visibility.temperature = 1.0f;
					break;
				case 4:
					visibility.agent = Plasma.SeenBy.Agent.Invisible;
					visibility.camera = Plasma.SeenBy.Camera.Invisible;
					visibility.thermal = Plasma.SeenBy.Thermal.Visible;
					visibility.sonar = Plasma.SeenBy.Sonar.Invisible;
					visibility.temperature = 1.0f;
					break;
				case 5:
					visibility.agent = Plasma.SeenBy.Agent.Invisible;
					visibility.camera = Plasma.SeenBy.Camera.Invisible;
					visibility.thermal = Plasma.SeenBy.Thermal.Visible;
					visibility.sonar = Plasma.SeenBy.Sonar.Visible;
					visibility.temperature = 0.25f;					
					break;
				case 6:
					visibility.agent = Plasma.SeenBy.Agent.Invisible;
					visibility.camera = Plasma.SeenBy.Camera.Invisible;
					visibility.thermal = Plasma.SeenBy.Thermal.Invisible;
					visibility.sonar = Plasma.SeenBy.Sonar.Visible;
					visibility.temperature = 0.0f;
					break;
				case 7:
					visibility.agent = Plasma.SeenBy.Agent.Invisible;
					visibility.camera = Plasma.SeenBy.Camera.Invisible;
					visibility.thermal = Plasma.SeenBy.Thermal.Visible;
					visibility.sonar = Plasma.SeenBy.Sonar.Invisible;
					visibility.temperature = 0.0f;
					break;
				default:
					visibility.agent = Plasma.SeenBy.Agent.Visible;
					visibility.camera = Plasma.SeenBy.Camera.Visible;
					visibility.thermal = Plasma.SeenBy.Thermal.Visible;
					visibility.sonar = Plasma.SeenBy.Sonar.Visible;
					visibility.temperature = 1.0f;
					break;
			}
		}
	}
}
