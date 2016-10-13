using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{

    public GameObject AgentUI;
    private Text SubObjectiveCounter;
    // Use this for initialization
    void Start()
    {
        AgentUI = Instantiate(AgentUI) as GameObject;

        Text[] textReferences;
        textReferences = AgentUI.GetComponentsInChildren<Text>();
        for (int i = 0; i < textReferences.Length; i++)
        {
            if (textReferences[i].name == "SubObjectiveCounter")
            {
                SubObjectiveCounter = textReferences[i];
                SetNumberOfObjectivesCompleted(FindObjectOfType<GameState>().numberOfSubObjectives);
            }
        }
    }

    public void SetNumberOfObjectivesCompleted(int _objectivesCompleted)
    {
        SubObjectiveCounter.text = _objectivesCompleted.ToString();
    }

    // This function is called when the MonoBehaviour will be destroyed
    public void OnDestroy()
    {
        Destroy(AgentUI);
    }
}
