using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeVoiceCollection : MonoBehaviour 
{
	public Plasma.VoiceCollection[] voices;

	private GoodDoor[] Doors;
	private List<int> listOfRooms = new List<int>();
	private int numOfRooms = 0;

	// Use this for initialization
	void Start () 
	{		
		Doors = FindObjectsOfType<GoodDoor>();	

		for(int i = 0; i < Doors.Length; ++i)
		{
			if(Doors[i].roomNumber > 0)
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

		Debug.Log("Number of Rooms: " + listOfRooms.Count);
		
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