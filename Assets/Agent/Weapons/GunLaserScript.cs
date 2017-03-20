using UnityEngine;
using System.Collections;

public class GunLaserScript : MonoBehaviour
{
    public LineRenderer line;
    Light light;
    RaycastHit rayHit;

    Vector3 laserDirection;
    float distanceMax = 100.0f;
    float distance = 100.0f;
    QueryTriggerInteraction hitTriggers;
    public LayerMask whatToCollideWith;
    public Material material;
    public static Color colorHit = Color.black;
    public static float metallicHit = 0.0f;

    public bool active = true;

    void Start()
    {        
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = true;
        light = line.GetComponentInChildren<Light>();
        if(light == null)
        {
            Debug.Log("Light is null!");
            light = new Light();
        }
        hitTriggers = QueryTriggerInteraction.Ignore;
    }

    void Awake()
    {
        StartCoroutine("FireLaser");
    }

    void Update()
    {
        line.enabled = active;
    }

    public void OnDestroy()
    {
        StopCoroutine("FireLaser");
        
    }

    public float GetDistance()
    {
        return distance;
    }

    public RaycastHit getRaycastHit()
    {
        return rayHit;
    }

    public Vector3 getLaserDirection()
    {
        return laserDirection;
    }

    IEnumerator FireLaser()
    {
        while (true)
        {
            if (active)
            {
                Ray ray = new Ray(transform.position, transform.forward);

                line.SetPosition(0, ray.origin);
                if (Physics.Raycast(ray, out rayHit, distanceMax, whatToCollideWith, hitTriggers))
                {
                    line.SetPosition(1, rayHit.point);
                    distance = Vector3.Distance(ray.origin, rayHit.point);
                    material.SetFloat("uDistance", distance);
                    Vector2 texcoord = rayHit.textureCoord;
                    //Debug.Log(texcoord);

                    texcoord.x = Random.Range(0.0f, 1.0f);
                    texcoord.y = Random.Range(0.0f, 1.0f);

                    if (rayHit.collider.gameObject.GetComponent<MeshRenderer>() != null)
                    {
                        Texture2D tex = (Texture2D)rayHit.collider.gameObject.GetComponent<MeshRenderer>().material.GetTexture("_MainTex");
                        //rayHit.collider.gameObject.GetComponent<MeshRenderer>().material.
                        if(rayHit.collider.gameObject.GetComponent<MeshRenderer>().material.HasProperty("_Metallic"))
                            metallicHit = rayHit.collider.gameObject.GetComponent<MeshRenderer>().material.GetFloat("_Metallic");
                        //Debug.Log(metallicHit);
                        colorHit = Color.white;

                        //if (tex != null)
                        //colorHit = tex.GetPixel((int)(tex.width * texcoord.x), (int)(tex.height * texcoord.y));
                    }
                    else
                    {
                        colorHit = Color.black;
                    }

                    if (rayHit.collider.gameObject.GetComponent<Terrain>() != null)
                    {
                        metallicHit = 1.0f;
                    }
                }
                else
                {
                    line.SetPosition(1, ray.GetPoint(distanceMax));
                    material.SetFloat("uDistance", distanceMax);
                }

                laserDirection = Vector3.Normalize(line.GetPosition(1) - line.GetPosition(0));
                if(light != null)
                    light.transform.position = line.GetPosition(1) + laserDirection * 0.05f;
                else
                    Debug.Log("Light is null!");
            }

            yield return null;
        }
    }
}
