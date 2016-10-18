using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class CAScript : MonoBehaviour
{

    public Material effectMaterial;

    //private Color ambientSkyColorTemp;
    //private float ambientIntensityTemp;
    private Color ambientLightTemp;

    public float filmGrainAmount = 0.3f;

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //float RandomNum = Random.Range(0.0f, 1.0f);
        //effectMaterial.SetFloat("RandomNumber", RandomNum);
        //effectMaterial.SetFloat("uAmount", filmGrainAmount);

        Graphics.Blit(source, destination, effectMaterial);
    }
}
