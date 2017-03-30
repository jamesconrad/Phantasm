using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicManagerScript : MonoBehaviour
{
    public AudioMixerSnapshot lowIntensity;
    public AudioMixerSnapshot HighIntensity;
    public AudioClip[] musicChannels;
    public AudioSource[] musicSource;
    public AudioClip musicDeathChannel;
    public AudioSource musicDeathSource;
    public GameObject AgentObject;
    public GameObject[] PhantomObject;

    float maxVolume = 0.25f;

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

        musicDeathSource.clip = musicDeathChannel;
        musicDeathSource.Play();
        musicDeathSource.loop = true;
        musicDeathSource.volume = 0.0f;
        //SpookyChip
    }

    const float timeToWaitTillSearch = 10.0f;
    float timeSinceLastSearch = timeToWaitTillSearch;

    float minDistance = 1.0f;
    float maxDistance = 25.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        //AgentObject = GameObject.FindGameObjectWithTag("Player");
        //PhantomObject = GameObject.FindGameObjectsWithTag("Enemy");

         timeSinceLastSearch += Time.fixedDeltaTime;
        if(timeSinceLastSearch > timeToWaitTillSearch)
        {
            AgentObject = GameObject.FindGameObjectWithTag("Player");
            PhantomObject = GameObject.FindGameObjectsWithTag("Enemy");
            timeSinceLastSearch = 0.0f;
        }

        if (AgentObject != null)
        {
            Health playerScript = AgentObject.GetComponent<Health>();
            float deathAmount = Mathf.InverseLerp(100.0f, 0.0f, playerScript.currentHealth);

            musicDeathSource.volume = deathAmount;

            float closestPhantom = 100000.0f;// = Vector3.Distance(AgentObject.transform.position, PhantomObject[0].transform.position);
            for (int i = 1; i < PhantomObject.Length; ++i)
            {
                closestPhantom = Mathf.Min(closestPhantom, Vector3.Distance(AgentObject.transform.position, PhantomObject[i].transform.position));
            }

            //intensity = -Mathf.Cos(Time.fixedTime * 0.05f) * 0.5f + 0.5f;
            float intensityOld = intensity;
            intensity = Mathf.InverseLerp(maxDistance, minDistance, closestPhantom);
            intensity *= intensity;
            if (intensity > intensityOld)
            {
                intensity = Mathf.Lerp(intensityOld, intensity, 0.1f);
            }
            else
            {
                intensity = Mathf.Lerp(intensityOld, intensity, 0.0125f);
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
            
            musicSource[i].volume = intensityAdjustClamp * maxVolume;
        }


    }
}
