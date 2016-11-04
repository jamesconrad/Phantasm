using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class NightVisionScript : MonoBehaviour
{
    public Material effectMaterial;

    public float filmGrainAmount = 0.3f;

    public void OnPreRender()
    {
        
        RenderSettings.ambientSkyColor = Color.white;
        RenderSettings.ambientIntensity = 60.5f;
        RenderSettings.ambientLight = Color.gray;
    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        float RandomNum = Random.Range(0.0f, 1.0f);
        effectMaterial.SetFloat("RandomNumber", RandomNum);
        effectMaterial.SetFloat("uAmount", filmGrainAmount);

        Graphics.Blit(source, destination, effectMaterial);
    }
}