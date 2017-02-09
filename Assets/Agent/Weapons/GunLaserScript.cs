using UnityEngine;
using System.Collections;

public class GunLaserScript : MonoBehaviour
{
    public LineRenderer line;
    float distanceMax = 100.0f;
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

    IEnumerator FireLaser()
    {
        while (true)
        {
            if (active)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;

                line.SetPosition(0, ray.origin);
                if (Physics.Raycast(ray, out hit, distanceMax, whatToCollideWith, hitTriggers))
                {
                    line.SetPosition(1, hit.point);
                    material.SetFloat("uDistance", Vector3.Distance(ray.origin, hit.point));
                    Vector2 texcoord = hit.textureCoord;
                    Debug.Log(texcoord);

                    if (hit.collider.gameObject.GetComponent<MeshRenderer>() != null)
                    {
                        Texture2D tex = (Texture2D)hit.collider.gameObject.GetComponent<MeshRenderer>().material.GetTexture("_MainTex");
                        metallicHit = hit.collider.gameObject.GetComponent<MeshRenderer>().material.GetFloat("_Metallic");
                        //Debug.Log(metallicHit);
                        if (tex != null)
                            colorHit = tex.GetPixel((int)(tex.width * texcoord.x), (int)(tex.height * texcoord.y));
                    }
                    else
                    {
                        colorHit = Color.black;
                    }

                    if (hit.collider.gameObject.GetComponent<Terrain>() != null)
                    {
                        metallicHit = 1.0f;
                    }
                }
                else
                {
                    line.SetPosition(1, ray.GetPoint(distanceMax));
                    material.SetFloat("uDistance", distanceMax);
                }
            }

            yield return null;
        }
    }
}
