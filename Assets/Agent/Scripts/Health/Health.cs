using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour {

    [Tooltip("Amount of health for the object")]
    public float health;
    
    [SyncVar]
    public float currentHealth;

    public float deathDelay;

    public bool destroyOnDeath = true;
    public UnityEvent OnDeath;

	// Use this for initialization
	void Start () {
        currentHealth = health;
	}
	
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0.0f && destroyOnDeath)
        {
            Kill(deathDelay);
        }
        else if (currentHealth <= 0.0f)
        {
            OnDeath.Invoke();
        }
        else
        {
            
        }
    }

    public void Kill(float delay = 0.0f)
    {
        OnDeath.Invoke();
        Destroy(gameObject, delay);
    }
}
