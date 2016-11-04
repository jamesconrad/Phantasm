using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class RGBSplitScript : MonoBehaviour
{

    public Material effectMaterial;
    
    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        effectMaterial.SetFloat("PixelSizeX", (1.0f / Screen.width));
        effectMaterial.SetFloat("PixelSizeY", (1.0f / Screen.height));

        float[] _rOffset = { -0.005f, -0.005f };

        float[] _gOffset = { 0.0f, 0.0f }; 

        float[] _bOffset = { 0.005f, 0.005f };

        effectMaterial.SetFloat("_rOffsetX", _rOffset[0]);
        effectMaterial.SetFloat("_gOffsetX", _gOffset[0]);
        effectMaterial.SetFloat("_bOffsetX", _bOffset[0]);
        effectMaterial.SetFloat("_rOffsetY", _rOffset[1]);
        effectMaterial.SetFloat("_gOffsetY", _gOffset[1]);
        effectMaterial.SetFloat("_bOffsetY", _bOffset[1]);

        float RandomNum = Random.Range(0.0f, 1.0f);
        effectMaterial.SetFloat("RandomNumber", RandomNum);

        Graphics.Blit(source, destination, effectMaterial);
    }
}
