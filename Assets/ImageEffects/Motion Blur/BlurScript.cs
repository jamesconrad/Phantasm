using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class BlurScript : MonoBehaviour
{
    public Shader shader;

    Material material;

    Vector3 oldPosition;
    Vector3 newPosition;
    Vector3 displacement;

    Vector2 blurDirection;
    float amount = 0.25f;
    public float amountMult = 0.01f;

    // Use this for initialization
    void Start ()
    {
        material = new Material(shader);
    }
	
    //public void setScreenLocation(GameObject location)
    //{
    //
    //}
    //
    //public void setScreenLocation(Vector3 position)
    //{
    //
    //}

    public void UpdatePosition(Vector3 pos)
    {
        oldPosition = newPosition;
        newPosition = pos;
    }

	// Update is called once per frame
	void Update ()
    {

    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        displacement = newPosition - oldPosition;
        amount = Vector3.Distance(displacement, Vector3.zero);

        material.SetVector("_BlurParam", new Vector4(displacement.x, displacement.y, displacement.z, amount * amountMult));

        Graphics.Blit(source, destination, material);
    }
}
