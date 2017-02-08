using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientLight : MonoBehaviour
{
    public Color ambientLight = new Color(0.0f, 0.0f, 0.0f);


	// Use this for initialization
	void Start ()
    {
        //RenderSettings.ambientLight = new Color(1.0f, 1.0f, 1.0f);

    }

    // Update is called once per frame
    public void OnPreRender()
    {
        //RenderSettings.ambientLight = ambientLight;
        RenderSettings.ambientLight = new Color(1.0f, 1.0f, 1.0f);
    }
}
