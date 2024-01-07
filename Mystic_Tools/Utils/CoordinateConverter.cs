namespace Mystic_Tools.Utils
{
    /// <summary>
    /// 座標を変換するクラスです。
    /// </summary>
    internal class CoordinateConverter
    {

        /// <summary>
        /// CoordinateConverterクラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        public CoordinateConverter(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.ratio = Math.Min(Width / mapWidth, Height / mapHeight);
        }

        /// <summary>
        /// キーを座標に変換します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>座標</returns>
        public int[] KeyToCoordinate(string key)
        {
            if (!keyMap.ContainsKey(key))
            {
                return new[] { -1, -1 };
            }

            return keyMap[key].Select(x => (int)(x * ratio)).ToArray();
        }

        /// <summary>
        /// キーのインデックスを座標に変換します。
        /// </summary>
        /// <param name="keyIndex">キーのインデックス</param>
        /// <returns>座標</returns>
        public int[] KeyIndexToCoordinate(int keyIndex)
        {
            if (keyIndex < 0 || keyIndex >= keyMap.Count)
            {
                return new[] { -1, -1 };
            }

            return KeyToCoordinate(keyMap.ElementAt(keyIndex).Key);
        }

        private readonly int Width, Height;

        private float ratio;

        private const float mapWidth = 3767;

        private const float mapHeight = 1227;

        private readonly Dictionary<string, int[]> keyMap = new()
        {
            {"ESC", [64, 32]},
            {"F1", [482, 30]},
            {"F2", [694, 36]},
            {"F3", [897, 32]},
            {"F4", [1110, 34]},
            {"F5", [1417, 30]},
            {"F6", [1622, 34]},
            {"F7", [1834, 34]},
            {"F8", [2041, 30]},
            {"F9", [2352, 36]},
            {"F10", [2563, 32]},
            {"F11", [2770, 36]},
            {"F12", [2980, 34]},
            {"PRINT", [3251, 34]},
            {"SCROLL", [3456, 32]},
            {"PAUSE", [3667, 28]},
            {"INSERT", [3249, 273]},
            {"HOME", [3456, 271]},
            {"PGUP", [3667, 273]},
            {"DEL", [3243, 483]},
            {"END", [3450, 473]},
            {"PGDN", [3673, 477]},
            {"UP_ARROW", [3454, 906]},
            {"L_ARROW", [3249, 1119]},
            {"DN_ARROW", [3450, 1113]},
            {"R_ARROW", [3665, 1115]},
            {"TILDE", [72, 281]},
            {"1", [247, 267]},
            {"2", [460, 271]},
            {"3", [690, 273]},
            {"4", [905, 273]},
            {"5", [1110, 269]},
            {"6", [1313, 273]},
            {"7", [1521, 271]},
            {"8", [1728, 271]},
            {"9", [1945, 275]},
            {"0", [2169, 273]},
            {"NEG", [2324, 275]},
            {"EQUATION", [2533, 275]},
            {"BACKSPACE", [2980, 283]},
            {"TAB", [120, 483]},
            {"Q", [383, 483]},
            {"W", [592, 483]},
            {"E", [769, 483]},
            {"R", [1009, 479]},
            {"T", [1214, 473]},
            {"Y", [1419, 477]},
            {"U", [1628, 485]},
            {"I", [1836, 479]},
            {"O", [2039, 483]},
            {"P", [2246, 477]},
            {"L_BRACKETS", [2424, 483]},
            {"R_BRACKETS", [2661, 481]},
            {"CAP", [153, 682]},
            {"A", [438, 686]},
            {"S", [660, 690]},
            {"D", [857, 688]},
            {"F", [1060, 690]},
            {"G", [1272, 686]},
            {"H", [1481, 688]},
            {"J", [1688, 692]},
            {"K", [1889, 690]},
            {"L", [2087, 694]},
            {"SEMICOLON", [2282, 686]},
            {"APOSTROPHE", [2485, 680]},
            {"ENTER", [2954, 601]},
            {"L_SHIFT", [205, 912]},
            {"Z", [502, 898]},
            {"X", [737, 900]},
            {"C", [947, 898]},
            {"V", [1150, 896]},
            {"B", [1361, 896]},
            {"N", [1567, 896]},
            {"M", [1772, 894]},
            {"COMMA", [1981, 900]},
            {"DOT", [2184, 900]},
            {"SLASH", [2380, 900]},
            {"R_SHIFT", [2900, 910]},
            {"L_CTRL", [116, 1093]},
            {"L_WIN", [403, 1117]},
            {"L_ALT", [662, 1097]},
            {"SPACE", [1313, 1081]},
            {"R_ALT", [2176, 1095]},
            {"R_WIN", [2432, 1115]},
            {"R_CTRL", [2954, 1101]},
            {"FN0", [2687, 1125]},
            {"CODE14", [2737, 269]},
            {"CODE42", [2719, 676]},
            {"CODE56", [2575, 890]},
            {"CODE131", [895, 1097]},
            {"CODE132", [1734, 1089]},
            {"CODE133", [1945, 1085]}
        };
    }
}
