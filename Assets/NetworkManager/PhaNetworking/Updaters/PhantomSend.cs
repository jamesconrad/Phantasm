using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomSend : PhaNetworkingMessager {

	
	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		SendEnemyUpdate(transform.position, transform.rotation, PhaNetworkingAPI.targetIP);
	}
}
