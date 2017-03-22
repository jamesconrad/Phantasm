using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOverdraw : MonoBehaviour 
{
	public Shader shader;
	public Color color;
	Camera reference;

	GameObject agent;
	// Use this for initialization
	void Start () 
	{
		reference = this.GetComponent<Camera>();
		reference.SetReplacementShader(shader, null);
		Shader.SetGlobalColor("uColor", color);

		SetObliqueness(-0.02f, 0.02f);
		agent = GameObject.FindGameObjectWithTag("Player");
	}
	
	void SetObliqueness(float horizObl, float vertObl) {
        Matrix4x4 mat  = reference.projectionMatrix;
        mat[0, 2] = horizObl;
        mat[1, 2] = vertObl;
        reference.projectionMatrix = mat;
    }

	const float maxTimeToCameraUpdate = 1.0f; 
	float timeSinceCameraUpdate = maxTimeToCameraUpdate;
	const float maxTimeToUpdate = 1.0f; 
	float timeSinceUpdate = maxTimeToUpdate;

	// Update is called once per frame
	void Update () 
	{		
		timeSinceUpdate += Time.deltaTime;
		if(timeSinceUpdate > maxTimeToUpdate)
		{
			timeSinceUpdate = 0.0f;
			agent = GameObject.FindGameObjectWithTag("Player");
		}


        timeSinceCameraUpdate += Time.deltaTime;
		if(timeSinceCameraUpdate > maxTimeToCameraUpdate)
		{
			timeSinceCameraUpdate = 0.0f;
			reference.Render();

            if(agent != null)
			reference.transform.position = agent.transform.position;
		}

		
		
	}
}
