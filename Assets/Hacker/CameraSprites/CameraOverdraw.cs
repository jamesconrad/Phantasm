using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOverdraw : MonoBehaviour 
{
	public Shader shader;
	public Color color;
	Camera reference;
	// Use this for initialization
	void Start () 
	{
		reference = this.GetComponent<Camera>();
		reference.SetReplacementShader(shader, null);
		Shader.SetGlobalColor("uColor", color);
	}
	
	const float maxTimeToUpdate = 1.0f; 
	float timeSinceUpdate = maxTimeToUpdate;

	// Update is called once per frame
	void Update () 
	{		
        timeSinceUpdate += Time.deltaTime;
		if(timeSinceUpdate > maxTimeToUpdate)
		{
			timeSinceUpdate = 0.0f;
			reference.Render();
		}
	}
}
