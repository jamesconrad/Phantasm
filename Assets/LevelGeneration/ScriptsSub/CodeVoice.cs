using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorUnlockChain
{
	Null,
	Tutorial,
	FirstRoom,
	SecondRoom,
	Exit
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
	bool codeGenned = false;
	// Code represented by a string
	string code = "";	
	// Code represented by an int
	int[] codeInt;
	// This is an array of voices to use when the code is spoken, 0 -> first voice, 1 -> second voice and so on
	int[] codeVoices;
	// Used to convert the codeInt into the string characters
	char[] codeWords = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
	
	
	DoorUnlockChain chainPosition = DoorUnlockChain.Null;
	// Variables controlling playback
	int currentChar = 0;
	bool activeSpeaker = false;

	// This was gonna a hard codey thingy, but I actually implemented the codes properly, yay me
	public void setChain(DoorUnlockChain chain)
	{
		chainPosition = chain;
	}
	public void setActive(bool act)
	{
		activeSpeaker = act;
	}


	public string getCode()
	{
		return code;
	}

	// Use this for initialization
	void Start () 
	{
		audioSus = GetComponent<AudioSource>();
	}
	
	public void genCode()
	{
		if(!codeGenned)
		{
			voiceCollection = FindObjectOfType<CodeVoiceCollection>();
			if(voiceCollection == null)
			{
				Debug.Log("FUCK!\n" + "VOICE COLLECTION IS NULL!");
			}
			int codeLength = Random.Range(5, 8);
			codeInt = new int[codeLength];
			codeVoices = new int[codeLength];
			for(int i = 0; i < codeLength; ++i)
			{
				codeInt[i] = Random.Range(0, codeWords.Length);
				codeVoices[i] = Random.Range(0, voiceCollection.voices.Length);
				code += codeWords[codeInt[i]];
			}

			codeGenned = true;

			Debug.Log(code);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if(activeSpeaker )
		{
			if(audioSus == null)
			{
				Debug.Log("WHAT THE SHIT");
			}
			if(!audioSus.isPlaying) 
			{
				if(currentChar < codeInt.Length)
				{
					//Debug.Log(	"Code Current: " + currentChar + "\n" +
					//			"Code Voice: " + codeVoices[currentChar] + "\n" +
					//			"Code Int: " + codeInt[currentChar]);
					audioSus.clip = voiceCollection.voices[codeVoices[currentChar]].voice[codeInt[currentChar]];
					audioSus.Play();
					currentChar++;
				}
				else 
				{
					audioSus.clip = voiceCollection.beep;
					audioSus.Play();
					currentChar = 0;
				}
				
			}
		}
	}
}
