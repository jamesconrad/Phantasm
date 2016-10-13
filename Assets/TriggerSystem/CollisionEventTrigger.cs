using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class CollisionEventTrigger : MonoBehaviour
{
    public bool UseTagFilter = false;
    public string TagFilter;
    public UnityEvent OnTriggerStart;
    public UnityEvent OnTriggerSit;
    public UnityEvent OnTriggerLeave;

    // OnTriggerEnter is called when the Collider other enters the trigger
    public void OnTriggerEnter(Collider other)
    {
        if (UseTagFilter)
        {
            if (other.CompareTag(TagFilter))
            {
                OnTriggerStart.Invoke();
            }
        }
        else
        {
            OnTriggerStart.Invoke();
        }
    }
    
    // OnTriggerStay is called once per frame for every Collider other that is touching the trigger
    public void OnTriggerStay(Collider other)
    {
        if (UseTagFilter)
        {
            if (other.CompareTag(TagFilter))
            {
                OnTriggerSit.Invoke();
            }
        }
        else
        {
            OnTriggerSit.Invoke();
        }
    }

    // OnTriggerExit is called when the Collider other has stopped touching the trigger
    public void OnTriggerExit(Collider other)
    {
        if (UseTagFilter)
        {
            if (other.CompareTag(TagFilter))
            {
                OnTriggerLeave.Invoke();
            }
        }
        else
        {
            OnTriggerLeave.Invoke();
        }
    }
}
