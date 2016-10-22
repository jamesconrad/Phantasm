using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animation))]
public class SquashAndStretch : MonoBehaviour {

    public bool playOnStart;
    public bool loopAnimation;
    private Animation anim;
    private AnimationClip SASClip;


    public CustomKeyFrame[] xScaleKeyframes;
    public CustomKeyFrame[] yScaleKeyframes;
    public CustomKeyFrame[] zScaleKeyframes;
    private AnimationCurve customCurve;


    // Use this for initialization
    void Start () {
        anim = GetComponent<Animation>();
        SASClip = new AnimationClip();
        SASClip.legacy = true;

        if (xScaleKeyframes != null)
        {
            customCurve = BuildAnimationCurve(xScaleKeyframes);
            SASClip.SetCurve("", typeof(Transform), "localScale.x", customCurve);
        }
        if (yScaleKeyframes != null)
        {
            customCurve = BuildAnimationCurve(yScaleKeyframes);
            SASClip.SetCurve("", typeof(Transform), "localScale.y", customCurve);
        }
        if (zScaleKeyframes != null)
        {
            customCurve = BuildAnimationCurve(zScaleKeyframes);
            SASClip.SetCurve("", typeof(Transform), "localScale.z", customCurve);
        }
        
        anim.clip = SASClip;
        anim.AddClip(SASClip, SASClip.name);
        if (playOnStart)
        {
            Play();
        }
    }

    // Update is called once per frame
    void Update () {
        if (!anim.IsPlaying(SASClip.name))
        {
            if (loopAnimation)
            {
                anim.Play(SASClip.name);
            }
        }
	}

    public void Play()
    {
        anim.Play(SASClip.name);
    }

    public void Stop()
    {
        anim.Stop(SASClip.name);
    }






    
    [System.Serializable]
    public struct CustomKeyFrame
    {
        [Tooltip("The time specified for this keyframe")]
        public float time;

        [Tooltip("The value change for the paramater")]
        public float value;

        [Tooltip("The in tangent control for the value point")]
        public float inTangent;

        [Tooltip("The out tangent control for the value point")]
        public float outTangent;

        public Keyframe BuildKeyframe()
        {
            Keyframe frame = new Keyframe(time, value, inTangent, outTangent);
            return frame;
        }
    }

    public AnimationCurve BuildAnimationCurve(CustomKeyFrame[] keyframes)
    {
        AnimationCurve curve = new AnimationCurve();
        for (int i = 0; i < keyframes.Length; i++)
        {
            curve.AddKey(keyframes[i].BuildKeyframe());
        }
        return curve;
    }
}
