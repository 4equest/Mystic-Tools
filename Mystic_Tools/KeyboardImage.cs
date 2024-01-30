using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mystic_Tools.Shapes;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Mystic_Tools
{
    internal class KeyboardImage
    {
        public KeyboardImage(int width, int height)
        {
            Image = new Mat(height, width, MatType.CV_8UC3, new Scalar(0,0,0));
        }
        public Mat Image { get; private set; }
    }
}
