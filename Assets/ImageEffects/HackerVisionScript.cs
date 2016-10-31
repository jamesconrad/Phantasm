using UnityEngine;
using System.Collections;

public class HackerVisionScript : MonoBehaviour
{
    public Material nightVisionMaterial;
    public Material thermalMaterial;
    public Material filmGrainMaterial;
    public Texture thermalRamp;


    
    private Color ambientLightTemp;
    public Color ambientLight = new Color(0.25f, 0.25f, 0.25f);

    [Range(0.0f, 1.0f)]
    public float filmGrainNightVisionAmount = 0.3f;

    [Range(0.0f, 1.0f)]
    public float filmGrainNormalAmount = 0.05f;

    public enum HackerVisionMode { Normal, Night, Thermal, Last };
    public HackerVisionMode Vision = HackerVisionMode.Normal;
    
    RenderTexture temp = new RenderTexture(Screen.width, Screen.height, 0);

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Vision++;
            if (Vision == HackerVisionMode.Last)
                Vision = HackerVisionMode.Normal;
        }
    }

    public void OnPreRender()
    {
        if (Vision == HackerVisionMode.Night)
        {
            ambientLightTemp = RenderSettings.ambientLight;
            RenderSettings.ambientLight = ambientLight; //Color.gray;
        }
    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderSettings.ambientLight = ambientLightTemp;



        float RandomNumGrain = Random.Range(0.0f, 1.0f);
        filmGrainMaterial.SetFloat("RandomNumber", RandomNumGrain);
        filmGrainMaterial.SetFloat("uAmount", filmGrainNormalAmount);

        if (Vision == HackerVisionMode.Thermal)
        {
            thermalMaterial.SetTexture("ThermalRamp", thermalRamp);
            Graphics.Blit(source, temp, thermalMaterial);

            Graphics.Blit(temp, destination, filmGrainMaterial);
        }
        else if (Vision == HackerVisionMode.Night)
        {

            float RandomNum = Random.Range(0.0f, 1.0f);
            nightVisionMaterial.SetFloat("RandomNumber", RandomNum);
            nightVisionMaterial.SetFloat("uAmount", filmGrainNightVisionAmount);
            nightVisionMaterial.SetFloat("uLightMult", 1.2f);

            Graphics.Blit(source, destination, nightVisionMaterial);
        }
        if (Vision == HackerVisionMode.Normal)
        {
            
            Graphics.Blit(source, destination, filmGrainMaterial);
        }
    }
}
