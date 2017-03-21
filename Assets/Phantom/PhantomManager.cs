using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PhantomManager : PhaNetworkingMessager {

	private static PhantomManager singleton;
	public static PhantomManager Singleton 
	{ 
		get 
		{ 
			return singleton; 
		} 
	}

	//Network manager
	PhaNetworkManager netManager = null;

	//Data of the phantoms
	List<PhantomSpawnLocation> ListOfPhantomSpawners;
	List<Phantom> phantoms;
	List<Vector3> PreviousPositions;

	int size;

	// Use this for initialization
	void Awake () {
		singleton = this;
		netManager = PhaNetworkManager.Singleton;
		PhantomSpawnLocation[] tempSpawnLocations = GetComponentsInChildren<PhantomSpawnLocation>();
		size = tempSpawnLocations.Length;
		ListOfPhantomSpawners = new List<PhantomSpawnLocation>(size);
		phantoms = new List<Phantom>(size);
		PreviousPositions = new List<Vector3>(size);
		for (int i = 0; i < size; i++)
		{
			ListOfPhantomSpawners.Add(tempSpawnLocations[i]);
		}
	}

	public void AddPhantom(Phantom givenPhantom)
	{
		phantoms.Add(givenPhantom);
		PreviousPositions.Add(givenPhantom.transform.position);
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
					SendEnemyUpdate(phantoms[i].transform.position, phantoms[i].transform.rotation, i, PhaNetworkingAPI.targetIP);
				}
			}
		}
	}
}
