using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class ScreenAdjustmentScript : MonoBehaviour
{
    Material material;
    public Shader shader;
    
    [Range(-1.0f, 1.0f)]
    public float brightness = 0.0f;
    [Range(-1.0f, 4.0f)]
    public float contrast = 1.0f;
    [Range(0.0625f, 10.0f)]
    public float gamma = 1.0f;
    //[Range(-1.0f, 4.0f)]
    private float something = 1.0f;

    [Range(-1.0f, 1.0f)]
    public float minInput = 0.0f;
    [Range(0.0f, 2.0f)]
    public float maxInput = 1.0f;
    [Range(-1.0f, 1.0f)]
    public float minOutput = 0.0f;
    [Range(0.0f, 2.0f)]
    public float maxOutput = 1.0f;

    // Use this for initialization
    void Start()
    {
        material = new Material(shader);

		if(!Plasma.ProjectorMode.active)
		{
			this.enabled = false;
		}
    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetVector("uParam", new Vector4(brightness, contrast, gamma, something));
        material.SetVector("uInputOutput", new Vector4(minInput, maxInput, minOutput, maxOutput));


        Graphics.Blit(source, destination, material);
    }
}
