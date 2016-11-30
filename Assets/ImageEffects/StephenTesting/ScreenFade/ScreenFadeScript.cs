using UnityEngine;
using System.Collections;

public class ScreenFadeScript : MonoBehaviour
{
	public Material material;
	public Texture fadeTexture;

	public enum FadeType
	{
		Disabled,
		FadeIn,
		FadeOut
	}

	public FadeType fadeActive = FadeType.Disabled;

	float fadeAmount = 1.0f;
	public float fadeSpeed = 1.0f;

	public void StartFadeIn(float delay = 0.0f)
	{
		fadeActive = FadeType.FadeIn;
		fadeAmount = 1.0f + delay;
	}

	public void StartFadeOut(float delay = 0.0f)
	{
		fadeActive = FadeType.FadeOut;
		fadeAmount = -delay;
	}

	public void StopFade()
	{
		fadeActive = FadeType.Disabled;
		fadeAmount = 0.0f;
	}

	public void Awake()
	{
		switch(fadeActive)
		{
			case FadeType.Disabled:
				fadeAmount = 0.0f;
				break;
			case FadeType.FadeIn:
				fadeAmount = 1.0f;
				break;
			case FadeType.FadeOut:
				fadeAmount = 0.0f;
				break;
			default:
				StopFade();
				break;
		}
	}

	void Update()
	{
		if (fadeActive == FadeType.FadeOut)
		{
			fadeAmount = Mathf.Min(1.0f, fadeAmount + fadeSpeed * Time.deltaTime);
		}

		if (fadeActive == FadeType.FadeIn)
		{
			fadeAmount = Mathf.Max(0.0f, fadeAmount - fadeSpeed * Time.deltaTime);
		}
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Amount", Mathf.Clamp(fadeAmount, 0.0f, 1.0f));
		material.SetTexture("_FadeTex", fadeTexture);
		Graphics.Blit(source, destination, material);
	}
}
