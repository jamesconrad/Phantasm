using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeTrail : MonoBehaviour
{
    float alpha = 1.0f;
    float startAlphaMult = 0.0f;
    LineRenderer line;
    float distance = 0.0f;
    public Material _material;

	// Use this for initialization
	void Start ()
    {
        line = GetComponent<LineRenderer>();
        line.material = _material;
        alpha = line.material.GetFloat("alphaAdd");
        startAlphaMult = line.material.GetFloat("alphaAmount");
    }
    
    public void SetLinePosition(LineRenderer liner)
    {
        Vector3[] temp = new Vector3[liner.numPositions];
        liner.GetPositions(temp);
        line = GetComponent<LineRenderer>();
        if (line != null)
        {
            line.numPositions = liner.numPositions;
            line.SetPositions(temp);
        }
        else
        {
            line = GetComponent<LineRenderer>();
            Debug.Log("Line is Null");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        alpha -= Time.deltaTime;
        distance = Vector3.Distance(line.GetPosition(0), line.GetPosition(1));
        line.material.SetFloat("alphaAdd", alpha);
        line.material.SetFloat("uDistance", distance);

        if (alpha <= -0.5f - startAlphaMult)
        {
            Destroy(this.gameObject);
        }

    }
}
