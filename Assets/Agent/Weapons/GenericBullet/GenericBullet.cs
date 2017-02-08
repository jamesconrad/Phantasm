using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GenericBullet : MonoBehaviour {
    public float maxLife = 5.0f;
    public float currentLife;

    public float damage;
    public float speed;

    [Tooltip("Object to create on death")]
    public GameObject onDeath;
    public GameObject onDeathMetallic;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        currentLife -= Time.deltaTime;

        if (currentLife < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Health>().takeDamage(damage);
        }
		if (collision.gameObject.CompareTag("Player") != true)
		{
            if (GunLaserScript.metallicHit < 0.5f)
            {
                Instantiate(onDeath, this.transform.position, this.transform.rotation);
            }
            else
            {
                Instantiate(onDeathMetallic, this.transform.position, this.transform.rotation); 
            }
			Destroy(gameObject);
		}
	}
}
