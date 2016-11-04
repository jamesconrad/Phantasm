using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class FilmGrainScript : MonoBehaviour
{
    public Material effectMaterial;

    [Range(0.0f, 1.0f)]
    public float filmGrainAmount = 0.3f;
    
    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        float RandomNum = Random.Range(0.0f, 1.0f);
        effectMaterial.SetFloat("RandomNumber", RandomNum);
        effectMaterial.SetFloat("uAmount", filmGrainAmount);



        Graphics.Blit(source, destination, effectMaterial);
    }
}