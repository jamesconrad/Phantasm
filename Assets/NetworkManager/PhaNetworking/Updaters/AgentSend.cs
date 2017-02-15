using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AgentSend : PhaNetworkingMessager {

	Health agentHealth;
	// Use this for initialization
	void Start () {
		agentHealth.GetComponent<Health>();
	}
	
	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		SendPlayerUpdate(transform.position, transform.rotation, PhaNetworkingAPI.targetIP);
	}
}
