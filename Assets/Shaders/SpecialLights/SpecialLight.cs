using UnityEngine;

[ExecuteInEditMode]
public class SpecialLight : MonoBehaviour
{
    public enum LightType
    {
        Sphere,
        Cylinder,
        SomethingElse   //Maybe I'll make more later
    }
    public LightType _LightType;

    public Color _Color = Color.white;
    public float _Intensity = 1.0f;
    public float _Range = 10.0f;
    public float _Radius = 0.5f;
    public float _Length = 1.0f;

    //These are all self explanitory, _Range is the dropoff for the attenuation, _Length is only applied to the 

    public void OnEnable()
    {
        SpecialLightSystem.instance.Add(this);
    }

    public void Start()
    {
        SpecialLightSystem.instance.Add(this);
    }

    public void OnDisable()
    {
        SpecialLightSystem.instance.Remove(this);
    }

    public Color GetLinearColor()
    {
        return new Color(
            Mathf.GammaToLinearSpace(_Color.r * _Intensity),
            Mathf.GammaToLinearSpace(_Color.g * _Intensity),
            Mathf.GammaToLinearSpace(_Color.b * _Intensity),
            1.0f
        );
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, _LightType == LightType.Cylinder ? "AreaLight Gizmo" : "PointLight Gizmo", true);
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.1f, 0.7f, 1.0f, 0.6f);
        if (_LightType == LightType.Cylinder)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, new Vector3(_Length * 2, _Radius * 2, _Radius * 2));
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
        else
        {
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawWireSphere(transform.position, _Radius);
        }
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawWireSphere(transform.position, _Range);
        //It's a sphere, the orientation of it doesn't matter
    }
}
