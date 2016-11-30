using UnityEngine;
using System.Collections;

public class AmbientLightSetScript : MonoBehaviour 
{
	public Color ambientLight;
	
	// Use this for initialization
	void Start () 
	{
		RenderSettings.ambientLight = ambientLight;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
