using UnityEngine;
using System.Collections;

[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class DebugScript : MonoBehaviour
{
    public Material testMaterial;

    public Material materialGBuffer0;
    public Material materialGBuffer1;
    public Material materialGBuffer2;
    public Material materialGBuffer2ViewSpace;
    public Material materialGBuffer3;

    public Font debugFont;

    public Camera mainCameraSettings;

    //public Vector2 WaveCount = new Vector2(40.0f, 40.0f);
    //public Vector2 WaveIntensity = new Vector2(0.01f, 0.01f);
    //public Vector2 WaveTimeMult = new Vector2(1.0f, 1.0f);

    float deltaTime = 0.0f;

    bool active = false;

    enum DebugTest {First, Off, GBuffer0, GBuffer1, GBuffer2, ViewSpaceGBuffer2, GBuffer3, Last};
    DebugTest currentMode = DebugTest.Off;

    // Use this for initialization
    void Start()
    {

    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 1.0f, 1.0f, 1.0f);
        style.font = debugFont;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0} ", currentMode);
        GUI.Label(rect, text, style);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            currentMode++;
            if (currentMode == DebugTest.Last)
                currentMode = DebugTest.First + 1;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            currentMode--;
            if (currentMode == DebugTest.First)
                currentMode = DebugTest.Last - 1;
        }
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //testMaterial.SetVector("uWaveCount", WaveCount);
        //testMaterial.SetVector("uWaveIntensity", WaveIntensity);
        //testMaterial.SetVector("uTime", WaveTimeMult * Time.time);

        mainCameraSettings.depthTextureMode = DepthTextureMode.DepthNormals;

        if (currentMode == DebugTest.Off)
            Graphics.Blit(source, destination);
        if (currentMode == DebugTest.GBuffer0)
            Graphics.Blit(source, destination, materialGBuffer0);
        if (currentMode == DebugTest.GBuffer1)
            Graphics.Blit(source, destination, materialGBuffer1);
        if (currentMode == DebugTest.GBuffer2)
            Graphics.Blit(source, destination, materialGBuffer2);
        if (currentMode == DebugTest.ViewSpaceGBuffer2)
            Graphics.Blit(source, destination, materialGBuffer2ViewSpace);        
        if (currentMode == DebugTest.GBuffer3)
            Graphics.Blit(source, destination, materialGBuffer3);
    }
}
