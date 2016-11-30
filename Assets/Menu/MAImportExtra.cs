using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MayaImporter;

public class MAImportExtra : MonoBehaviour {

    List<Curve> curves;
    MayaImp importer;
    public float t;
    public float tmod;
    public bool anim;
    public bool animComplete;

	public void Begin()
	{
		t = 0;
		anim = true;
	}

    // Use this for initialization
    void Start () {
        t = 0;
        tmod = 0.005f;
        anim = false;
        animComplete = false;
        importer = new MayaImp();
        curves = new List<Curve>();
        //print(Application.dataPath + "/Menu/Menu Scene/menu scene.ma");
        curves = importer.ParseCurves(Application.dataPath + "/Menu/Menu Scene/menu scene.ma");
        GameObject camera = GameObject.Find("Main Camera");
        //initial positionng

        Vector4 temp = transform.localToWorldMatrix * new Vector4(curves[1].points[0].x,
                                                                  curves[1].points[0].y,
                                                                  curves[1].points[0].z, 1);

        camera.transform.position = new Vector3(temp.x, temp.y, temp.z);
        
        temp = transform.localToWorldMatrix * new Vector4(curves[0].points[0].x,
                                                          curves[0].points[0].y,
                                                          curves[0].points[0].z, 1);
        camera.transform.LookAt(temp);

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (anim)
        {
            t += tmod;
            if (t >= 1)
            {
                t = 1;
                //tmod = 0;
                animComplete = true;
            }
        }
        List<Vector3> buffer = new List<Vector3>();
        for (int i = 0; i < curves[1].points.Count; i++)
            buffer.Add(curves[1].points[i]);

        Vector3 p = importer.Bezier(ref buffer, t, 0);

        buffer = new List<Vector3>();
        for (int i = 0; i < curves[0].points.Count; i++)
            buffer.Add(curves[0].points[i]);

        Vector3 l = importer.Bezier(ref buffer, t, 0);

        p = ConvertToWorldSpace(p);
        p.x *= -1;
        l = ConvertToWorldSpace(l);
        l.x *= -1;

        GameObject camera = GameObject.Find("Main Camera");
        camera.transform.position = p;
        camera.transform.LookAt(l);
    }

    Vector3 ConvertToWorldSpace(Vector3 v)
    {
        Vector4 temp = transform.localToWorldMatrix * new Vector4(v.x, v.y, v.z, 1);
        return new Vector3(temp.x, temp.y, temp.z);
    }
}
