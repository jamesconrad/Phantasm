using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class SobelScript : MonoBehaviour
{
    public Shader shader;
    Material material;

	// Use this for initialization
	void Start ()
    {
		material = new Material(shader);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }


}
