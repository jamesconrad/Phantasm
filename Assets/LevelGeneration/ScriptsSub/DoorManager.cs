using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DoorManager : PhaNetworkingMessager {

	private static DoorManager singleton;
	public static DoorManager Singleton 
	{ 
		get 
		{ 
			return singleton; 
		} 
	}

	GoodDoor[] doors;
	Vector3[] previousPositions;
	// Use this for initialization
	void Start () {
		singleton = this;
		doors = GetComponentsInChildren<GoodDoor>();
		previousPositions = new Vector3[doors.Length];
		for (int i = 0; i < doors.Length; i++)
		{
			previousPositions[i] = doors[i].transform.position;
		}
	}

	public void parseDoorUpdate(ref StringBuilder buffer)
	{
		string[] values = buffer.ToString().Split(' ');
		int id = int.Parse(values[1]);

		Vector3 newPos = new Vector3(float.Parse(values[2]), float.Parse(values[3]), float.Parse(values[4]));
		Quaternion newQuat = new Quaternion(float.Parse(values[5]), float.Parse(values[6]), float.Parse(values[7]), float.Parse(values[8]));

		doors[id].transform.rotation = newQuat;
	
	}
	
	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		if (PhaNetworkManager.characterSelection == 0)
		{
			for (int i = 0; i < doors.Length; i++)
			{
				if (previousPositions[i] != doors[i].transform.position)
				{
					SendDoorUpdate(i, doors[i].transform.position, doors[i].transform.rotation);
					previousPositions[i] = doors[i].transform.position;
				}
			}
		}
	}
}
