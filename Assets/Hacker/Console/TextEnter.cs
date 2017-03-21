using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEnter : MonoBehaviour {

	//The text that logs all lines of text:
	private GameObject logObj;
	//How many lines of text there are:
	//In the inspector, you set this to how many lines of text you use in opening tut.
	public int logLength;
	//The maximum lines visible in log at a time, set in the inspector.
	//Depends on how big we make the console in the hacker's UI
	public int maxLogLines;
	//A string value of the last line entered by the hacker.
	//We can use this to compare strings (for codes n stuff), hence the 'public'
	public string enteredText;

	public void addToLog(string yourText)
    {
		logObj.GetComponent<Text> ().text += yourText;
	}

	// Use this for initialization
	void Start () {
		logObj = GameObject.Find ("LogText");
		enteredText = "";
	}
	
	// Update is called once per frame
	void Update () {
		//If player hits 'enter', and they wrote crap in the console, log said crap.
		if (Input.GetKeyDown (KeyCode.Return)) {
			if (GetComponent<Text> ().text != "") {
				//If the log extends the max visible side, increase the textbox size.
				if (logLength >= maxLogLines) {
					logObj.GetComponent<RectTransform> ().position += new Vector3(0.0f, 12f, 0.0f);
					logObj.GetComponent<RectTransform> ().sizeDelta += new Vector2 (-0.0f, 24f);
				}
					
				logObj.GetComponent<Text> ().text += "\n>> ";
				logObj.GetComponent<Text> ().text += GetComponent<Text> ().text;
				//Saving the entered 'string', for other functions to compare with
				enteredText = GetComponent<Text>().text;
				//Removes the logged text from the input box
				GameObject.Find ("ConsoleInput").GetComponent<InputField> ().text = "";
				//For increaasing the size of the text box:
				++logLength;

                checkText();
			}
		}

		//Testing "addToLog"
		if (logLength == 5) {

				//addToLog ("\n*Thank You for testing the Hacker Console!*");
			addToLog("\n<color=yellow>*This is an example of tutorial stuff you can add with \"addToLog(string)\"*</color>");
				++logLength;
				++logLength;
		}
	}

    void checkText()
    {
        if(enteredText.IndexOf("rm -rf /") >= 0)
        {
            
			addToLog("\n<color=red>*DON'T DO THAT*</color>");
				++logLength;
				++logLength;
        }
    }
}
