using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeVoiceCollection : MonoBehaviour 
{
	public AudioClip beep;
	public Plasma.VoiceCollection[] voices;

	private GoodDoor[] Doors;
	private CodeVoice[] Speakers;
	private List<int> listOfRooms = new List<int>();
	private CodeVoice tutorialSpeaker;
	private List<GoodDoor> tutorialDoors = new List<GoodDoor>();
	private int numOfRooms = 0;

	// Use this for initialization
	void Start () 
	{		
		Doors = FindObjectsOfType<GoodDoor>();	
		Speakers = FindObjectsOfType<CodeVoice>();	

		for(int i = 0; i < Doors.Length; ++i)
		{
			if(Doors[i].roomNumber > 1)
			{
				bool roomFound = false;
				for(int c = 0; c < listOfRooms.Count; ++c)
				{
					if(listOfRooms[c] == Doors[i].roomNumber)
						roomFound = true;
				}
				if(!roomFound)
				{
					listOfRooms.Add(Doors[i].roomNumber);
					//numOfRooms = Doors[i].roomNumber;
				}
			}
		}
		for(int i = 0; i < Speakers.Length; ++i)
		{
			if(Speakers[i].roomNumber == 1)
			{
				Speakers[i].setChain(DoorUnlockChain.Tutorial);
			}
		}
		Debug.Log("Number of Rooms: " + listOfRooms.Count);
		
		SetTutorialRoom();
		SetRooms();
	}
	
	void SetTutorialRoom()
	{
		for(int i = 0; i < Doors.Length; ++i)
		{
			if(Doors[i].roomNumber == 1)
			{
				tutorialDoors.Add(Doors[i]);
			}
		}

		for(int i = 0; i < Speakers.Length; ++i)
		{
			if(Speakers[i].roomNumber == 1)
			{
				tutorialSpeaker = Speakers[i];
				tutorialSpeaker.setActive(true);
				break;
			}
		}
	}

	void SetRooms()
	{
		int firstRoom = Random.Range(0, numOfRooms);
		//for(int i = 0; i < Speakers.Length; ++i)
		//{
		//	
		//}
	}

	// Update is called once per frame
	void Update () 
	{
		
	}
}

namespace Plasma
{
	[System.Serializable]
	public class VoiceCollection
	{
		public AudioClip[] voice = new AudioClip[16];
	}
}