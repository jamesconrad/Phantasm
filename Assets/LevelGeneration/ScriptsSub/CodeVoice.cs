using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum DoorUnlockChain
{

}

public class CodeVoice : MonoBehaviour 
{
	[Header("READ THE TOOLTIP")]
	[Tooltip("This is the room that the speaker is in, this ensures that the doors locked are not the same ones that agent must enter to get to the speaker\n\n" + 
	"0 is null, set it to something else so it'll work\n" +
	"1 is for the tutorial room")]
	public int roomNumber = 0;
	AudioSource audioSus;
	CodeVoiceCollection voiceCollection;
	// Code represented by a string
	string code = "";	
	// Code represented by an int
	int[] codeInt;
	// This is an array of voices to use when the code is spoken, 0 -> first voice, 1 -> second voice and so on
	int[] codeVoices;
	// Used to convert the codeInt into the string characters
	char[] codeWords = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
	
	

	// Variables controlling playback
	int currentChar = 0;


	// Use this for initialization
	void Start () 
	{
		audioSus = GetComponent<AudioSource>();
		voiceCollection = FindObjectOfType<CodeVoiceCollection>();
		if(voiceCollection == null)
		{
			Debug.Log("FUCK!\n" + "VOICE COLLECTION IS NULL!");
		}
		int codeLength = Random.Range(4, 6);
		codeInt = new int[codeLength];
		codeVoices = new int[codeLength];
		for(int i = 0; i < codeLength; ++i)
		{
			codeInt[i] = Random.Range(0, codeWords.Length);
			codeVoices[i] = Random.Range(0, voiceCollection.voices.Length);
			code += codeWords[codeInt[i]];
		}

		Debug.Log(code);


	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
