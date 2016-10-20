using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class SquashAndStretch : MonoBehaviour {

    public bool isPlaying;
    private Animation anim;
    public AnimationClip SASClip;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animation>();
        SASClip.legacy = true;
        anim.clip = SASClip;
        anim.AddClip(SASClip, SASClip.name);
	}
	
	// Update is called once per frame
	void Update () {
        if (isPlaying)
        {
            anim.Play(SASClip.name);
        }
	}
}
