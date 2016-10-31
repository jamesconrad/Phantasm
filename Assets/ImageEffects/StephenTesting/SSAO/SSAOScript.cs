using UnityEngine;
using System.Collections;

public class SSAOScript : MonoBehaviour
{
    bool setUpSSAOKernel = false;
    int numOfSSAOSamples = 16;

    public Camera mainCameraSettings;

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

    Matrix4x4 ProjBiasMatrix = new Matrix4x4();

    // Use this for initialization
    void Start()
    {
        ProjBiasMatrix.SetRow(0, new Vector4(2.0f, 0.0f, 0.0f, -1.0f));
        ProjBiasMatrix.SetRow(1, new Vector4(0.0f, 2.0f, 0.0f, -1.0f));
        ProjBiasMatrix.SetRow(2, new Vector4(0.0f, 0.0f, 2.0f, -1.0f));
        ProjBiasMatrix.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f,  1.0f));

        occlusionTexture = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.RHalf);
        blurTextureX2 = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.RHalf);
        blurTextureY2 = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.RHalf);
        CompositeMaterial.SetTexture("_OcclusionTex", blurTextureY2);
    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        OcclusionMaterial.SetFloat("GammaA", Gamma);
        OcclusionMaterial.SetFloat("GammaY", Contrast);

        Matrix4x4 inverseMatrix = Matrix4x4.Inverse(mainCameraSettings.projectionMatrix) * ProjBiasMatrix;
        OcclusionMaterial.SetMatrix("uProjBiasMatrixInverse", inverseMatrix);

        //Graphics.Blit(source, occlusionTexture, OcclusionMaterial);
        Graphics.Blit(source, destination, OcclusionMaterial);
        
        //HorizontalBlurMaterial.SetFloat("PixelSize", (1.0f / Screen.width) * HorizontalStretch);
        //VerticalBlurMaterial.SetFloat("PixelSize", (1.0f / Screen.height) * VerticalStretch);
        //
        //Graphics.Blit(occlusionTexture, blurTextureX2, HorizontalBlurMaterial);
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
        //Graphics.Blit(source, destination, CompositeMaterial);
    }
}