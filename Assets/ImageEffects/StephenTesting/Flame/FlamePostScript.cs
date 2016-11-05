using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class FlamePostScript : MonoBehaviour
{
    public Vector4 uRGB;
    public Vector4 uColAdd;

    public Vector4 uNewRGB;
    public Vector4 uNewColAdd;

    public Material effectMaterial;

    float lerpInterp = 1.0f;

    public void Start()
    {
        uRGB = new Vector4(0.333f, 0.500f, -0.150f, 0.0f);
        uColAdd = new Vector4(0.100f, 0.000f, -0.050f, 0.0f);

        uNewRGB = Random.ColorHSV();
        uNewColAdd = Random.ColorHSV();
    }

    public void Update()
    {
        lerpInterp += Time.deltaTime;

        if (lerpInterp > 1.0f)
        {
            lerpInterp = 0.0f;

            uRGB = uNewRGB;
            uColAdd = uNewColAdd;

            //Random.ColorHSV(0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f)
            //                              Hue         Sat          Value       Alpha    
            uNewRGB =       Random.ColorHSV(0.0f, 1.0f, 0.25f, 1.0f, 0.5f, 1.00f, 1.0f, 1.0f);
            uNewColAdd =    Random.ColorHSV(0.0f, 1.0f, 0.25f, 1.0f, 0.0f, 0.25f, 1.0f, 1.0f);
        }
    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        effectMaterial.SetVector("uRGB", Color.Lerp(uRGB, uNewRGB, Mathf.SmoothStep(0.0f, 1.0f, lerpInterp)));
        effectMaterial.SetVector("uColAdd", Color.Lerp(uColAdd, uNewColAdd, Mathf.SmoothStep(0.0f, 1.0f, lerpInterp)));
        Graphics.Blit(source, destination, effectMaterial);
    }
}
