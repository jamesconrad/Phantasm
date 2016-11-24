using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

//Used for importing light object information from a maya ascii file

    //MODELLING RULES:
    //ALL LIGHTS MUST START WITH typeLight
    //ALL CURVES MUST START WITH curve
    //NO OTHER OBJECTS MAY BEGIN WITH THE ABOVE

    //Missing Features:
    //

namespace MayaImporter
{
    public class Vec3
    {
        public Vec3(float _x, float _y, float _z)
        {
            x = _x; y = _y; z = _z;
        }
        public Vec3()
        {
            x = y = z = 0;
        }
        public float x, y, z;
    }

    public class Curve
    {
        public Curve()
        {
            s = new Vec3(1, 1, 1);
        }
        public Vec3 t;
        public Vec3 s;
        public Vec3 r;
        public int numpoints;
        public List<Vec3> points;
        public List<Vec3> pointVectors;
        public string name; 
    }

    public class Light
    {
        public Light()
        {
            s = new Vec3(1, 1, 1);
        }
        public Vec3 t;
        public Vec3 s;
        public Vec3 r;
        public string type;
        public string name;
    }

    public class MI
    {
        int error;
        string errors;

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

        Vec3 SplitStringToVec3(string[] s, int xind, int yind, int zind)
        {
            return new Vec3(float.Parse(s[xind]), float.Parse(s[yind]), float.Parse(s[zind]));
        }

        public List<Curve> ParseCurves(string filepath)
        {
            List<Curve> ret = new List<Curve>();

            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();
                        if (line.Contains("createNode transform -n \"curve"))
                        {
                            Curve c = new Curve();
                            c.points = new List<Vec3>();
                            c.pointVectors = new List<Vec3>();
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
                                    string line1 = sr.ReadLine(); //This line in the ma file confuses me, TODO learn what it means
                                    string line2 = sr.ReadLine(); //index for knots? Not sure, test file has 2 indicies more than points
                                    //num1 = number of numbers; numbers 2 to end are indicies?
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
                    }
                }
            }
            catch (Exception e)
            {
                error = 404;
                errors = e.Message;
            }

            return ret;
        }

        public List<Light> ParseLights(string filepath)
        {
            List<Light> ret = new List<Light>();

            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
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
                }
            }
            catch (Exception e)
            {
                error = 404;
                errors = e.Message;
            }

            return ret;
        }

        public int GetError()
        {
            int tmp = error;
            error = 200;
            return error;
        }

        public string ErrorString(int error)
        {
            if (error == 200)
                return "OK";
            return errors;
        }
    }
}
