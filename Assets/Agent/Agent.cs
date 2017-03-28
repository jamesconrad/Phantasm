using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class Agent : MonoBehaviour
{
    public GameObject AgentUI;
    public GameObject PauseUI;
	
	[Space(5)]
	public Plasma.Visibility visibility;
	[Space(5)]

    private Text SubObjectiveCounter;
    private Text AmmoCounter;
    // Use this for initialization
    void Start()
    {
		visibility.agent = Plasma.SeenBy.Agent.Visible;
		visibility.camera = Plasma.SeenBy.Camera.Visible;
		visibility.thermal = Plasma.SeenBy.Thermal.Visible;
		visibility.sonar = Plasma.SeenBy.Sonar.Visible;
		visibility.temperature = 1.0f;
    }

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        PauseUI = Instantiate(PauseUI) as GameObject;
        PauseUI.SetActive(false);
        
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
            else if (textReferences[i].gameObject.name == "AmmoCounter")
            {
                AmmoCounter = textReferences[i];
                SetAmmoCount(GetComponent<GunHandle>().weaponSettings.currentNumberOfRounds, GetComponent<GunHandle>().weaponSettings.currentNumberOfClips);
            }
        }
        
        SplashScreen endGameScreen = AgentUI.GetComponentInChildren<SplashScreen>();
        endGameScreen.screenOwner = gameObject;
    }

    public void SetNumberOfObjectivesCompleted(int _objectivesCompleted)
    {
        SubObjectiveCounter.text = _objectivesCompleted.ToString();
    }

    public void SetAmmoCount(int _ammo, int _mags)
    {
        AmmoCounter.text = _ammo.ToString("00") + "/" + _mags.ToString("00");
    }

    // This function is called when the behaviour becomes disabled or inactive
    public void OnDisable()
    {
        Score scoreSystem = FindObjectOfType<Score>();
        scoreSystem.endTimer();
        scoreSystem.saveScore();
        AgentUI.GetComponentInChildren<SplashScreen>().createSplashScreen(0);
        GetComponent<GunHandle>().gunReference.gameObject.SetActive(false);
    }

    /// Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<FirstPersonMovement>().enabled = !GetComponent<FirstPersonMovement>().enabled;
            GetComponent<FirstPersonCamera>().enabled = !GetComponent<FirstPersonCamera>().enabled;
            PauseUI.SetActive(!PauseUI.activeSelf);
        }
    }

    // This function is called when the MonoBehaviour will be destroyed
    public void OnDestroy()
    {
        //Destroy(AgentUI);

    }
}
