using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class Bloom : MonoBehaviour
{

    public Material ToneMapMaterial;
    public Material HorizontalBlurMaterial;
    public Material VerticalBlurMaterial;
    public Material CompositeMaterial;

    public float Gamma;
    public float Contrast;

    //[Range(1, 4)]
    //int NumberOfBlurSamples;
    
    private RenderTexture toneMapTexture;
    private RenderTexture blurTextureX2;
    private RenderTexture blurTextureY2;
    private RenderTexture blurTextureX4;
    private RenderTexture blurTextureY4;

    [Range(0.1f, 5.0f)]
    public float HorizontalStretch = 1.0f;
    [Range(0.1f, 5.0f)]
    public float VerticalStretch = 1.0f;

    // Use this for initialization
    void Start()
    {
        toneMapTexture = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
        blurTextureX2 = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
        blurTextureY2 = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
        blurTextureX4 = new RenderTexture(Screen.width / 4, Screen.height / 4, 0, RenderTextureFormat.ARGBHalf);
        blurTextureY4 = new RenderTexture(Screen.width / 4, Screen.height / 4, 0, RenderTextureFormat.ARGBHalf);
        CompositeMaterial.SetTexture("Blur4Tex", blurTextureY2);
    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        ToneMapMaterial.SetFloat("GammaA", Gamma);
        ToneMapMaterial.SetFloat("GammaY", Contrast);
        Graphics.Blit(source, toneMapTexture, ToneMapMaterial);

        HorizontalBlurMaterial.SetFloat("PixelSize", (1.0f / Screen.width) * HorizontalStretch);
        VerticalBlurMaterial.SetFloat("PixelSize", (1.0f / Screen.height) * VerticalStretch);
        
        Graphics.Blit(toneMapTexture, blurTextureX2, HorizontalBlurMaterial);

        Graphics.Blit(blurTextureX2, blurTextureY2, VerticalBlurMaterial);

        Graphics.Blit(blurTextureY2, blurTextureX4, VerticalBlurMaterial);

        Graphics.Blit(blurTextureX4, blurTextureY4, HorizontalBlurMaterial);

        Graphics.Blit(blurTextureY4, blurTextureX2, HorizontalBlurMaterial);

        Graphics.Blit(blurTextureX2, blurTextureY2, VerticalBlurMaterial);

        //CompositeMaterial.SetTexture("Blur4Tex", blurTextureY2);
        Graphics.Blit(source, destination, CompositeMaterial);
    }
}
