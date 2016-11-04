using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class SonarScript : MonoBehaviour
{
    public Material sonarMaterial;

    public Camera mainCameraSettings;

    Matrix4x4 ProjBiasMatrix = new Matrix4x4 ( );

    //[Range(0.0f, 1.0f)]
    //public float filmGrainAmount = 0.3f;
    
    void Start()
    {
        ProjBiasMatrix.SetRow(0, new Vector4(2.0f, 0.0f, 0.0f, -1.0f));
        ProjBiasMatrix.SetRow(1, new Vector4(0.0f, 2.0f, 0.0f, -1.0f));
        ProjBiasMatrix.SetRow(2, new Vector4(0.0f, 0.0f, 2.0f, -1.0f));
        ProjBiasMatrix.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f,  1.0f));
    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //float RandomNum = Random.Range(0.0f, 1.0f);
        //sonarMaterial.SetFloat("RandomNumber", RandomNum);
        //sonarMaterial.SetFloat("uAmount", filmGrainAmount);
        //sonarMaterial.SetFloat("uTime", );
        // (0.0f, 0.5f, 1.0f, 0.5f);
        // "uColorAdd"

        sonarMaterial.SetVector("uColorAdd", new Vector4(1.0f, 1.0f, 1.0f, Time.time));
        sonarMaterial.SetVector("uParameter", new Vector4(0.02f, 1.0f, 0.0f, 0.0f));
        //0.02f;
        //0.0f;
        //0.0f;
        //0.0f;



        //sonarMaterial.SetTexture("_DepthTex", source);
        Matrix4x4 inverseMatrix = Matrix4x4.Inverse(mainCameraSettings.projectionMatrix) * ProjBiasMatrix;
        sonarMaterial.SetMatrix("uProjBiasMatrixInverse", inverseMatrix);
        Graphics.Blit(source, destination, sonarMaterial);

        
    }
}