using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class NightVisionScript : MonoBehaviour
{
    public Material effectMaterial;

    //private Color ambientSkyColorTemp;
    //private float ambientIntensityTemp;
    private Color ambientLightTemp;

    public float filmGrainAmount = 0.3f;

    public void OnPreRender()
    {

        //RenderSettings.ambientSkyColor = Color.white;
        //RenderSettings.ambientIntensity = 1.0f;
        ambientLightTemp = RenderSettings.ambientLight;
        RenderSettings.ambientLight = new Color(0.25f, 0.25f, 0.25f); //Color.gray; 
    }

    public void OnPostRender()
    {
    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderSettings.ambientLight = ambientLightTemp;

        float RandomNum = Random.Range(0.0f, 1.0f);
        effectMaterial.SetFloat("uRandom", RandomNum);
        effectMaterial.SetFloat("uAmount", filmGrainAmount);

        Graphics.Blit(source, destination, effectMaterial);
    }
}