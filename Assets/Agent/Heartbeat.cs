using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class Heartbeat : MonoBehaviour {

	private float leftMotor;
	private float rightMotor;
	private GamePadState state;
	private int rumbleTimer;
	private float beatSpeed;
	public float beatIntensity;
	public bool beatActive;



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
		while (true) {
			
			//Time between beat pairs
			beatSpeed = Mathf.Lerp (1.1f, 0.25f, beatIntensity);
			beatSpeed = Mathf.Clamp (beatSpeed, 0, 1);
			yield return new WaitForSeconds (beatSpeed);

			//If the heartbeat is turned on
			if (beatActive) {
				//low beat
				float timer = 0.1f;
				while (timer >= 0) {
					GamePad.SetVibration (PlayerIndex.One, 1.0f, 1.0f);
					timer -= Time.deltaTime;
				}

				//Time between low/high beat
				yield return new WaitForSeconds (0.2f);

				//high beat
				timer = 0.12f;
				while (timer >= 0) {
					GamePad.SetVibration (PlayerIndex.One, 1.0f, 1.0f);
					timer -= Time.deltaTime;
				}
			}
		}
	}
		

	// Use this for initialization
	void Start () {
		//StartCoroutine (lowBeat ());
		rumbleTimer = 50;

		StartCoroutine (Beat ());
	}


	//// Update is called once per frame
	//void Update () {
	//
	//}
}
