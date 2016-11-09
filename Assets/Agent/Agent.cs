using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class Agent : NetworkBehaviour
{

    public GameObject AgentUI;
    private Text SubObjectiveCounter;
    private Text AmmoCounter;
    // Use this for initialization
    void Start()
    {
        
    }

    // Called on clients for player objects for the local client (only)
    public override void OnStartLocalPlayer()
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
            else if (textReferences[i].name == "AmmoCounter")
            {
                AmmoCounter = textReferences[i];
                SetAmmoCount(GetComponent<GunHandle>().weaponSettings.currentNumberOfRounds);
            }
        }
    }

    public void SetNumberOfObjectivesCompleted(int _objectivesCompleted)
    {
        SubObjectiveCounter.text = _objectivesCompleted.ToString();
    }

    public void SetAmmoCount(int _ammo)
    {
        AmmoCounter.text = _ammo.ToString("00");
    }

    // This function is called when the MonoBehaviour will be destroyed
    public void OnDestroy()
    {
        AgentUI.GetComponentInChildren<SplashScreen>().createSplashScreen();
        GetComponent<GunHandle>().gunReference.gameObject.SetActive(false);
        GameObject actualCamera = FindObjectOfType<GlobalParameters>().gameObject;
        actualCamera.transform.position = transform.position;
        actualCamera.transform.rotation = GetComponent<GunHandle>().gunReference.transform.rotation;
        //Destroy(AgentUI);
    }
}
