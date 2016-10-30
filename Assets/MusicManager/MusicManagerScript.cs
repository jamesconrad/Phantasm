using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicManagerScript : MonoBehaviour
{
    public AudioMixerSnapshot lowIntensity;
    public AudioMixerSnapshot HighIntensity;
    public AudioClip[] musicChannels;
    public AudioSource[] musicSource;
    public GameObject AgentObject;
    public GameObject[] PhantomObject;

    float intensity = 0.3f;

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
        AgentObject = GameObject.Find("Agent(Clone)");
        PhantomObject[0] = GameObject.Find("Phantom(Clone)");

        if (AgentObject != null)
        {
            float closestPhantom = Vector3.Distance(AgentObject.transform.position, PhantomObject[0].transform.position);
            for (int i = 1; i < PhantomObject.Length; ++i)
            {
                closestPhantom = Mathf.Min(closestPhantom, Vector3.Distance(AgentObject.transform.position, PhantomObject[i].transform.position));
            }

            //intensity = -Mathf.Cos(Time.fixedTime * 0.05f) * 0.5f + 0.5f;
            float intensityOld = intensity;
            intensity = Mathf.InverseLerp(50.0f, 2.0f, closestPhantom);
            if (intensity > intensityOld)
            {
                intensity = Mathf.Lerp(intensityOld, intensity, 0.1f);
            }
            else
            {
                intensity = Mathf.Lerp(intensityOld, intensity, 0.0025f);
            }
        }
        else
        {
            intensity = 0.0f;
        }
        
       
       

        float intensityAdjust = intensity * musicChannels.Length;
        for (int i = 0; i < musicChannels.Length; ++i)
        {
            float intensityAdjustClamp = Mathf.Clamp(intensityAdjust - i, 0.0f, 1.0f);
            
            musicSource[i].volume = intensityAdjustClamp;
        }

    }
}
