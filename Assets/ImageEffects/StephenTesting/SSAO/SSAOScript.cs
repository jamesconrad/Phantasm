using UnityEngine;
using System.Collections;

public class SSAOScript : MonoBehaviour
{

    public Material OcclusionMaterial;
    public Material HorizontalBlurMaterial;
    public Material VerticalBlurMaterial;
    public Material CompositeMaterial;

    public float Gamma;
    public float Contrast;

    //[Range(1, 4)]
    //int NumberOfBlurSamples;

    private RenderTexture occlusionTexture;
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
        occlusionTexture = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
        blurTextureX2 = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
        blurTextureY2 = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
        CompositeMaterial.SetTexture("Blur4Tex", blurTextureY2);
    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        OcclusionMaterial.SetFloat("GammaA", Gamma);
        OcclusionMaterial.SetFloat("GammaY", Contrast);
        Graphics.Blit(source, occlusionTexture, OcclusionMaterial);

        HorizontalBlurMaterial.SetFloat("PixelSize", (1.0f / Screen.width) * HorizontalStretch);
        VerticalBlurMaterial.SetFloat("PixelSize", (1.0f / Screen.height) * VerticalStretch);

        Graphics.Blit(occlusionTexture, blurTextureX2, HorizontalBlurMaterial);

        Graphics.Blit(blurTextureX2, blurTextureY2, VerticalBlurMaterial);

        Graphics.Blit(blurTextureY2, blurTextureX4, VerticalBlurMaterial);

        Graphics.Blit(blurTextureX4, blurTextureY4, HorizontalBlurMaterial);

        Graphics.Blit(blurTextureY4, blurTextureX2, HorizontalBlurMaterial);

        Graphics.Blit(blurTextureX2, blurTextureY2, VerticalBlurMaterial);

        //CompositeMaterial.SetTexture("Blur4Tex", blurTextureY2);
        Graphics.Blit(source, destination, CompositeMaterial);
    }
}