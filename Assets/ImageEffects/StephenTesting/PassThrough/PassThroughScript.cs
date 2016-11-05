using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class PassThroughScript : MonoBehaviour
{
    public Material testMaterial;
    //public Vector2 WaveCount = new Vector2(40.0f, 40.0f);
    //public Vector2 WaveIntensity = new Vector2(0.01f, 0.01f);
    //public Vector2 WaveTimeMult = new Vector2(1.0f, 1.0f);

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //testMaterial.SetVector("uWaveCount", WaveCount);
        //testMaterial.SetVector("uWaveIntensity", WaveIntensity);
        //testMaterial.SetVector("uTime", WaveTimeMult * Time.time); 
        Graphics.Blit(source, destination, testMaterial);
    }
}
