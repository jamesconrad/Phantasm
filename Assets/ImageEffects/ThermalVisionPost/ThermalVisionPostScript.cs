using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class ThermalVisionPostScript : MonoBehaviour
{
    public Material effectMaterial;
    public Texture thermalRamp;

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        effectMaterial.SetTexture("ThermalRamp", thermalRamp);
        Graphics.Blit(source, destination, effectMaterial);
    }
}