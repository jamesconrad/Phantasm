using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicManagerScript : MonoBehaviour
{
    public AudioMixerSnapshot lowIntensity;
    public AudioMixerSnapshot HighIntensity;
    public AudioClip[] musicChannels;
    public AudioSource[] musicSource;

    float intensity = 1.0f;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < musicChannels.Length; ++i)
        { 
            musicSource[i].clip = musicChannels[i];
            musicSource[i].Play();
            musicSource[i].loop = true;

            //musicSource[i].volume = 0.0f;
        }
        //SpookyChip
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        intensity = -Mathf.Cos(Time.fixedTime * 0.05f) * 0.5f + 0.5f;
        float intensityAdjust = intensity * musicChannels.Length;
        for (int i = 0; i < musicChannels.Length; ++i)
        {
            float intensityAdjustClamp = Mathf.Clamp(intensityAdjust - i, 0.0f, 1.0f);
            
            musicSource[i].volume = intensityAdjustClamp;
        }

    }
}
