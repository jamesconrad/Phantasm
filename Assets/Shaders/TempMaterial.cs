using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TempMaterial : MonoBehaviour
{
    public Color color = new Color(1.0f, 1.0f, 1.0f);
    Material material;

	// Use this for initialization
	void Start ()
    {
        material = new Material(gameObject.GetComponent<MeshRenderer>().material);
        gameObject.GetComponent<MeshRenderer>().material = material;// = new Material(gameObject.GetComponent<MeshRenderer>().material);
        material.SetColor("_Color", color);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //material.SetColor("_Color", color);

    }
}
