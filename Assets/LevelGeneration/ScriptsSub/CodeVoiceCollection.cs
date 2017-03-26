﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeVoiceCollection : MonoBehaviour 
{
	public AudioClip beep;
	public Plasma.VoiceCollection[] voices;

	private GoodDoor[] Doors;
	private List<GoodDoor> ExitDoors = new List<GoodDoor>();
	private CodeVoice[] Speakers;
	// These ints represent the "RoomNum" that a room has 
	private List<int> listOfRooms = new List<int>();
	private int numOfChains = 1;
	private CodeVoice tutorialSpeaker;
	private List<GoodDoor> tutorialDoors = new List<GoodDoor>();

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
				}
			}
			else if(Doors[i].roomNumber == 0 && Doors[i].exitNumber > 0)
			{
				Debug.Log("Exit Door Added");
				ExitDoors.Add(Doors[i]);
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
				tutorialSpeaker.genCode();
				tutorialSpeaker.setActive(true);
			}
		}
	}

	void SetRooms()
	{
		int firstRoom = Random.Range(0, listOfRooms.Count);
		
		Debug.Log("Setting Room " + listOfRooms.Count + "\nFirst Room is " + firstRoom);


		int counter = 1;
		while(listOfRooms.Count > 0 && counter <= numOfChains)
		{
			CodeVoice speaker = null;
			for(int i = 0; i < Speakers.Length; ++i) 
			{
				if (Speakers[i].roomNumber == listOfRooms[firstRoom])
				{
					speaker = Speakers[i];
					speaker.genCode();
					speaker.setActive(true);
				}
			}

			if(speaker == null)
			{
				Debug.Log("Well shit, that didn't turn out so well!");
			}
			else
			{
				Debug.Log("Cool, speaker isn't null for first room!");
			}
			
			++counter;
			if(listOfRooms.Count > 0 && counter <= numOfChains)
			{
				Debug.Log("Chaining the rooms");	
				
				for(int i = 0; i < Doors.Length; ++i) 
				{
					if (Doors[i].roomNumber == listOfRooms[firstRoom])
					{
						Doors[i].SetCode(speaker.getCode());
					}
				}
			}
			else
			{
				Debug.Log("Last Room gots to be chained to the exit");

				//for(int i = 0; i < ExitDoors.Count; ++i) 
				if(ExitDoors.Count > 0)
				{
					int randomExit = Random.Range(0, listOfRooms.Count);
					ExitDoors[randomExit].SetCode(speaker.getCode());
				}
				else
				{
					Debug.Log("ERROR: NO EXIT EXISTS");
				}
			}
		}

		
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