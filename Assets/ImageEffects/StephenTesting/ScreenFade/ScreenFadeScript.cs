using UnityEngine;
using System.Collections;

public class ScreenFadeScript : MonoBehaviour
{
	public Material material;
	public Texture fadeTexture;

	bool fadeActive = false;
	float fadeAmount = 0.0f;
	float fadeSpeed = 1.01f;

	public void StartFade()
	{
		fadeActive = true;
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(fadeActive)
		{
			fadeAmount += Mathf.Min(1.0f, fadeSpeed * Time.deltaTime);
		}
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Amount", fadeAmount);
		material.SetTexture("_FadeTex", fadeTexture);
		Graphics.Blit(source, destination, material);
	}
}
