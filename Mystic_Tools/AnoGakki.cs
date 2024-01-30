using Mystic_Tools.Shapes;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenCvSharp.FileStorage;

namespace Mystic_Tools
{
    internal class AnoGakki : IAnimation
    {

        public AnoGakki(Keyboard keyboard) 
        {
            this.keyboard = keyboard;
            mapWidth = keyboard.mapWidth;
            mapHeight = keyboard.mapHeight;
            backgroundImage = new KeyboardImage(mapWidth, mapHeight);
            shapes = new List<Shape>();
        }

        public void Start()
        {
            Circle circle = new Shapes.Circle();
            circle.Radius = 200;
            circle.Thickness = 250;
            circle.Color = new OpenCvSharp.Scalar(255, 200, 50);
            circle.Location = new OpenCvSharp.Point(500, 600);


            //backgroundImage.Image.SaveImage("matframe.png");

            shapes.Add(circle);
            while (!Cts.Token.IsCancellationRequested)
            {
                Console.WriteLine("AnoGakki");
                DrawShapes();
                circle.Radius += 100;

                Bitmap frameBitmap = BitmapConverter.ToBitmap(backgroundImage.Image);
                frameBitmap.Save("frame.png");
                for (int i = 0; i < 92; i++)
                {
                    int[] keyCoordinate = keyboard.KeyIndexToCoordinate(i, mapWidth, mapHeight);
                    keyboard.SetCustomizeRGBColor(i, frameBitmap.GetPixel(keyCoordinate[0], keyCoordinate[1]));
                }

                keyboard.SetCustomize(255);
                Thread.Sleep(80);

            }
        }

        public void Stop()
        {
            Console.WriteLine("AnoGakki Stop");
            Cts.Cancel();
        }

        public void DrawShapes()
        {
            backgroundImage = new KeyboardImage(mapWidth, mapHeight);
            foreach (var shape in shapes)
            {
                shape.Draw(backgroundImage.Image);
            }
        }

        public KeyboardImage backgroundImage { get; set; }
        public List<Shape> shapes { get; set; }
        public CancellationTokenSource Cts { get; set; } = new CancellationTokenSource();

        public Keyboard keyboard { get; set; }

        private int mapWidth, mapHeight;
    }
}
