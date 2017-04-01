using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenuButton : MonoBehaviour {

	public void Click()
	{
		SceneManager.LoadScene("Menu");
	}
}
