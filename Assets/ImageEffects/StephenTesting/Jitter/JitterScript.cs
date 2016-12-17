using UnityEngine;
using System.Collections;

public class JitterScript : MonoBehaviour 
{
    public Material imageEffect;
    public Vector2 jitterAmount = new Vector2(4.0f, 4.0f);
    
    [Range(0.0f, 1.0f)]
    public float jitterBias = 1.0f;

    Vector4 param;

	void Start () 
	{
	
	}
	
	void Update () 
	{
	    
	}

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        param.x = Random.Range(0.0f, 1.0f);
        param.y = Random.Range(0.0f, 1.0f);
        param.z = jitterAmount.x * jitterBias;
        param.w = jitterAmount.y * jitterBias;

        imageEffect.SetVector("param", param);
        Graphics.Blit(source, destination, imageEffect);
        //float2 jitterAmount;
		//float2 RandomNumber;
    }


}
