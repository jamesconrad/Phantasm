using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class HueShiftEfficientScript : MonoBehaviour {

    public Material effectMaterial;

    public void Update()
    {
    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //effectMaterial.SetFloat("uHue", Time.time * 10.0f);
        Graphics.Blit(source, destination, effectMaterial);
    }
}
