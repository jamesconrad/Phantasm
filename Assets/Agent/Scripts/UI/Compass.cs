using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour 
{
    Canvas rootCanvas;
    RawImage texture;
    GameObject playerReference;

    public float middlePoint = 0.378f;
    public float offset = 0.0f;
	// Initialization
	void Start () 
	{
        rootCanvas = GetComponentInParent<Canvas>();
        playerReference = rootCanvas.GetComponentInChildren<SplashScreen>().screenOwner;
        texture = GetComponent<RawImage>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        texture.uvRect = new Rect(offset + middlePoint * ((Vector3.Angle(Vector3.left, playerReference.transform.right) / 180.0f) * (Vector3.Dot(Vector3.forward, playerReference.transform.right) > 0 ? 1 : -1)), texture.uvRect.y, texture.uvRect.width, texture.uvRect.height);
	}
}
