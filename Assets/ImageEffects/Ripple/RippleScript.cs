using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class RippleScript : MonoBehaviour
{

    public Shader shader;
    public Texture rippleTexture;

    Material effectMaterial;

    float time = 0.0f;
    float speed = 0.5f;
    bool active = false;
    float length = 0.25f;

    public Vector4 screenLocation = new Vector4(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f, 0.0f);

    float amount = 0.0f;
    float maxAmount = 0.125f;

    // Use this for initialization
    void Start ()
    {
        effectMaterial = new Material(shader);
    }
	
    public void setScreenLocation(GameObject location)
    {
        setScreenLocation(location.transform.position);
    }

    public void setScreenLocation(Vector3 position)
    {
        screenLocation = this.GetComponent<Camera>().WorldToScreenPoint(position);
    }

    public void ActivateRipple()
    {
        time = 0.0f;
        active = true;
    }

	// Update is called once per frame
	void Update ()
    {
        if (active)
        {
            time += Time.deltaTime * speed;
            amount = Mathf.Clamp(1.0f - time / length, 0.0f, 1.0f);

            if(time > length)
            {
                active = false;
                amount = 0.0f;
            }
        }

    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        effectMaterial.SetVector("_Location", screenLocation);
        effectMaterial.SetTexture("_RippleTex", rippleTexture);
        effectMaterial.SetFloat("_Amount", maxAmount * amount); //  * Mathf.Sin(Time.time * 4.0f)

        Graphics.Blit(source, destination, effectMaterial);
    }
}
