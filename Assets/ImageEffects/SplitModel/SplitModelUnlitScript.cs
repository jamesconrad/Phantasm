using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SplitModelUnlitScript : MonoBehaviour
{
    public Material material;
    public Vector3 offsetMult = new Vector3(0.0f, 0.1f, 0.0f);
    public float Alpha = 0.5f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        material.SetVector("uOffset", new Vector3(Time.time * offsetMult.x, Time.time * offsetMult.y, Time.time * offsetMult.z));
        material.SetFloat("uAlpha", Alpha);
        
    }
}
