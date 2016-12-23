using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMainMenuButton : MonoBehaviour {

	public void Click()
	{
		FindObjectOfType<CustomNetworkManager>().endGame();
	}
}
