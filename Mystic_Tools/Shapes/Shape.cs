using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_Tools.Shapes
{
    public abstract class Shape
    {
        public Point Location { get; set; }
        public Scalar Color { get; set; }
        public int Thickness { get; set; }

        public abstract void Draw(Mat image);
    }

}
