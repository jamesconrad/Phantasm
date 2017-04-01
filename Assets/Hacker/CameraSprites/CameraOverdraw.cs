using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOverdraw : MonoBehaviour 
{
	public Shader shader;
	public Color color;
	Camera reference;

	Vector2 cameraSkew = new Vector2(-0.01f, -0.01f);

	GameObject agent;
	// Use this for initialization
	void Start () 
	{
		reference = this.GetComponent<Camera>();
		reference.SetReplacementShader(shader, null);
		Shader.SetGlobalColor("uColor", color);

		SetObliqueness(cameraSkew);
		agent = GameObject.FindGameObjectWithTag("Player");
	}
	
	void SetObliqueness(Vector2 skew) 
	{
        Matrix4x4 mat  = reference.projectionMatrix;
        mat[0, 2] = skew.x;
        mat[1, 2] = skew.y;
        reference.projectionMatrix = mat;
    }

	void SetObliqueness(float horizObl, float vertObl) 
	{
        Matrix4x4 mat  = reference.projectionMatrix;
        mat[0, 2] = horizObl;
        mat[1, 2] = vertObl;
        reference.projectionMatrix = mat;
    }

	const float maxTimeToCameraUpdate = 0.10f; 
	float timeSinceCameraUpdate = maxTimeToCameraUpdate;
	const float maxTimeToUpdate = 1.0f; 
	float timeSinceUpdate = maxTimeToUpdate;

	// Update is called once per frame
	void Update () 
	{		
		if(PhaNetworkManager.characterSelection == 1)
		{
		timeSinceUpdate += Time.deltaTime;
		if(timeSinceUpdate > maxTimeToUpdate)
		{
			timeSinceUpdate = 0.0f;
			agent = GameObject.FindGameObjectWithTag("Player");
		}

		if(Input.mouseScrollDelta.y > 0)
		{
			reference.orthographicSize *= 0.9f;
			reference.ResetProjectionMatrix();
			SetObliqueness(cameraSkew);
			
			//timeSinceCameraUpdate = maxTimeToCameraUpdate;
			reference.Render();
		}
		if(Input.mouseScrollDelta.y < 0)
		{
			reference.orthographicSize /= 0.9f;
			reference.ResetProjectionMatrix();
			SetObliqueness(cameraSkew);
			//timeSinceCameraUpdate = maxTimeToCameraUpdate;
			reference.Render();
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
}
