using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//TODO: Test the score keeping when we go ahead and get the level finished.
public class ScoreFetch : MonoBehaviour {

	// Use this for initialization
    public string scores;
    int LatestScore = -1;

	void Start () {
        Text t = GetComponent<Text>();
        scores = System.IO.File.ReadAllText(Application.dataPath + "\\Resources\\Phantasm.score");
        string[] splitScores = scores.Split(' ');
        if (LatestScore > -1)
        {
            if (LatestScore > int.Parse(splitScores[0]))
            {
                string newScores = LatestScore.ToString() + " " + PhaNetworkingAPI.hostAddress.ToString() + " " + PhaNetworkingAPI.targetIP.ToString() + "\n";
                newScores += scores;
                scores = newScores;
                Debug.Log("Seomthing added to scores");
            }
            LatestScore = -1;
        }
        string textstring = "Scores: \n";
        for (int i = 0, length = splitScores.Length; i < length; i+=3)
        {
            textstring += splitScores[i+1] + " scored " + splitScores[i] + " with the help of " + splitScores[i + 2] + "\n";
        }
        t.text = textstring;
        print(t.name);
        
        SceneManager.activeSceneChanged += AddScene;
    }
	
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        SaveScores();
    }

    void AddScene(Scene _sceneA, Scene _sceneB)
    {
        if (_sceneB.name == "Menu")
        {
            LatestScore = Score.getLastScore();
        }
    }

    public void ReceiveBuffer(ref StringBuilder buffer)
    {
        string[] splitScores = buffer.ToString().Split(' ');
        if (int.Parse(scores.Split(' ')[0]) < int.Parse(splitScores[0])) 
        {
            string newScores = splitScores[0] + " " + splitScores[1] + " " + splitScores[2] + "\n";
            newScores += scores;
            scores = newScores;
        }       
    }

    public void SendTopScore()
    {
        string[] splitScores = scores.Split(' ');
        StringBuilder sendBuffer = new StringBuilder(((int)PhaNetworkingMessager.MessageType.ScoreUpdate).ToString() + " " + splitScores[0] + " " + splitScores[1] + " " + splitScores[2]);
        PhaNetworkingAPI.SendTo(PhaNetworkingAPI.mainSocket, sendBuffer, sendBuffer.Length, PhaNetworkingAPI.targetIP);
    }

    private void SaveScores()
    {
        string[] splitScores = scores.Split(' ');
        string SaveData = "";
        for (int i = 0, length = splitScores.Length; i < length || i >= 15; i+=3)
        {
            SaveData += splitScores[i] + " " + splitScores[i + 1] + " " + splitScores[i + 2];
        }
        System.IO.File.WriteAllText(Application.dataPath + "\\Resources\\Phantasm.score", SaveData);
    }
}
