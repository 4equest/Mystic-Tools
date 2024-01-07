using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Mystic_Tools
{
    /// <summary>
    /// ビデオフレームの解析を行うクラスです。
    /// </summary>
    public class VideoFrameAnalyzer
    {
        /// <summary>
        /// VideoFrameAnalyzerクラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="videoPath">ビデオファイルのパス</param>
        public VideoFrameAnalyzer(string videoPath)
        {
            capture = new VideoCapture(videoPath);
            frame = new Mat();
            FrameRate = capture.Get(VideoCaptureProperties.Fps);
        }

        /// <summary>
        /// 指定したフレーム番号のフレームの色を取得します。
        /// </summary>
        /// <param name="frameNumber">フレーム番号</param>
        /// <returns>フレームの色</returns>
        public Bitmap GetFrameColorAtFrame(int frameNumber)
        {
            capture.PosFrames = frameNumber;
            capture.Read(frame);

            return frame.ToBitmap();
        }

        /// <summary>
        /// 指定したフレーム番号の指定した座標のピクセルの色を取得します。
        /// </summary>
        /// <param name="frameNumber">フレーム番号</param>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>ピクセルの色</returns>
        public Color GetPixelColorAtFrame(int frameNumber, int x, int y)
        {
            Bitmap frameBitmap = GetFrameColorAtFrame(frameNumber);
            return frameBitmap.GetPixel(x, y);
        }

        private readonly VideoCapture capture;

        private readonly Mat frame;

        public double FrameRate = 30.0;

        public int FrameCount => (int)capture.Get(VideoCaptureProperties.FrameCount);
    }
}
