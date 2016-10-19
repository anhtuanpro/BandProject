using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BandProject
{
    public class Point
    {
        public String gesture;
        public String SampleName;
        public double accX, accY, accZ;
        public double gyX, gyY, gyZ;
        public Point() { }
        public void setAccelerometter(double x, double y, double z)
        {
            accX = x;
            accY = y;
            accZ = z;
        }
        public void setGyroscope(double x1, double y1, double z1)
        {
            gyX = x1;
            gyY = y1;
            gyZ = z1;
        }
        public void setGesture(String name)
        {
            gesture = name;
        }
    }
}
