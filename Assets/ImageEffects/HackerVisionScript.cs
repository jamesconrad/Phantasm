using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class HackerVisionScript : MonoBehaviour
{
    public Material nightVisionMaterial;
    public Material thermalMaterial;
    public Material sonarMaterial;
    public Material filmGrainMaterial;
    public Material waveMaterial;
    public Texture thermalRamp;

    public Camera CameraSettings;

    private Color ambientLightTemp;
    public Color ambientLight = new Color(0.25f, 0.25f, 0.25f);

    private Light[] all_my_damn_lights;

    enum LightState { Off, On, TempOn, TempOff}
    private bool[] lightStatus;
    private List<float> lightStatus2;

    public Texture FilmGrainScrollingTexture;
    public Texture FilmGrainMultTexture;
    public float scrollAmount;
    private float scrollSpeed;
    [Range(0.0f, 1.0f)]
    public float filmGrainNightVisionAmount = 0.3f;

    [Range(0.0f, 1.0f)]
    public float filmGrainNormalAmount = 0.05f;


    public Color SonarColor = new Color(0.5f, 0.5f, 1.0f);
    [Range(0.0f, 1.0f)]
    public float SonarDiffusePass = 1.0f;
    public float SonarTimeMult = 1.0f;
    public float SonarMult = 0.02f;


    enum HackerVisionMode { Normal, Night, Thermal, Sonar, Last };
    HackerVisionMode Vision = HackerVisionMode.Normal;

    

    public Material AgentMaterial;
    public Material PhantomMaterial;
    
    public Vector2 WaveCount = new Vector2(40.0f, 40.0f);
    public Vector2 WaveIntensity = new Vector2(0.01f, 0.01f);
    public Vector2 WaveTimeMult = new Vector2(1.0f, 1.0f);

    RenderTexture temp;

    float timeNudge;

    Matrix4x4 ProjBiasMatrix = new Matrix4x4();

    float timeSinceOffset = 0.0f;
    float timeOffsetLength = 0.1f;
    float timeOffsetRange = 0.05f;
    Vector2 timeOffsetValue;

    float timeSinceSwap = 1.0f;


    void Start()
    {
        //all_my_damn_lights.FindAll(s => s.Equals("Light")); //= GetComponent<Light>();

        all_my_damn_lights = FindObjectsOfType(typeof(Light)) as Light[];
        for (int i = 0; i < all_my_damn_lights.Length; ++i)
        {
            Debug.Log("It Works");
            lightStatus = new bool[all_my_damn_lights.Length];
            //lightStatus.Add(all_my_damn_lights[i].enabled);
        }

        scrollSpeed = Random.Range(0.8f, 1.2f);
        scrollAmount = Random.Range(0.0f, 1000.0f);
        temp = new RenderTexture(Screen.width, Screen.height, 0);
        timeNudge = Random.Range(0.0f, 1000.0f);

        filmGrainMaterial.SetTexture("uScrollingTexture", FilmGrainScrollingTexture);
        filmGrainMaterial.SetTexture("uMultTexture", FilmGrainMultTexture);

        ProjBiasMatrix.SetRow(0, new Vector4(2.0f, 0.0f, 0.0f, -1.0f));
        ProjBiasMatrix.SetRow(1, new Vector4(0.0f, 2.0f, 0.0f, -1.0f));
        ProjBiasMatrix.SetRow(2, new Vector4(0.0f, 0.0f, 2.0f, -1.0f));
        ProjBiasMatrix.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f,  1.0f));

        ambientLightTemp = RenderSettings.ambientLight;

        thermalRamp.wrapMode = TextureWrapMode.Clamp;

        Shader.SetGlobalFloat("_EmissionVisionMult", 0.0f);
    }

    public void Update()
    {
        if (Vision != HackerVisionMode.Sonar)
        {
            for (int i = 0; i < all_my_damn_lights.Length; ++i)
            {
                lightStatus[i] = all_my_damn_lights[i].enabled;
            }
        }

        // Film Grain transition between vision modes
        if (timeSinceOffset > timeOffsetLength * 20.0f)
        {
            float RandomOffsetChance = Random.Range(0.0f, 1000.0f);
            if (RandomOffsetChance > 995.0f)
            {
                timeSinceOffset = 0.0f;
                timeOffsetValue = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
                timeOffsetLength = Random.Range(0.1f, 0.3f);
            }
        }

        timeSinceOffset += Time.deltaTime;
        timeSinceSwap += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.N))
        {
            timeSinceSwap = 0.0f;

            Vision++;
            if (Vision == HackerVisionMode.Last)
                Vision = HackerVisionMode.Normal;
            // Loop back to beginning
        }

       

    }

    public void OnPreRender()
    {
        //_Light.enabled = true;

        if (Vision == HackerVisionMode.Night)
        {
            // If night vision is on, turn the ambient light up and store actual ambient light
            ambientLightTemp = RenderSettings.ambientLight;
            RenderSettings.ambientLight = ambientLight;
        }

        if (Vision == HackerVisionMode.Normal)
        {
            Shader.SetGlobalFloat("_EmissionVisionMult", 0.0f);
        }
        else
        {
            Shader.SetGlobalFloat("_EmissionVisionMult", 10.0f);
        }

        if (Vision == HackerVisionMode.Sonar)
        {
            for (int i = 0; i < all_my_damn_lights.Length; ++i)
            {
                all_my_damn_lights[i].enabled = false;
            }
            //_Light.enabled = false;
        }
        else
        {
            for (int i = 0; i < all_my_damn_lights.Length; ++i)
            {
                all_my_damn_lights[i].enabled = lightStatus[i];
            }

        }
    }

    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        float filmGrainAmountTotal = 0.0f;

        // Reset the ambient light
        RenderSettings.ambientLight = ambientLightTemp;

        

        if (timeSinceOffset < timeOffsetLength && Vision != HackerVisionMode.Sonar)
        {
            filmGrainAmountTotal += 0.1f;

            filmGrainMaterial.SetVector("uOffsetAmount", new Vector2(
                timeOffsetValue.x + Random.Range(-timeOffsetRange, timeOffsetRange),
                timeOffsetValue.y + Random.Range(-timeOffsetRange, timeOffsetRange)));
        }
        else
        {
            filmGrainMaterial.SetVector("uOffsetAmount", new Vector2(0.0f, 0.0f));
        }

        filmGrainMaterial.SetVector("uScrollAmount", new Vector2(timeNudge + Time.time * scrollSpeed, timeNudge + Time.time * scrollSpeed));

        filmGrainAmountTotal += filmGrainNormalAmount + Mathf.InverseLerp(0.3f, 0.1f, timeSinceSwap);
        float RandomNum = Random.Range(0.0f, 1.0f);
        filmGrainMaterial.SetFloat("RandomNumber", RandomNum);
        filmGrainMaterial.SetFloat("uAmount", filmGrainAmountTotal);

        waveMaterial.SetVector("uWaveCount", WaveCount);
        waveMaterial.SetVector("uWaveIntensity", WaveIntensity);
        waveMaterial.SetVector("uTime", WaveTimeMult * Time.time);

        AgentMaterial.SetColor("_EmissionColor", new Color(0.0f, 0.0f, 0.0f));
        PhantomMaterial.SetColor("_EmissionColor", new Color(0.0f, 0.0f, 0.0f));
        //Shader.SetGlobalFloat(emissionShader.GetInstanceID(), 1.0f); 
        //_EmissionMult
        //emissionShader.

        if (Vision == HackerVisionMode.Normal)
        {

            Graphics.Blit(source, temp, waveMaterial);
            Graphics.Blit(temp, destination, filmGrainMaterial);
        }
        else if (Vision == HackerVisionMode.Night)
        {
            //float RandomNum = Random.Range(0.0f, 1.0f);
            nightVisionMaterial.SetFloat("RandomNumber", RandomNum);
            nightVisionMaterial.SetFloat("uAmount", 0.0f);
            nightVisionMaterial.SetFloat("uLightMult", 1.2f);


            filmGrainMaterial.SetFloat("uAmount", Mathf.Max(filmGrainNightVisionAmount, filmGrainAmountTotal));

            Graphics.Blit(source, temp, nightVisionMaterial);
            Graphics.Blit(temp, destination, filmGrainMaterial);
        }
        else if (Vision == HackerVisionMode.Thermal)
        {
            //thermalRamp.filterMode = FilterMode.Point;
            thermalMaterial.SetTexture("ThermalRamp", thermalRamp);
            
            AgentMaterial.SetColor("_EmissionColor", new Color(1.0f, 1.0f, 1.0f));
            PhantomMaterial.SetColor("_EmissionColor", new Color(1.0f, 1.0f, 1.0f));

            Graphics.Blit(source, temp, thermalMaterial);
            Graphics.Blit(temp, destination, filmGrainMaterial);
        }
        else if (Vision == HackerVisionMode.Sonar)
        {
            sonarMaterial.SetVector("uColorAdd", new Vector4(SonarColor.r, SonarColor.g, SonarColor.b, SonarTimeMult * Time.time));
            sonarMaterial.SetVector("uParameter", new Vector4(SonarMult, SonarDiffusePass, 0.0f, 0.0f));

            Matrix4x4 inverseMatrix = Matrix4x4.Inverse(CameraSettings.projectionMatrix) * ProjBiasMatrix;
            sonarMaterial.SetMatrix("uProjBiasMatrixInverse", inverseMatrix);

            filmGrainMaterial.SetFloat("uAmount", filmGrainAmountTotal - filmGrainNormalAmount);  

            Graphics.Blit(source, temp, sonarMaterial);
            Graphics.Blit(temp, destination, filmGrainMaterial);
        }
    }
}
