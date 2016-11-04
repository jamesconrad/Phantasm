using UnityEngine;
using System.Collections;

public class HackerSwitch : MonoBehaviour
{


    public AudioClip switchSound;
    public AudioSource soundSource;

    // Use this for initialization
    void Start ()
    {
        soundSource.clip = switchSound;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            soundSource.clip = switchSound;
            soundSource.Play();
        }
    }
}
