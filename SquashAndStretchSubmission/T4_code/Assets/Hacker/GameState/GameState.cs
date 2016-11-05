using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Events;
using System.Collections;

public class GameState : NetworkBehaviour {

    public int numberOfSubObjectives;
    public int numberOfCompletedSubObjectives;

    public UnityEvent OnAllSubObjectivesComplete;

    public GameObject agentReference;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        numberOfSubObjectives = FindObjectsOfType<SubObjective>().Length;
        numberOfCompletedSubObjectives = 0;
        
	}
	
	// Update is called once per frame
	void Update () {
	    if (agentReference == null && FindObjectOfType<Agent>())
        {
            agentReference = FindObjectOfType<Agent>().gameObject;
        }
	}

    public void EndGame()
    {
        if (!isServer)
        {
            return;
        }
        Camera.main.transform.parent = null;
        CustomNetworkManager.singleton.StopHost();
        NetworkServer.Shutdown();
        Cursor.lockState = CursorLockMode.None;
    }

    public static void StaticEndGame()
    {
        CustomNetworkManager.singleton.StopHost();
        NetworkServer.Shutdown();
        Cursor.lockState = CursorLockMode.None;
    }

    public void IncrementNumberOfCompletedSubObjectives()
    {
        numberOfCompletedSubObjectives += 1;
        if (numberOfCompletedSubObjectives == numberOfSubObjectives)
        {
            OnAllSubObjectivesComplete.Invoke();
        }
        agentReference.GetComponent<Agent>().SetNumberOfObjectivesCompleted(numberOfSubObjectives - numberOfCompletedSubObjectives);
    }
}
