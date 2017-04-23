using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeVoiceCollection : MonoBehaviour 
{
	public AudioClip beep;
	public Plasma.VoiceCollection[] voices;

	private GoodDoor[] Doors;
	private List<GoodDoor> ExitDoors = new List<GoodDoor>();
	private List<int> listOfExitDoorGroups = new List<int>();
	private CodeVoice[] Speakers;
	// These ints represent the "RoomNum" that a room has 
	private List<int> listOfRooms = new List<int>();
	private int numOfChains = 3;
	private CodeVoice tutorialSpeaker;
	private List<GoodDoor> tutorialDoors = new List<GoodDoor>();

	// Use this for initialization
	void Start () 
	{		
		if (!(PhaNetworkManager.characterSelection == 0))
		{
			return;
		}
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

		// Group the exit doors
		for(int i = 0; i < ExitDoors.Count; ++i)
		{
			bool roomFound = false;
			for(int c = 0; c < listOfExitDoorGroups.Count; ++c)
			{
				if(listOfExitDoorGroups[c] == ExitDoors[i].exitNumber)
					roomFound = true;
			}
			if(!roomFound)
			{
				listOfExitDoorGroups.Add(ExitDoors[i].exitNumber);
				//Debug.Log("");
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
		
		for(int i = 0; i < tutorialDoors.Count; ++i)
		{
			tutorialDoors[i].SetCode(tutorialSpeaker.getCode());
		}
	}

	void SetRooms()
	{
		
		// First Speaker
		int selectedRoom = Random.Range(0, listOfRooms.Count);

		int counter = 1;
		while(listOfRooms.Count > 0 && counter <= numOfChains)
		{
		
			Debug.Log("Rooms Left: " + listOfRooms.Count + "\nRoom chain #" + counter + " is in Room #" + listOfRooms[selectedRoom]);

			CodeVoice speaker = null;
			for(int i = 0; i < Speakers.Length; ++i) 
			{
				if (Speakers[i].roomNumber == listOfRooms[selectedRoom])
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
			if(listOfRooms.Count > 1 && counter <= numOfChains)
			{
				Debug.Log("Chaining the rooms " + listOfRooms[selectedRoom]);	
				
				// Link to new door
				listOfRooms.RemoveAt(selectedRoom);
				selectedRoom = Random.Range(0, listOfRooms.Count);
				
				for(int i = 0; i < Doors.Length; ++i) 
				{
					if (Doors[i].roomNumber == listOfRooms[selectedRoom])
					{
						Doors[i].SetCode(speaker.getCode());
					}
				}
			}
			else
			{
				counter = numOfChains + 1;
				Debug.Log("Last Room gots to be chained to the exit");

				//for(int i = 0; i < ExitDoors.Count; ++i) 
				if(listOfExitDoorGroups.Count > 0)
				{
					Debug.Log("Creating Exit " + listOfExitDoorGroups.Count);
					int randomExit = Random.Range(0, listOfExitDoorGroups.Count);
					Debug.Log("Exit #" + randomExit + " has been selected");
					for(int i = 0; i < ExitDoors.Count; ++i)
					{
						if (ExitDoors[i].exitNumber == listOfExitDoorGroups[randomExit])
						{
							ExitDoors[i].SetCode(speaker.getCode());
						}
					}
				}
				else
				{
					Debug.Log("ERROR: NO EXIT EXISTS");
				}
			}
		}

		
	}

bool hasInformedOtherPlayer = false;
	// Update is called once per frame
	void Update () 
	{
		if (!hasInformedOtherPlayer && PhaNetworkManager.characterSelection == 0)
		{
			DoorManager.Singleton.SendDoorMessages();
			hasInformedOtherPlayer = true;			
		}
	}

public CodeVoice[] GetCodeVoices()
{
	return Speakers;
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