using System.Drawing;

namespace Utils
{
    /// <summary>
    /// HSV色空間からRGB色空間への変換を提供するクラスです。
    /// </summary>
    internal class HSVtoRGB
    {
        /// <summary>
        /// HSV色空間からRGB色空間へ変換します。
        /// </summary>
        /// <param name="h">色相</param>
        /// <param name="s">彩度</param>
        /// <param name="v">明度</param>
        /// <returns>RGB色空間の色</returns>
        public static Color GetHSVtoRGB(double h, double s, double v)
        {
            h = h % 360;
            if (h < 0) h += 360;

            int hi = (int)(h / 60) % 6;
            double f = h / 60 - Math.Floor(h / 60);

            byte vByte = (byte)(255 * v);
            byte p = (byte)(255 * v * (1 - s));
            byte q = (byte)(255 * v * (1 - f * s));
            byte t = (byte)(255 * v * (1 - (1 - f) * s));

            switch (hi)
            {
                case 0: return Color.FromArgb(vByte, t, p);
                case 1: return Color.FromArgb(q, vByte, p);
                case 2: return Color.FromArgb(p, vByte, t);
                case 3: return Color.FromArgb(p, q, vByte);
                case 4: return Color.FromArgb(t, p, vByte);
                default: return Color.FromArgb(vByte, p, q);
            }
        }
    }
}
