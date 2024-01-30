using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_Tools.Shapes
{
    public class Circle : Shape
    {
        public int Radius { get; set; }

        public override void Draw(Mat image)
        {
            Cv2.Circle(image, Location, Radius, Color, Thickness);
        }
    }
}
