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
    public Color ambientLight = new Color(0.25f, 0.25f, 0.25f);

    public float filmGrainAmount = 0.3f;

    public bool NightVisionActive = true;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NightVisionActive = !NightVisionActive;
        }

    }

    public void OnPreRender()
    {
        if (NightVisionActive)
        {
            //RenderSettings.ambientSkyColor = Color.white;
            //RenderSettings.ambientIntensity = 1.0f;
            ambientLightTemp = RenderSettings.ambientLight;
            RenderSettings.ambientLight = ambientLight; //Color.gray;
        } 
    }

    public void OnPostRender()
    {
    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (NightVisionActive)
        {
            RenderSettings.ambientLight = ambientLightTemp;

            float RandomNum = Random.Range(0.0f, 1.0f);
            effectMaterial.SetFloat("RandomNumber", RandomNum);
            effectMaterial.SetFloat("uAmount", filmGrainAmount);
            effectMaterial.SetFloat("uLightMult", 1.2f);
            

            Graphics.Blit(source, destination, effectMaterial);
        }
    }
}