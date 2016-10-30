using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class ThermalVisionPostScript : MonoBehaviour
{
    public Material effectMaterial;
    public Texture thermalRamp;

    public bool ThermalVisionActive = false;

    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ThermalVisionActive = !ThermalVisionActive;
        }

    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (ThermalVisionActive)
        { 
            effectMaterial.SetTexture("ThermalRamp", thermalRamp);
            Graphics.Blit(source, destination, effectMaterial);
        }
    }
}