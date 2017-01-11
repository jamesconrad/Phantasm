using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DQNAgent : MonoBehaviour {

    struct ComputeGlobals
    {
        int filterWidth;    //width of the filter
        int filterArea;     //filterWidth^2
        int filterStride;   //distance from center to center of filter movement
        int filterStartOffset; //(filterWidth - 1 /) 2
        int filterSegmentationOffset; //TODO: start working on chunking large input fields
        Vector2 pixelSize;   //size of 1 pixel in uv space
    };

    struct Action
    {
        int imageReference;
        float time;
        List<float> action;
    };

    private static ComputeShader DQNShader;
    private static ComputeGlobals DQNGlobals;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ExecuteNetwork()
    {


    }

    void TrainNetwork()
    {


    }
}
