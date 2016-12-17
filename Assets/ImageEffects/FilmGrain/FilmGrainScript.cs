using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class FilmGrainScript : MonoBehaviour
{
    public Material effectMaterial;

    [Range(0.0f, 1.0f)]
    public float filmGrainAmount = 0.3f;

    private float filmGrainBarrel = 1.0f;

    
    private Vector4 param;
    public Vector2 jitterAmount = new Vector2(4.0f, 4.0f);    
    [Range(0.0f, 1.0f)]
    public float jitterBias = 1.0f;
    
    public bool jitterActive =      true;
    public bool faceActive =        false;
    public bool scrollingActive =   false;
    public bool multActive =        false;
    public bool movieActive =       false;

    public void Update()
    {
        if (jitterActive)
            Shader.EnableKeyword("SAT_GRAIN_JITTER");
        else
            Shader.DisableKeyword("SAT_GRAIN_JITTER");
        if(faceActive)
            Shader.EnableKeyword("SAT_GRAIN_FACE");
        else
            Shader.DisableKeyword("SAT_GRAIN_FACE");
        if(scrollingActive)
            Shader.EnableKeyword("SAT_GRAIN_SCROLLING");
        else
            Shader.DisableKeyword("SAT_GRAIN_SCROLLING");
        if(multActive)
            Shader.EnableKeyword("SAT_GRAIN_MULT");
        else
            Shader.DisableKeyword("SAT_GRAIN_MULT");
        if(movieActive)
            Shader.EnableKeyword("SAT_GRAIN_MOVIE");
        else
            Shader.DisableKeyword("SAT_GRAIN_MOVIE");

    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        param.x = Random.Range(0.0f, 1.0f);
        param.y = Random.Range(0.0f, 1.0f);
        param.z = jitterAmount.x * jitterBias;
        param.w = jitterAmount.y * jitterBias;

        effectMaterial.SetVector("jitterParam", param);


        float RandomNum = Random.Range(0.0f, 1.0f);
        effectMaterial.SetFloat("RandomNumber", RandomNum);
        effectMaterial.SetFloat("uAmount", filmGrainAmount);        
        effectMaterial.SetFloat("uDistortion", filmGrainBarrel); 
        effectMaterial.SetVector("uOffsetAmount", Vector2.zero);
        
        Graphics.Blit(source, destination, effectMaterial);
    }
}