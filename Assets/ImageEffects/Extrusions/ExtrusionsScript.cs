using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class ExtrusionsScript : MonoBehaviour
{
    public Material effectMaterial;



    // OnRenderImage is called after all rendering is complete to render image
    void Update()
    {
        float timeAmount = Time.time * 10.1f;
        //float RandomNum = Random.Range(0.0f, 1.0f);
        //effectMaterial.SetFloat("RandomNumber", RandomNum);
        effectMaterial.SetFloat("_ExtrusionAdd", timeAmount);
    }
}