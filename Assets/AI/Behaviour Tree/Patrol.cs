using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {

    private int index = 0;
    public float travelTime = 1;
    public float resolution = 100;
    public float vel = 0.01f;
    public bool drawPath = false;
    public bool rebuildCurve = false;
    private List<SpeedControlCurve> curve = new List<SpeedControlCurve>();

    private struct SpeedControlCurve
    {
        public Vector3 v;
        public float tval;
        public float arcl;
        public int seg;
    }

    private float arclength(int i)
    {
        Vector3 d = curve[i].v + (curve[i - 1].v * -1);
        float ad = Mathf.Sqrt(Mathf.Pow(d.x, 2) + Mathf.Pow(d.y, 2) + Mathf.Pow(d.z, 2));
        return ad + curve[i - 1].arcl;
    }

    public void Update()
    {
        if (drawPath)
            DrawDebug();
        if (rebuildCurve)
            rebuild();
    }

    public void rebuild()
    {
        rebuildCurve = false;
        curve.RemoveRange(0, curve.Count);
        Start();
    }

    public void Start()
    {
        //build curve
        float t = 0;
        int numchildren = transform.childCount;
        bool curveComplete = false;
        int cycles = 0;
        index = 0;
        while (!curveComplete && numchildren > 1)
        {
            Vector3 point = Vector3.zero;
            t += 1 / resolution;
            float t2 = t * t;
            float t3 = t2 * t;

            //redoing logic
            int prevIndex = index - 1 < 0 ? index - 1 + numchildren : index - 1;
            index = index == numchildren ? 0 : index;
            int nextIndex = index + 1 > numchildren - 1 ? index + 1 - numchildren : index + 1;
            int lastIndex = index + 2 > numchildren - 1 ? index + 2 - numchildren : index + 2;

            //print("Cycle: " + cycles + "\nIndex: " + index + "\n" + prevIndex + ":" + index + ":" + nextIndex + ":" + lastIndex);

            //Catmull
            Vector3 prev = transform.GetChild(prevIndex).transform.position;
            Vector3 current = transform.GetChild(index).transform.position;
            Vector3 next = transform.GetChild(nextIndex).transform.position;
            Vector3 last = transform.GetChild(lastIndex).transform.position;


            point.x = ((-t3 + 2 * t2 - t) * (prev.x) + (3 * t3 - 5 * t2 + 2) * (current.x) + (-3 * t3 + 4 * t2 + t) * (next.x) + (t3 - t2) * (last.x)) / 2;
            point.y = ((-t3 + 2 * t2 - t) * (prev.y) + (3 * t3 - 5 * t2 + 2) * (current.y) + (-3 * t3 + 4 * t2 + t) * (next.y) + (t3 - t2) * (last.y)) / 2;
            point.z = ((-t3 + 2 * t2 - t) * (prev.z) + (3 * t3 - 5 * t2 + 2) * (current.z) + (-3 * t3 + 4 * t2 + t) * (next.z) + (t3 - t2) * (last.z)) / 2;


            SpeedControlCurve c;
            c.v = point;
            c.seg = index;
            c.tval = t;
            c.arcl = 0;
            curve.Add(c);

            if (cycles >= numchildren)
                curveComplete = true;
            if (t >= 1)
            {
                index++;
                cycles++;
                t = 0;
            }

        }
        //distance function
        //mag(p2 - p1) + alp1
        for (int i = 1; i < curve.Count; i++)
        {
            var temp = curve[i];
            temp.arcl = arclength(i);
        }

        for (int i = 0; i < curve.Count; i++)
        {
            var temp = curve[i];
            temp.arcl /= curve[curve.Count - 1].arcl;
        }
    }

	public Vector3 PathPosition(float t)
    {
        Vector3 p;

        //stage 1 lerp for distance
        //d is incremented by vel and reset to 0
        float dst = Mathf.Lerp(0.0f, 1.0f, t);
        //stage 2 find closest points
        int p1 = 0, p2 = 0;
        for (int i = 1; i < curve.Count; i++)
        {
            if (curve[i].arcl > dst)
            {
                p2 = i;
                p1 = i - 1;
                continue;
            }
        }

        float il = Mathf.InverseLerp(curve[p1].arcl, curve[p2].arcl, dst);
        p = Vector3.Lerp(curve[p1].v, curve[p2].v, il);
        
        return p;
    }

    public Vector3 NextPoint(Vector3 prevPoint)
    {
        for (int i = 0; i < curve.Count; i++)
        {
            if (curve[i].v == prevPoint)
                return curve[i + 1 >= curve.Count ? 0 : i].v;
        }
        return curve[0].v;
    }

    public void DrawDebug()
    {
        for (int i = 0; i < curve.Count; i++)
        {
            int nextIndex = i + 1 > curve.Count - 1 ? i + 1 - curve.Count : i + 1;
            Debug.DrawLine(curve[i].v, curve[nextIndex].v, Color.red);
        }
    }
}
