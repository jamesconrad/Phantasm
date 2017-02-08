using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayThenDelete : MonoBehaviour
{
    public AudioClip clip;
    AudioSource source;
    float delay = 0.0f;
    bool activated = false;

	// Use this for initialization 
	void Start ()
    {
        //source = this.gameObject.AddComponent<AudioSource>();
        this.gameObject.GetComponent<AudioSource>().clip = clip;
        Play();
	}

    public void Play()
    {
        this.gameObject.GetComponent<AudioSource>().Play();
        activated = true;
    }

    //public void Play(float delay)
    //{
    //    StartCoroutine("SetActive", delay);        
    //}

    //IEnumerator SetActive(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //
    //    this.gameObject.GetComponent<AudioSource>().Play();
    //    activated = true;
    //}

    // Update is called once per frame 
    void Update ()
    {
		if(activated && !this.gameObject.GetComponent<AudioSource>().isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
