using UnityEngine;
using System.Collections;

//[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class Bloom : MonoBehaviour
{

    public Material PassThroughMaterial;
    public Material ToneMapMaterial;
    public Material HorizontalBlurMaterial;
    public Material VerticalBlurMaterial;
    public Material CompositeMaterial;

    //public float Gamma;
    //public float Contrast;

    public float Intensity = 0.5f;
    public float Threshold = 0.5f;

    //[Range(1, 4)]
    //int NumberOfBlurSamples;
    
    //private RenderTexture toneMapTexture;
    private RenderTexture blurTextureX2;
    private RenderTexture blurTextureY2;
    private RenderTexture blurTextureX4;
    private RenderTexture blurTextureY4;
    private RenderTexture blurTextureX8;
    private RenderTexture blurTextureY8;
    private RenderTexture blurTextureX16;
    private RenderTexture blurTextureY16;

    [Range(0.1f, 5.0f)]
    public float HorizontalStretch = 1.0f;
    [Range(0.1f, 5.0f)]
    public float VerticalStretch = 1.0f;

    // Use this for initialization
    void Start()
    {
        //toneMapTexture = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
        blurTextureX2 = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
        blurTextureY2 = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
        blurTextureX4 = new RenderTexture(Screen.width / 4, Screen.height / 4, 0, RenderTextureFormat.ARGBHalf);
        blurTextureY4 = new RenderTexture(Screen.width / 4, Screen.height / 4, 0, RenderTextureFormat.ARGBHalf);
        blurTextureX8 = new RenderTexture(Screen.width / 8, Screen.height / 8, 0, RenderTextureFormat.ARGBHalf);
        blurTextureY8 = new RenderTexture(Screen.width / 8, Screen.height / 8, 0, RenderTextureFormat.ARGBHalf);
        blurTextureX16 = new RenderTexture(Screen.width / 16, Screen.height / 16, 0, RenderTextureFormat.ARGBHalf);
        blurTextureY16 = new RenderTexture(Screen.width / 16, Screen.height / 16, 0, RenderTextureFormat.ARGBHalf);
        CompositeMaterial.SetTexture("Blur1Tex", blurTextureX2);
        CompositeMaterial.SetTexture("Blur2Tex", blurTextureX4);
        CompositeMaterial.SetTexture("Blur3Tex", blurTextureX8);
        CompositeMaterial.SetTexture("Blur4Tex", blurTextureX16);
    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //ToneMapMaterial.SetFloat("GammaA", Gamma);
        //ToneMapMaterial.SetFloat("GammaY", Contrast);
        ToneMapMaterial.SetFloat("uThreshold", Threshold);
        CompositeMaterial.SetFloat("uIntensity", Intensity);
        //ToneMapMaterial.SetFloat("uIntensity", Intensity);
        
        Graphics.Blit(source, blurTextureX2, ToneMapMaterial);
        Graphics.Blit(blurTextureX2, blurTextureX4, PassThroughMaterial);
        Graphics.Blit(blurTextureX4, blurTextureX8, PassThroughMaterial);
        Graphics.Blit(blurTextureX8, blurTextureX16, PassThroughMaterial);

        HorizontalBlurMaterial.SetFloat("uPixelSize", (1.0f / Screen.width) * HorizontalStretch * 2.0f);
        VerticalBlurMaterial.SetFloat("uPixelSize", (1.0f / Screen.height) * VerticalStretch * 2.0f);
        
        Graphics.Blit(blurTextureX2, blurTextureY2, HorizontalBlurMaterial);
        Graphics.Blit(blurTextureY2, blurTextureX2, VerticalBlurMaterial);

        HorizontalBlurMaterial.SetFloat("uPixelSize", (1.0f / Screen.width) * HorizontalStretch * 4.0f);
        VerticalBlurMaterial.SetFloat("uPixelSize", (1.0f / Screen.height) * VerticalStretch * 4.0f);
        Graphics.Blit(blurTextureX4, blurTextureY4, HorizontalBlurMaterial);
        Graphics.Blit(blurTextureY4, blurTextureX4, VerticalBlurMaterial);

        HorizontalBlurMaterial.SetFloat("uPixelSize", (1.0f / Screen.width) * HorizontalStretch * 8.0f);
        VerticalBlurMaterial.SetFloat("uPixelSize", (1.0f / Screen.height) * VerticalStretch * 8.0f);
        Graphics.Blit(blurTextureX8, blurTextureY8, HorizontalBlurMaterial);
        Graphics.Blit(blurTextureY8, blurTextureX8, VerticalBlurMaterial);

        HorizontalBlurMaterial.SetFloat("uPixelSize", (1.0f / Screen.width) * HorizontalStretch * 16.0f);
        VerticalBlurMaterial.SetFloat("uPixelSize", (1.0f / Screen.height) * VerticalStretch * 16.0f);
        Graphics.Blit(blurTextureX16, blurTextureY16, HorizontalBlurMaterial);
        Graphics.Blit(blurTextureY16, blurTextureX16, VerticalBlurMaterial);

        //Graphics.Blit(toneMapTexture, blurTextureX2, HorizontalBlurMaterial);
//
        //Graphics.Blit(blurTextureX2, blurTextureY2, VerticalBlurMaterial);
//
        //Graphics.Blit(blurTextureY2, blurTextureX4, VerticalBlurMaterial);
//
        //Graphics.Blit(blurTextureX4, blurTextureY4, HorizontalBlurMaterial);
//
        //Graphics.Blit(blurTextureY4, blurTextureX2, HorizontalBlurMaterial);
//
        //Graphics.Blit(blurTextureX2, blurTextureY2, VerticalBlurMaterial);

        //CompositeMaterial.SetTexture("Blur4Tex", blurTextureY2);
        
        
        Graphics.Blit(source, destination, CompositeMaterial);
        //Graphics.Blit(blurTextureX8, destination, PassThroughMaterial);
    }
}
