using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GenericBullet : NetworkBehaviour {
    public float maxLife = 5.0f;
    public float currentLife;

    public float damage;
    public float speed;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
        currentLife = maxLife;
    }

    // Called on clients for player objects for the local client (only)
    public override void OnStartLocalPlayer()
    {
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
        Destroy(gameObject, 0.0066f);
    }
}
