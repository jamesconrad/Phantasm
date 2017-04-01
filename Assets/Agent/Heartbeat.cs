using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class Heartbeat : MonoBehaviour {

	private float beatSpeed;
	public float beatIntensity;
	public bool beatActive;
	private bool vTest;



	IEnumerator ClearConsole(){
		//wait until console visible
		while (!Debug.developerConsoleVisible) {
			yield return null;
		}
		//this is required to wait for an additional frame,
		//Without this, clearing doesn't work
		yield return null; 
		Debug.ClearDeveloperConsole ();

	}

	IEnumerator Beat(){

		//vTest is set to false at the start, then true at the end,
		//	making it so that update will not ruin our fun.
		vTest = false;

		//Time between beat pairs of beats
		beatSpeed = Mathf.Lerp (1.1f, 0.25f, beatIntensity);
		beatSpeed = Mathf.Clamp (beatSpeed, 0, 1);
		yield return new WaitForSeconds (beatSpeed);

		//low beat
		float timer = 0.3f;
		while (timer >= 0) {
			GamePad.SetVibration (PlayerIndex.One, 1.0f, 1.0f);
			timer -= Time.deltaTime;
		}
		GamePad.SetVibration (PlayerIndex.One, 0.0f, 0.0f);

		//Time between low/high beat
		yield return new WaitForSeconds (0.2f);

		//high beat
		//print ("3");
		timer = 0.4f;
		while (timer >= 0) {
			GamePad.SetVibration (PlayerIndex.One, 1.0f, 1.0f);
			timer -= Time.deltaTime;
		}
		GamePad.SetVibration (PlayerIndex.One, 0.0f, 0.0f);

		//Again, vTest is set back to true,
		//	meaning it is now time to do this thing again
		vTest = true;
	}
		

	void Start () {
		vTest = true;
	}


	void FixedUpdate () {
		//the "vTest" should stop it from updating always
		//	(since waiting is part of a coroutine and such)
		//and, of course, it wont do a gosh dern thing if beat active is off.
		if (vTest && beatActive){
			StartCoroutine (Beat ());
		}
	}
}
