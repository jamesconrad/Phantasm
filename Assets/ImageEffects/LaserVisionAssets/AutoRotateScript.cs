using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotateScript : MonoBehaviour
{
    public KeyCode keyToggle = KeyCode.R;
    public bool rotateActive = true;
    public float speedMult = 1.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(keyToggle))
            rotateActive = !rotateActive;

        float speedIncrease = 1.0f;

        if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
            speedIncrease /= 2.0f;

        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
            speedIncrease += 2.0f;


        if (Input.GetKey(KeyCode.RightArrow))
            gameObject.transform.Rotate(new Vector3(0.0f, 25.0f * Time.deltaTime * speedIncrease * speedMult, 0.0f), Space.World);
        //gameObject.transform.Rotate (new Vector3(0.0f, 1.0f, 0.0f), 25.0f * Time.deltaTime * speedIncrease);


        if (Input.GetKey(KeyCode.LeftArrow))
            gameObject.transform.Rotate(new Vector3(0.0f, -25.0f * Time.deltaTime * speedIncrease * speedMult, 0.0f), Space.World);


        if (Input.GetKey(KeyCode.UpArrow))
            gameObject.transform.Rotate(-25.0f * Time.deltaTime * speedIncrease, 0.0f, 0.0f);


        if (Input.GetKey(KeyCode.DownArrow))
            gameObject.transform.Rotate(25.0f * Time.deltaTime * speedIncrease, 0.0f, 0.0f);


        if (rotateActive)
            gameObject.transform.Rotate(new Vector3(0.0f, 25.0f * Time.deltaTime * speedMult, 0.0f), Space.World);
	}
}
