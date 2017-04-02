using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MusicVolumeSliderScript : MonoBehaviour 
{

	MusicManagerScript musicManager;

	// Use this for initialization
	void Start () 
	{
		musicManager = GameObject.FindObjectOfType<MusicManagerScript>();
	}
	
	public void SetValue(float newValue)
 	{
		 musicManager.maxVolume = newValue;
 	}

	// Update is called once per frame
	void Update ()
	{

	}
}
