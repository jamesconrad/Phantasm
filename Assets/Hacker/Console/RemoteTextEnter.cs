using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RemoteTextEnter : MonoBehaviour {

    // Reference to all the doors in the level
    // When text is inputted, check all doors and unlock them if the code is correct
    private GoodDoor[] Doors;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
        Doors = FindObjectsOfType<GoodDoor>();
	}

	public void ReceiveCode(StringBuilder message)
	{
		checkDoors(message.ToString());
	}

    void checkDoors(string code)
    {
        Doors = FindObjectsOfType<GoodDoor>();
        for(int i = 0; i < Doors.Length; ++i)
        {
            if(!Doors[i].isActive())
            {
            	Doors[i].Unlock(code);
            }
        }
    }
}
