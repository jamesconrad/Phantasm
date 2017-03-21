using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class ElectricityLineScript : MonoBehaviour
{
    //[Tooltip("Y Axis determines height, X/Z determine circle range")]
    [Tooltip("Width and Depth Size")]
    public Vector3 size = new Vector3(1.0f, 2.25f, 0.5f);
    public AnimationCurve curveZ = AnimationCurve.Linear(0, 0, 1, 0);
    LineRenderer line;
    public Material electricityMaterial;
    Material material;

    float offsetAmt = 0.4f;
    Vector3 offsetMult = new Vector3(0.25f, 0.5f, 0.25f);

    float timeSinceJump = 0.0f;
    float maxStillTime = 0.155f;
    Vector2 minMaxTime = new Vector2 (0.375f, 0.500f);

    public float width = 0.1f;

    
   AnimationCurve curveHeight = AnimationCurve.Linear(0, 0, 1, 0);

	// Use this for initialization
	void Start ()
    {
		line = GetComponent<LineRenderer>();
        line.widthMultiplier = width;
        line.numPositions = 15;
        material = new Material(electricityMaterial);
        line.material = material;
        for(int i = 0; i < line.numPositions; ++i)
        {
            float Interp = Mathf.Lerp(-1.0f, 1.0f, (float)i / (line.numPositions - 1.0f));
            line.SetPosition(i, new Vector3(size.x * Interp, 0.0f, curveZ.Evaluate(Mathf.Abs(Interp)))); //Mathf.InverseLerp(0.0f, line.numPositions, i))));
        }
	}

    public void Activate()
    {

    }

    public void Deactivate()
    {

    }

    private void OnWillRenderObject()
    {

    //}



    // Update is called once per frame
    //void Update ()
    //{
        timeSinceJump += Time.deltaTime;
        
        if (timeSinceJump > maxStillTime)
        {
            line.material.SetColor("ColorMult", new Color(1.0f, 1.0f, 1.0f, 1.0f));

            timeSinceJump = 0.0f;
            maxStillTime = Random.Range(minMaxTime.x, minMaxTime.y);

            curveHeight = AnimationCurve.Linear(0, Random.Range(-1.0f, 1.0f), 1, Random.Range(-1.0f, 1.0f));

            //Debug.Log("First " + curveHeight.keys[0].value + "\nSecond " + curveHeight.keys[1].value);

            Keyframe[] widthKeyframes = new Keyframe[line.numPositions];

            for (int i = 0; i < line.numPositions; ++i)
            {
                widthKeyframes[i].value = Random.Range(0.4f, 1.5f);
                widthKeyframes[i].time = (float)i / line.numPositions;
            }

		    for(int i = 1; i < line.numPositions - 1; ++i)
            {
                Vector3 offset = new Vector3(
                    offsetMult.x * Random.Range(-offsetAmt, offsetAmt), 
                    offsetMult.y * Random.Range(-offsetAmt, offsetAmt), 
                    offsetMult.z * Random.Range(-offsetAmt, offsetAmt));
                float Interp = Mathf.Lerp(-1.0f, 1.0f, (float)i / (line.numPositions - 1.0f));
                
                line.SetPosition(i, offset + new Vector3(
                    size.x * Interp, 
                    curveHeight.Evaluate(Interp * 0.5f + 0.5f) * size.y, 
                    size.z * curveZ.Evaluate(Mathf.Abs(Interp)))); //Mathf.InverseLerp(0.0f, line.numPositions, i))));

                


                //line.wid
            }
            
            line.widthCurve = new AnimationCurve(widthKeyframes);
        }
        else
        {
            line.material.SetColor("ColorMult", new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp(0.75f, 1.00f, timeSinceJump / maxStillTime)));

            for(int i = 1; i < line.numPositions - 1; ++i)
            {
                Vector3 offset = new Vector3(
                    offsetMult.x * Random.Range(-offsetAmt, offsetAmt) * 6.1f, 
                    offsetMult.y * Random.Range(-offsetAmt, offsetAmt) * 6.1f, 
                    offsetMult.z * Random.Range(-offsetAmt, offsetAmt) * 6.1f);
                line.SetPosition(i, line.GetPosition(i) + offset * Time.deltaTime); //Mathf.InverseLerp(0.0f, line.numPositions, i))));
            }
        }
	}
}
