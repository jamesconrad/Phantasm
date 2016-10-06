using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class CollisionEventTrigger : MonoBehaviour
{

    public UnityEvent OnTriggerStart;
    public UnityEvent OnTriggerSit;
    public UnityEvent OnTriggerLeave;

    // OnTriggerEnter is called when the Collider other enters the trigger
    public void OnTriggerEnter(Collider other)
    {
        OnTriggerStart.Invoke();
    }
    
    // OnTriggerStay is called once per frame for every Collider other that is touching the trigger
    public void OnTriggerStay(Collider other)
    {
        OnTriggerSit.Invoke();
    }

    // OnTriggerExit is called when the Collider other has stopped touching the trigger
    public void OnTriggerExit(Collider other)
    {
        OnTriggerLeave.Invoke();
    }
}
