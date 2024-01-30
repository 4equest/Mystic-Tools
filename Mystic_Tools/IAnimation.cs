using Mystic_Tools.Shapes;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_Tools
{
    internal interface IAnimation
    {
        void Start();
        void Stop();

        KeyboardImage backgroundImage { get; set; }
        List<Shape> shapes { get; set; }

        CancellationTokenSource Cts { get; set; }

        Keyboard keyboard { get; set; }

        private const float mapWidth = 3767;

        private const float mapHeight = 1227;
    }
}
