using UnityEngine;
using System.Collections;

public class GunLaserScript : MonoBehaviour
{
    public LineRenderer line;
    float distanceMax = 100.0f;
    QueryTriggerInteraction hitTriggers;
    public LayerMask whatToCollideWith;
    public Material material;

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
