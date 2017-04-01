using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameState : MonoBehaviour
{

    public int numberOfSubObjectives;
    public int numberOfCompletedSubObjectives;

    public UnityEvent OnAllSubObjectivesComplete;

    public GameObject agentReference;

    public GameObject[] possibleEnds;
    public GameObject EndGameTrigger;
    public Shader endShader;

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        if (possibleEnds.Length > 0)
        {
            GameObject end = possibleEnds[Random.Range(0, possibleEnds.Length - 1)];
            end.GetComponent<MeshRenderer>().sharedMaterial = new Material(endShader);
            GameObject trigger = (Instantiate(EndGameTrigger, end.transform.position, end.transform.rotation, end.transform) as GameObject);
            //trigger.GetComponent<CollisionEventTrigger>().OnTriggerStart.AddListener(() => { (CustomNetworkManager.singleton as CustomNetworkManager).endGame(); });
            trigger.GetComponent<CollisionEventTrigger>().OnTriggerStart.AddListener(() => { agentReference.GetComponent<Agent>().AgentUI.GetComponentInChildren<SplashScreen>().createSplashScreen(1); });
        }
    }

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        numberOfSubObjectives = FindObjectsOfType<SubObjective>().Length;
        numberOfCompletedSubObjectives = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (agentReference == null && FindObjectOfType<Agent>())
        {
            agentReference = FindObjectOfType<Agent>().gameObject;
        }
    }

    public void EndGame()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public static void StaticEndGame()
    {
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
