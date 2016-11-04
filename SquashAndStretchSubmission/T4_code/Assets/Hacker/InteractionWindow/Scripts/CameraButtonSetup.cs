using UnityEngine;
using System.Collections;

public class CameraButtonSetup : MonoBehaviour {

 
	// Use this for initialization
	void Start () {
        }

    public void positionCorrectly()
    {
        GetComponent<RectTransform>().anchoredPosition = GetComponentInParent<RectTransform>().anchoredPosition + new Vector2(Random.Range(-GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.width), Random.Range(-GetComponent<RectTransform>().rect.height, GetComponent<RectTransform>().rect.height));
    }
}
