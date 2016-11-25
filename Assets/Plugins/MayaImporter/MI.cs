using System.Collections.Generic;
using System.IO;
using UnityEngine;

//Used for importing light object information from a maya ascii file

//MODELLING RULES:
//ALL LIGHTS MUST START WITH typeLight
//ALL CURVES MUST START WITH curve
//NO OTHER OBJECTS MAY BEGIN WITH THE ABOVE

//Missing Features:
//

namespace MayaImporter
{
    public class Curve : MonoBehaviour
    {
        public Curve()
        {
            s = new Vector3(1, 1, 1);
        }
        public Vector3 t;
        public Vector3 s;
        public Vector3 r;
        public int numpoints;
        public List<Vector3> points;
        public string mayaname;
        public List<int> index;
        public int power;
    }

    public class Light : MonoBehaviour
    {
        public Light()
        {
            s = new Vector3(1, 1, 1);
        }
        public Vector3 t;
        public Vector3 s;
        public Vector3 r;
        public string type;
        public string mayaname;
    }

    public class MI : MonoBehaviour
    {
        int error;
        //string errors;

        public MI()
        {
            error = 200;
        }

        ~MI()
        {

        }

        string[] SplitString(string s)
        {
            return s.Split(new char[] { ' ', ',', '.', ':', '\t', ';' });
        }

        Vector3 SplitStringToVec3(string[] s, int xind, int yind, int zind)
        {
            return new Vector3(float.Parse(s[xind]), float.Parse(s[yind]), float.Parse(s[zind]));
        }

        public List<Curve> ParseCurves(string filepath)
        {
            List<Curve> ret = new List<Curve>();
            StreamReader sr = new StreamReader(filepath);
            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();
                if (line.Contains("createNode transform -n \"curve"))
                {
                    Curve c = new Curve();
                    c.points = new List<Vector3>();
                    c.name = line.Substring(30, line.Length - 32);
                    while (sr.Peek() >= 0 && !(line = sr.ReadLine()).Contains("createNode transform -n"))
                    {
                        if (line.Contains("setAttr"))
                        {
                            string args = line.Substring(7);
                            if (args.Contains("\".t\""))
                            {
                                string[] segs = SplitString(args);
                                int numsegs = segs.Length;
                                c.t = SplitStringToVec3(segs, numsegs - 3, numsegs - 2, numsegs - 1);
                            }
                            else if (args.Contains("\".r\""))
                            {
                                string[] segs = SplitString(args);
                                int numsegs = segs.Length;
                                c.r = SplitStringToVec3(segs, numsegs - 3, numsegs - 2, numsegs - 1);
                            }
                            else if (args.Contains("\".s\""))
                            {
                                string[] segs = SplitString(args);
                                int numsegs = segs.Length;
                                c.s = SplitStringToVec3(segs, numsegs - 3, numsegs - 2, numsegs - 1);
                            }
                        }
                    }
                    //All transform node data read... i think
                    while (sr.Peek() >= 0 && !(line = sr.ReadLine()).Contains("createNode"))
                    {
                        //We only care about the ".cc" arg here
                        if (line.Contains("setAttr \".cc\""))
                        {
                            string line1 = sr.ReadLine(); //This line in the ma file confuses me, first number is the power
                            c.power = int.Parse(SplitString(line1)[0]);
                            string line2 = sr.ReadLine(); //index for interpolation
                            int numind = int.Parse(SplitString(line2)[0]);
                            string[] ind = SplitString(line2);
                            for (int i = 1; i < numind; i++)
                            {
                                c.index.Add(int.Parse(ind[i]));
                            }
                            string line3 = sr.ReadLine(); //number of line points
                            c.numpoints = int.Parse(SplitString(line3)[0]);
                            for (int i = 0; i < c.numpoints; i++)
                            {
                                c.points.Add(SplitStringToVec3(SplitString(sr.ReadLine()), 0, 1, 2));
                            }
                            sr.ReadLine(); //discard semicolon line
                        }
                    }
                    ret.Add(c);
                }
                sr.Dispose();
            }

            return ret;
        }

        public List<Light> ParseLights(string filepath)
        {
            List<Light> ret = new List<Light>();
            StreamReader sr = new StreamReader(filepath);
            while (sr.Peek() >= 0)
            {
                bool light = false;
                Light c = new Light();
                string line = sr.ReadLine();
                if (line.Contains("createNode transform -n \"point"))
                {
                    light = true;
                    c.type = "point";
                }
                else if (line.Contains("createNode transform -n \"spot"))
                {
                    light = true;
                    c.type = "spot";
                }
                else if (line.Contains("createNode transform -n \"area"))
                {
                    light = true;
                    c.type = "area";
                }
                if (light)
                {
                    c.name = line.Substring(25, line.Length - 27);
                    while (sr.Peek() >= 0 && !(line = sr.ReadLine()).Contains("createNode transform -n"))
                    {
                        if (line.Contains("setAttr"))
                        {
                            string args = line.Substring(7);
                            if (args.Contains("\".t\""))
                            {
                                string[] segs = SplitString(args);
                                int numsegs = segs.Length;
                                c.t = SplitStringToVec3(segs, numsegs - 3, numsegs - 2, numsegs - 1);
                            }
                            else if (args.Contains("\".r\""))
                            {
                                string[] segs = SplitString(args);
                                int numsegs = segs.Length;
                                c.r = SplitStringToVec3(segs, numsegs - 3, numsegs - 2, numsegs - 1);
                            }
                            else if (args.Contains("\".s\""))
                            {
                                string[] segs = SplitString(args);
                                int numsegs = segs.Length;
                                c.s = SplitStringToVec3(segs, numsegs - 3, numsegs - 2, numsegs - 1);
                            }
                        }
                    }
                }
                ret.Add(c);
            }
            sr.Dispose();
            return ret;
        }

        public int GetError()
        {
            int tmp = error;
            error = 200;
            return error;
        }

        //public string ErrorString(int error)
        //{
        //    if (error == 200)
        //        return "OK";
        //    return errors;
        //}
    }
}
