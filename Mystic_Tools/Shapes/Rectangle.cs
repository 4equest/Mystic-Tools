using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_Tools.Shapes
{
    public class Rectangle : Shape
    {
        public Size Size { get; set; }

        public override void Draw(Mat image)
        {
            Cv2.Rectangle(image, new Rect(Location, Size), Color, Thickness);
        }
    }
}
