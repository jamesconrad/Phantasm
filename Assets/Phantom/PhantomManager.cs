using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PhantomManager : PhaNetworkingMessager {

	//Network manager
	PhaNetworkManager netManager = null;

	//Data of the phantoms
	PhantomSpawnLocation[] ListOfPhantomSpawners;
	Phantom[] phantoms;
	Vector3[] PreviousPositions;

	int size;

	// Use this for initialization
	void Start () {
		netManager = PhaNetworkManager.Singleton;

		ListOfPhantomSpawners = GetComponentsInChildren<PhantomSpawnLocation>();
		phantoms = new Phantom[ListOfPhantomSpawners.Length];
		PreviousPositions = new Vector3[ListOfPhantomSpawners.Length];
		for (int i = 0; i < ListOfPhantomSpawners.Length; i++)
		{
			phantoms[i] = ListOfPhantomSpawners[i].GetComponent<Phantom>();
			PreviousPositions[i] = ListOfPhantomSpawners[i].transform.position;
		}
		size = ListOfPhantomSpawners.Length;
	}

	public void ParsePhantomUpdate(int id, StringBuilder buffer)
	{
		ParseObjectUpdate(buffer, phantoms[id].transform);
	}

	/// <summary>
	/// LateUpdate is called every frame, if the Behaviour is enabled.
	/// It is called after all Update functions have been called.
	/// </summary>
	void LateUpdate()
	{
		for (int i = 0; i < size; i++)
		{
			if (PreviousPositions[i] != phantoms[i].transform.position)
			{
				PreviousPositions[i] = phantoms[i].transform.position;

				if (PhaNetworkManager.Ishost)
				{
					//Send new position;
				}
			}
		}
	}
}
