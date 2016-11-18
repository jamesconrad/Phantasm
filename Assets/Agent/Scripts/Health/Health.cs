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
    
    public bool destroyOnDeath = false;
    public UnityEvent OnDeath;

	// Use this for initialization
	void Start () {
        currentHealth = health;
	}
	
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0.0f )
        {
        }
        if (currentHealth <= 0.0f && destroyOnDeath)
        {
            OnDeath.Invoke();
        }
    }
}
