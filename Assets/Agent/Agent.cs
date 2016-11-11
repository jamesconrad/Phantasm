using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class Agent : NetworkBehaviour
{

    public GameObject AgentUI;
    private Text SubObjectiveCounter;
    private Text AmmoCounter;

    public Font _Font;

    // Use this for initialization
    void Start()
    {

        SubObjectiveCounter.font = _Font;
        AmmoCounter.font = _Font;
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
        CustomNetworkManager.singleton.GetComponent<NetworkManagerHUD>().showGUI = false;

        UnityAction endGameAction = new UnityAction(FindObjectOfType<GameState>().EndGame);
        SplashScreen endGameScreen = AgentUI.GetComponentInChildren<SplashScreen>();
        endGameScreen.OnTimeReached.AddListener(endGameAction);
        endGameScreen.OnTimeReached.AddListener(() => { CustomNetworkManager.singleton.GetComponent<NetworkManagerHUD>().showGUI = true; });
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
        //Destroy(AgentUI);

    }
}
