using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class LaserScript : MonoBehaviour
{
    float CameraShakeAmount = 0.5f;
    float CameraShakeTime = 0.5f;

    public GameObject MainCamera;
    CAScript CA_Amount;
    RippleScript Ripple;
    BlurScript Blur;

    ///////////////////////////////////////////////////////////////////////////////////

    public LineRenderer line;
    public float distanceMax = 100.0f;
    float currDistanceMax = 100.0f;
    QueryTriggerInteraction hitTriggers;
    public LayerMask whatToCollideWith;
    RaycastHit hit;

    Quaternion laserRotation;
    Vector3 laserDirection;
    Vector3 laserDirectionUp;

    float laserDistance;
    Vector3 lpBegin;
    Vector3 lpMiddle;
    Vector3 lpEnd;

    public Material material;
    Material _material;
    bool laserActive;
    bool laserWarmUpActive;
    bool laserCoolDownActive;

    float laserTimerBegin;
    float laserTimer;
    float laserWarmUpTimer = 0.0f;
    float laserWarmUpTime = 6.0f;
    float warmUpLerp;

    float laserSize = 0.25f;

    float laserTimerTurnOff = 10.0f;

    public GameObject[] LaserCylinder;
    public GameObject LaserCylinderCenter;
    public Light LaserLightBegin;
    public Light LaserLightEnd;

    public Color ChargeLaserColor = new Color(0.5f, 1.0f, 1.0f);
    public Color FireLaserColor = new Color(1.0f, 0.5f, 0.5f);

    float laserChargeUpTime = 2.0f / 5.0f;
    float laserChargeDownTime = 2.0f / 5.0f;


    ///////////////////////////////////////////////////////////////////////////////////

    // This is the stuff for line renderers electricity stuff
    public int numOfLines = 6;
    public int numOfMaxLineSegments = 100;
    LineRenderer[] LaserLines;
    GameObject[] LaserLineGameObjects;

    public int numOfChargeLines = 4;
    LineRenderer[] LaserChargeUpLines;
    GameObject[] LaserLineChargeUpGameObjects;

    public Shader LaserElectricity;
    Material LaserElectricityMaterial;
    Material LaserElectricitySharpMaterial;
    public Texture LaserElectricityTexture;
    public Texture LaserChargeUpTexture;

    // Maximum displacement offset for electricity lines
    float electricityRandom = 0.45f;
    float electricityMiniRandom = 0.15f;
    int electricityLineUpdate = 0;
    int electricityChargeLineUpdate = 0;


    ///////////////////////////////////////////////////////////////////////////////////

    public ParticleSystem LaserParticleSuckIn;
    public ParticleSystem LaserParticleGiantSuckIn;
    public ParticleSystem LaserParticleGiantBlowOut;
    public ParticleSystem LaserParticleGenerate;
    public ParticleSystem LaserParticleStreak;
    public ParticleSystem LaserParticleSparks;
    
    public AudioClip[] LaserSound;
    public float[] LaserSoundTime;

    float ParticleGenerateRadius = 0.35f;
    //float electricityTime = 0.0f;
    //float electricityTimeLimit = 0.15f;

    public bool randomLaserCharge = true;
    public AudioSource LaserChargeSound;

    public float LaserForce = 10.0f;
    
    public ParticleSystem LaserParticleMEMES;
    
    public KeyCode FireKey = KeyCode.Space;


    private bool CameraLink = false;

    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////

    void Start()
    {
        _material = new Material(material);
        line.material = _material;

        if (MainCamera != null)
        {
            CameraLink = true;
            CA_Amount = MainCamera.GetComponent<CAScript>();
            Ripple = MainCamera.GetComponent<RippleScript>();
            Blur = MainCamera.GetComponent<BlurScript>();
        }

        LaserElectricityMaterial = new Material(LaserElectricity);
        LaserElectricitySharpMaterial = new Material(LaserElectricity);
        LaserElectricityMaterial.SetTexture("_MainTex", LaserElectricityTexture);
        LaserElectricitySharpMaterial.SetTexture("_MainTex", LaserChargeUpTexture);

        LaserLineGameObjects = new GameObject[numOfLines];
        LaserLines = new LineRenderer[numOfLines];
        for (int i = 0; i < numOfLines; ++i)
        {
            LaserLineGameObjects[i] = new GameObject("LaserLine" + i, typeof(LineRenderer));
            LaserLines[i] = LaserLineGameObjects[i].GetComponent<LineRenderer>();
            LaserLines[i].material = LaserElectricityMaterial;
            LaserLines[i].enabled = false;
            LaserLineGameObjects[i].transform.SetParent(this.transform);
        }

        LaserLineChargeUpGameObjects = new GameObject[numOfChargeLines];
        LaserChargeUpLines = new LineRenderer[numOfChargeLines];
        for (int i = 0; i < numOfChargeLines; ++i)
        {
            LaserLineChargeUpGameObjects[i] = new GameObject("LaserChargeLine" + i, typeof(LineRenderer));
            LaserChargeUpLines[i] = LaserLineChargeUpGameObjects[i].GetComponent<LineRenderer>();
            LaserChargeUpLines[i].material = LaserElectricitySharpMaterial;
            LaserChargeUpLines[i].enabled = false;
            LaserLineChargeUpGameObjects[i].transform.SetParent(this.transform);
        }


        LaserParticleSuckIn.Stop();
        LaserParticleGiantSuckIn.Stop();
        LaserParticleGiantBlowOut.Stop();
        LaserParticleGenerate.Stop();
        LaserParticleStreak.Stop();
        LaserParticleSparks.Stop();

        LaserParticleSuckIn.Clear();
        LaserParticleGiantSuckIn.Clear();
        LaserParticleGiantBlowOut.Clear();
        LaserParticleGenerate.Clear();
        LaserParticleStreak.Clear();
        LaserParticleSparks.Clear();

        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = true;
        hitTriggers = QueryTriggerInteraction.Ignore;
    }

    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////

    float offsetMult = 1.0f;

    void Update()
    {
        if (CameraLink)
        {

            offsetMult = Mathf.Sin(Time.time * 11.1f) * 0.15f +
                          //Mathf.Sin(Time.time * 12.2f) * 0.10f + 
                          //Mathf.Sin(Time.time * 10.0f) * 0.10f + 
                          1.0f;

            //CameraPosition = Camera.main.transform
            //Ripple.screenLocation = MainCamera.GetComponent<Camera>().WorldToScreenPoint(gameObject.transform.position);
            Ripple.setScreenLocation(gameObject);

            if (laserTimer > 0.0f && laserTimer < CameraShakeTime || laserTimer > 0.0f)
            {
                float t = Mathf.InverseLerp(CameraShakeTime, 0.0f, laserTimer) + 0.05f;
                //t = 1.0f;
                CA_Amount.offset = t * 0.025f;

                MainCamera.transform.localPosition = new Vector3(
                    Random.Range(-CameraShakeAmount, CameraShakeAmount) * t,
                    Random.Range(-CameraShakeAmount, CameraShakeAmount) * t,
                    Random.Range(-CameraShakeAmount, CameraShakeAmount) * t);

                Blur.UpdatePosition(MainCamera.transform.localPosition);
            }
            else
            {
                CA_Amount.offset = 0.0f;
                MainCamera.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                //Camera.main.transform.localPosition = CameraPosition;
            }

        }

        if (Input.GetKeyDown(FireKey) || Input.GetMouseButtonDown(0) || laserTimer > 2000000.0f)
        {
            if (laserActive || laserWarmUpActive)
            {
                if (laserActive)
                    laserCoolDownActive = true;
                //LaserChargeSound.Stop();

                //laserTimerTurnOff = Mathf.Lerp(1.0f, 0.0f, laserTimer / laserChargeUpTime);
                laserTimer = -1.0f;
                Debug.Log("Laser Disabled");              
                laserActive = false;
                laserWarmUpActive = false;
                line.enabled = false;
                //LaserLightBegin.intensity = 0.0f;
                //LaserLightEnd.intensity = 0.0f;
                foreach (GameObject LAZER in LaserCylinder)
                    LAZER.SetActive(false);
                LaserCylinderCenter.SetActive(false);
                foreach (LineRenderer LAZER in LaserLines)
                {
                    LAZER.enabled = false;
                }
                foreach (LineRenderer LAZER in LaserChargeUpLines)
                {
                    LAZER.enabled = false;
                }
                LaserParticleSuckIn.Stop();
                LaserParticleGenerate.Stop();
                LaserParticleStreak.Stop();
                LaserParticleSparks.Stop();
                //laserWarmUpTimer = 0.0f;

                StopCoroutine("FireLaser");
                StopCoroutine("ActivateLaser");
                StopCoroutine("LaserSuckGiant");
            }
            else if (laserTimerTurnOff / laserChargeDownTime > 1.0f)
            {
                laserTimerTurnOff = 0.0f;// Mathf.Lerp(0.0f, 1.0f, laserTimer / laserChargeDownTime);

                LaserLightBegin.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f);
                LaserLightBegin.color = ChargeLaserColor;
                LaserLightEnd.color = ChargeLaserColor;

                if (randomLaserCharge)
                {
                    int ran = Random.Range(0, LaserSound.Length);
                    LaserChargeSound.clip = LaserSound[ran];
                    laserWarmUpTime = LaserSoundTime[ran];
                    LaserChargeSound.time = 0.0f;
                }
                LaserChargeSound.Play();
                LaserChargeSound.pitch = 1.0f;
                LaserParticleSuckIn.Play();
                LaserParticleGenerate.Play();
                laserWarmUpActive = true;
                laserWarmUpTimer = 0.0f;
                StartCoroutine("ActivateLaser", laserWarmUpTime);
                StartCoroutine("LaserSuckGiant", laserWarmUpTime - 0.5f);

                foreach (LineRenderer LAZER in LaserChargeUpLines)
                {
                    LAZER.enabled = true;
                    LAZER.widthMultiplier = 0.0f;
                }

            }
        }

        //Debug.Log(laserTimerTurnOff);

        if (!laserWarmUpActive && !laserActive)
        {
            laserTimerTurnOff += Time.deltaTime;

            LaserChargeSound.pitch = Mathf.Lerp(1.0f, 0.0f, Mathf.Pow(laserTimerTurnOff * 1.0f, 2.0f));

            float interp = Mathf.Lerp(1.0f, 0.0f, laserTimerTurnOff / laserChargeDownTime);

            LaserLightEnd.intensity = LaserLightBegin.intensity;
            if (interp > 0.0f && laserCoolDownActive)
            {
                line.enabled = true;
                LaserLightBegin.intensity = interp;
                CalcRaycast(interp);
            }
            else
            {
                LaserLightBegin.intensity = interp * laserWarmUpTimer / laserWarmUpTime;
                LaserLightEnd.intensity = 0.0f;
                line.enabled = false;
            }
        }


        if (laserWarmUpActive)
        {

            laserCoolDownActive = false;

            laserWarmUpTimer += Time.deltaTime;

            warmUpLerp = Mathf.Lerp(0.0f, 1.0f, laserWarmUpTimer / laserWarmUpTime);

            var shape = LaserParticleGenerate.shape;
            shape.radius = Mathf.Lerp(0.0f, ParticleGenerateRadius, warmUpLerp);
            //Debug.Log(shape.radius);

            LaserLightBegin.intensity = warmUpLerp * 1.0F;
        }

        if (laserActive || laserWarmUpActive)
        {
            {

                int num = electricityChargeLineUpdate % LaserChargeUpLines.Length;

                //LaserChargeUpLines[num].useWorldSpace = false;

                int numOfLineSegments = Random.Range(2, 6);
                numOfLineSegments = Mathf.Clamp(numOfLineSegments, 2, numOfMaxLineSegments);

                Vector3 RandomDirection = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), Random.Range(1.5f, 2.0f));

                LaserChargeUpLines[num].numPositions = numOfLineSegments + 1;
                Vector3[] linePos = new Vector3[LaserChargeUpLines[num].numPositions];
                for (int i = 0; i <= numOfLineSegments; ++i)
                {
                    linePos[i] = Vector3.Lerp(Vector3.zero + transform.position, transform.rotation * RandomDirection + transform.position, i / (float)numOfLineSegments);
                }

                for (int i = 1; i <= numOfLineSegments; ++i)
                {
                    linePos[i] += new Vector3(Random.Range(-electricityMiniRandom, electricityMiniRandom), Random.Range(-electricityMiniRandom, electricityMiniRandom), Random.Range(-electricityMiniRandom, electricityMiniRandom));
                }

                Keyframe[] widthKeyframes = new Keyframe[numOfLineSegments + 1];

                float widthIncrease = warmUpLerp;

                if (laserActive)
                {
                    widthIncrease *= 2.0f;
                }

                for (int i = 0; i <= numOfLineSegments; ++i)
                {
                    widthKeyframes[i].value = Random.Range(0.05f, 0.05f) * (float)(numOfLineSegments - i + 1.0f);
                    widthKeyframes[i].time = (float)i / numOfLineSegments;
                }

                LaserChargeUpLines[num].widthCurve = new AnimationCurve(widthKeyframes);

                LaserChargeUpLines[num].widthMultiplier = widthIncrease;

                LaserChargeUpLines[num].SetPositions(linePos);
                LaserChargeUpLines[num].enabled = true;
                float hue = (float)System.Math.IEEERemainder(num / (float)LaserChargeUpLines.Length + Time.time, 1.0);
                //Debug.Log(hue);
                Color ElectricityColor = Color.HSVToRGB(hue + 0.5f, 1.0f, 1.0f);

                ElectricityColor = Color.HSVToRGB(hue * 0.25f + 0.6f, 1.0f, 1.0f);

                LaserChargeUpLines[num].material.SetColor("ColorMult", ElectricityColor); //
                for (int i = 0; i < LaserChargeUpLines.Length; i++)
                {
                    LaserChargeUpLines[(num + LaserChargeUpLines.Length - i) % LaserChargeUpLines.Length].material.SetFloat("alphaAdd", 1.0f - ((float)i * 2.0f / LaserChargeUpLines.Length));
                }

                ++electricityChargeLineUpdate;
            }



        }

        if (laserActive)
        {
            if(!LaserChargeSound.isPlaying)
            {
                LaserChargeSound.Play();
                LaserChargeSound.time = laserWarmUpTime;                
            }

            var shape = LaserParticleGenerate.shape;
            shape.radius = ParticleGenerateRadius;

            laserTimer = Time.time - laserTimerBegin;

            float LaserIntensity = Mathf.Lerp(0.0f, 1.0f, laserTimer / laserChargeUpTime);
            LaserLightBegin.intensity = LaserIntensity;
            LaserLightEnd.intensity = LaserIntensity;



            foreach (GameObject LAZER in LaserCylinder)
            {
                LAZER.transform.position = lpMiddle;
                LAZER.transform.rotation = laserRotation;
                LAZER.SetActive(true);
            }

            LaserCylinderCenter.transform.position = lpMiddle;
            LaserCylinderCenter.transform.rotation = laserRotation;
            LaserCylinderCenter.SetActive(true);
            
        }
        else
        {

            LaserParticleMEMES.Stop();
            LaserParticleMEMES.Clear();


        }

        if(laserActive)
        {
            //float H = 0.0F;
            //float S = 1.0F;
            //float V = 1.0F;
            //
            //Color.RGBToHSV(LaserLightBegin.color, out H, out S, out V);
            
            LaserLightBegin.color = Color.HSVToRGB((float)System.Math.IEEERemainder(Time.time * 1.1f, 1.0f) + 0.5f, 1.0f, 1.0f);
            Vector3 colorVec =  Vector3.Normalize(new Vector3(LaserLightBegin.color.r, LaserLightBegin.color.g, LaserLightBegin.color.b));
            LaserLightBegin.color = new Color(colorVec.x, colorVec.y, colorVec.z);
            LaserLightEnd.color = LaserLightBegin.color;

        }

        
        LaserLightEnd.intensity *= 5.0f;
        LaserLightBegin.intensity *= 5.0f;

        //LaserLightBegin.intensity = 0.0f;

    }


    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////

    IEnumerator ActivateLaser(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (laserWarmUpActive)
        {
            LaserChargeSound.pitch = 1.0f;

            LaserParticleMEMES.Play();
            LaserParticleStreak.Play(true);
            LaserParticleSparks.Play();
            LaserParticleGiantBlowOut.Play();
            LaserParticleSuckIn.Stop();

            if(CameraLink)
                Ripple.ActivateRipple();

            foreach (LineRenderer LAZER in LaserChargeUpLines)
            {
                LAZER.enabled = false;
            }


            LaserLightBegin.color = FireLaserColor;
            LaserLightEnd.color = FireLaserColor;

            laserTimerBegin = Time.time;
            currDistanceMax = 0.0f;
            Debug.Log("Laser Active");

            line.enabled = true;
            laserActive = true;
            laserWarmUpActive = false;
            //LaserParticleSuckIn.IsAlive(true);
            StartCoroutine("FireLaser");
        }
        //yield return null;
    }

    IEnumerator LaserSuckGiant(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);

        LaserParticleGiantSuckIn.Play();
    }

    IEnumerator LaserBlowGiant(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////

    void CalcRaycast(float width = 1.0f)
    {
        Ray ray = new Ray(transform.position, transform.forward);

        currDistanceMax = Mathf.Clamp(currDistanceMax + Time.deltaTime / laserChargeUpTime * distanceMax, 0.0f, distanceMax);

        line.SetPosition(0, ray.origin);
        if (Physics.Raycast(ray, out hit, currDistanceMax, whatToCollideWith, hitTriggers))
        {            
            LaserParticleSparks.transform.position = Vector3.Lerp(lpBegin, lpEnd, 0.995f);
            LaserParticleSparks.transform.rotation = Quaternion.LookRotation(hit.normal);
            LaserParticleSparks.transform.position += hit.normal * 0.1f; 
            line.SetPosition(1, hit.point);
            _material.SetFloat("uDistance", Vector3.Distance(ray.origin, hit.point));
        }
        else
        {
            line.SetPosition(1, ray.GetPoint(currDistanceMax));
            _material.SetFloat("uDistance", currDistanceMax);
        }

        lpBegin = line.GetPosition(0);
        lpEnd = line.GetPosition(1);
        lpMiddle = Vector3.Lerp(lpBegin, lpEnd, 0.5f);
        laserDistance = Vector3.Distance(lpBegin, lpEnd);


        laserDirection = Vector3.Normalize(lpEnd - lpBegin);
        laserRotation = Quaternion.LookRotation(laserDirection);

        laserRotation *= Quaternion.AngleAxis(90.0f, new Vector3(1.0f, 0.0f, 0.0f));
        laserRotation *= Quaternion.AngleAxis(Time.time * 10.0f, new Vector3(0.0f, 1.0f, 0.0f));

        laserDirectionUp = laserDirection + new Vector3(0.0f, 0.1f, 0.0f);

        line.widthMultiplier = width;        
        line.widthMultiplier *= offsetMult;
    }

    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////

    IEnumerator FireLaser()
    {
        while (true)
        {
            CalcRaycast();

            if (hit.rigidbody)
                hit.rigidbody.AddForceAtPosition(LaserForce * (laserDirectionUp), hit.point);

            //foreach (LineRenderer LAZER in LaserLines)
            {
                int num = electricityLineUpdate % LaserLines.Length;

                int numOfLineSegments = (int)laserDistance;
                numOfLineSegments = Mathf.Clamp(numOfLineSegments, 2, numOfMaxLineSegments);

                LaserLines[num].numPositions = numOfLineSegments + 1;
                Vector3[] linePos = new Vector3[LaserLines[num].numPositions];
                for (int i = 0; i <= numOfLineSegments; ++i)
                {
                    linePos[i] = Vector3.Lerp(lpBegin, lpEnd, i / (float)numOfLineSegments);
                }

                for (int i = 1; i <= numOfLineSegments; ++i)
                {
                    linePos[i] += offsetMult * new Vector3(Random.Range(-electricityRandom, electricityRandom), Random.Range(-electricityRandom, electricityRandom), Random.Range(-electricityRandom, electricityRandom));
                }

                Keyframe[] widthKeyframes = new Keyframe[numOfLineSegments + 1];

                for (int i = 0; i <= numOfLineSegments; ++i)
                {
                    widthKeyframes[i].value = Random.Range(0.025f, 0.15f);
                    widthKeyframes[i].time = (float)i / numOfLineSegments;
                }

                LaserLines[num].widthCurve = new AnimationCurve(widthKeyframes);

                LaserLines[num].SetPositions(linePos);
                LaserLines[num].enabled = true;
                float hue = (float)System.Math.IEEERemainder(num / (float)LaserLines.Length + Time.time, 1.0);
                //Debug.Log(hue);
                Color ElectricityColor = Color.HSVToRGB(hue + 0.5f, 1.0f, 1.0f);
                LaserLines[num].material.SetColor("ColorMult", ElectricityColor); //
                for (int i = 0; i < numOfLines; i++)
                {
                    LaserLines[(num + LaserLines.Length - i) % LaserLines.Length].material.SetFloat("alphaAdd", 1.0f - ((float)i * 2.0f / LaserLines.Length));
                }
                ++electricityLineUpdate;
            }

            Vector3 CylinderScale = new Vector3(laserSize, Vector3.Distance(lpEnd, lpBegin) * 0.5f, laserSize);
            Vector3 CylinderSmallScale = new Vector3(laserSize * 0.4f, Vector3.Distance(lpEnd, lpBegin) * 0.5f, laserSize * 0.4f);

            for (int i = 0; i < LaserCylinder.Length; ++i)
            {
                Vector3 scale = CylinderScale;
                scale.x += Mathf.Sin(Time.time * (10.0f + i * 1.0f)) * laserSize * 0.05f;
                scale.z += Mathf.Sin(Time.time * (10.0f + i * 1.0f)) * laserSize * 0.05f;
                scale.x *= i + 1;
                scale.z *= i + 1;
                LaserCylinder[i].transform.localScale = scale;
            }

            LaserCylinderCenter.transform.localScale = CylinderSmallScale;

            //LaserLightBegin.transform.position = Vector3.Lerp(lpBegin, lpEnd, Mathf.Max(0.05f, 0.01f * (laserDistance / distanceMax)));
            LaserLightBegin.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f);
            LaserLightEnd.transform.position = Vector3.Lerp(lpBegin, lpEnd, 0.90f);

            //Debug.Log("Distance" + Vector3.Distance(lpBegin, lpEnd));

            LaserParticleStreak.transform.position = lpEnd;
                        
            if(hit.rigidbody == null)
            {
                LaserParticleStreak.Stop();
            }

            if (laserDistance * 1.01f <= currDistanceMax)
            {
                if (!LaserParticleStreak.isEmitting)
                    LaserParticleStreak.Play();
                if (!LaserParticleSparks.isEmitting)
                    LaserParticleSparks.Play(true);
            }
            else
            {
                //Debug.Log("Larger");
                if (LaserParticleStreak.isPlaying)
                    LaserParticleStreak.Stop();
                if (LaserParticleSparks.isPlaying)
                    LaserParticleSparks.Stop();
            }

            currDistanceMax = laserDistance;

            //Debug.Log(hit.normal);

            //LaserParticleSparks.transform.rotation = Quaternion.Euler(hit.normal);

            //LaserParticleSparks.transform.rotation;

            //Debug.Log("Laser Distance: " + Vector3.Distance(line.GetPosition(0), line.GetPosition(1)));

            yield return null;
        }
    }
}
