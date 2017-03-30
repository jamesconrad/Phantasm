using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour 
{
	static int score;
	float timeStart;
	float timeEnd;
	[TooltipAttribute("Maximum amount of points you get for the time")]
	public int scoreTime = 10000;
	[TooltipAttribute("(Seconds) How long it takes before the time bonus gives you nothing")]
	public float timeBonusEnd = 10.0f * 60.0f;
	int numberOfPhantomsKilled;
	[TooltipAttribute("How many points is killing a phantom worth?")]
	public int phantomKilledMult = 100;


	// Use this for initialization
	void Start () 
	{
        startTimer();
	}

    public void OnKill()
    {
        numberOfPhantomsKilled++;
    }

	public void startTimer()
	{
		timeStart = Time.time;
	}

	public void endTimer()
	{
		timeEnd = Time.time;
		timeEnd -= timeStart; 
	}

	public void calculateScore()
	{
		score = (int) Mathf.Lerp(scoreTime, 0.0f, Mathf.InverseLerp(0.0f, timeBonusEnd, timeEnd))
		 + numberOfPhantomsKilled * phantomKilledMult;
	}

	public void PrintScore()
	{
		Debug.Log("Score: " + (int)score);
	}

    public void saveScore()
    {
    }

    public static int getLastScore()
    {
		return score;
    }
}
