using UnityEngine;
using System.Collections;

public class SubObjective : MonoBehaviour
{
    private GameState manager;

    // Start is called just before any of the Update methods is called the first time
    public void Start()
    {
        manager = FindObjectOfType<GameState>();
    }

    public void CompleteSubObjective()
    {
        if (!manager)
        {
            manager = FindObjectOfType<GameState>();
        }
        manager.IncrementNumberOfCompletedSubObjectives();
    }
}
