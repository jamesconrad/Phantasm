using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewModeSwap : MonoBehaviour {

    private HackerVisionScript[] hvs;
    public HackerVisionScript.HackerVisionMode buttonMaterial;
    public RawImage highlight;

    public void Start()
    {
        hvs = FindObjectsOfType<HackerVisionScript>();
    }

    public void OnClick()
    {
        for (int i = 0; i < hvs.Length; i++)
        {
            hvs[i].Vision = buttonMaterial;
            hvs[i].timeSinceSwap = 0.0f;
            //hvs[i].GetComponent<Camera>().ResetReplacementShader();

            if (buttonMaterial == HackerVisionScript.HackerVisionMode.Normal)

            {
                hvs[i].cameraModeArray = 1;
                hvs[i].filmGrainBarrel = hvs[i].barrelDistortAmount.normal;
                hvs[i].filmGrainBarrelZoom = hvs[i].barrelDistortZoomAmount.normal;
            }
            else if (buttonMaterial == HackerVisionScript.HackerVisionMode.Night)
            {
                hvs[i].cameraModeArray = 1;
                hvs[i].filmGrainBarrel = hvs[i].barrelDistortAmount.night;
                hvs[i].filmGrainBarrelZoom = hvs[i].barrelDistortZoomAmount.night;
            }
            else if (buttonMaterial == HackerVisionScript.HackerVisionMode.Sonar)
            {
                //hvs[i].GetComponent<Camera>().SetReplacementShader(hvs[i].sonarPassthroughGeometry, null);
                hvs[i].cameraModeArray = 3;
                hvs[i].filmGrainBarrel = hvs[i].barrelDistortAmount.sonar;
                hvs[i].filmGrainBarrelZoom = hvs[i].barrelDistortZoomAmount.sonar;
            }
            else if (buttonMaterial == HackerVisionScript.HackerVisionMode.Thermal)
            {
                hvs[i].cameraModeArray = 2;
                hvs[i].filmGrainBarrel = hvs[i].barrelDistortAmount.thermal;
                hvs[i].filmGrainBarrelZoom = hvs[i].barrelDistortZoomAmount.thermal;
            }
        }

        highlight.transform.position = transform.position;
    }
}
