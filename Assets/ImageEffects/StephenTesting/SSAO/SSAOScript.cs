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

    Vector3[] samples;

    //samples[0] = float3(0.14840700, -0.133314 0.1506750);
    //samples[1] = float3(0.16187000, -0.128777 0.1455610);
    //samples[2] = float3(0.06263370, -0.132009 0.2171340);
    //samples[3] = float3(0.02966160, -0.252805 0.1076520);
    //samples[4] = float3(0.03951750, -0.262670 0.1325810);
    //samples[5] = float3(0.19147700, 0.1764530 0.1915370);
    //samples[6] = float3(-0.1612670, 0.2202880 0.2276490);
    //samples[7] = float3(0.24657500, 0.2411850 0.1895140);
    //samples[8] = float3(-0.0103453, -0.275986 0.3393100);
    //samples[9] = float3(-0.1822100, 0.2695660 0.3627670);
    //samples[10] = float3(-0.0833288, -0.527435 0.0984072);
    //samples[11] = float3(0.18961900, 0.5640350 0.1063950);
    //samples[12] = float3(0.43003300, 0.3541680 0.3755700);
    //samples[13] = float3(0.51916100, 0.2713890 0.4604560);
    //samples[14] = float3(0.76124700, -0.302437 0.0914969);
    //samples[15] = float3(0.33867700, 0.8104540 0.2346690);

    // Use this for initialization
    void Start()
    {
        samples = new Vector3[16];
        samples[ 0] = new Vector3(0.14840700f, -0.133314f, 0.1506750f);
        samples[ 1] = new Vector3(0.16187000f, -0.128777f, 0.1455610f);
        samples[ 2] = new Vector3(0.06263370f, -0.132009f, 0.2171340f);
        samples[ 3] = new Vector3(0.02966160f, -0.252805f, 0.1076520f);
        samples[ 4] = new Vector3(0.03951750f, -0.262670f, 0.1325810f);
        samples[ 5] = new Vector3(0.19147700f, 0.1764530f, 0.1915370f);
        samples[ 6] = new Vector3(-0.1612670f, 0.2202880f, 0.2276490f);
        samples[ 7] = new Vector3(0.24657500f, 0.2411850f, 0.1895140f);
        samples[ 8] = new Vector3(-0.0103453f, -0.275986f, 0.3393100f);
        samples[ 9] = new Vector3(-0.1822100f, 0.2695660f, 0.3627670f);
        samples[10] = new Vector3(-0.0833288f, -0.527435f, 0.0984072f);
        samples[11] = new Vector3(0.18961900f, 0.5640350f, 0.1063950f);
        samples[12] = new Vector3(0.43003300f, 0.3541680f, 0.3755700f);
        samples[13] = new Vector3(0.51916100f, 0.2713890f, 0.4604560f);
        samples[14] = new Vector3(0.76124700f, -0.302437f, 0.0914969f);
        samples[15] = new Vector3(0.33867700f, 0.8104540f, 0.2346690f);

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
        //OcclusionMaterial.SetFloat("GammaA", Gamma);
        //OcclusionMaterial.SetFloat("GammaY", Contrast);
        mainCameraSettings.depthTextureMode = DepthTextureMode.DepthNormals;
        Matrix4x4 inverseMatrix = Matrix4x4.Inverse(mainCameraSettings.projectionMatrix) * ProjBiasMatrix;
        OcclusionMaterial.SetMatrix("uProjBiasMatrixInverse", inverseMatrix);
        OcclusionMaterial.SetMatrix("uProj", mainCameraSettings.projectionMatrix);

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