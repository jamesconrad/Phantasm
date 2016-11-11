using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class CAScript : MonoBehaviour
{

    public Material effectMaterial;
    
    
    [Tooltip("Determines concentration of aberration closer to the center of the screen")]
    [Range(0.1f, 3.0f)]
    public float dispersal = 1.0f;

    [Tooltip("Determines amount of Chromatic Aberration")]
    [Range(0.0f, 0.2f)]
    public float offset = 0.01f;
    
    
    
    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        effectMaterial.SetFloat("_Dispersal", dispersal);
        effectMaterial.SetFloat("_Offset", offset);

        Graphics.Blit(source, destination, effectMaterial);
    }
}
