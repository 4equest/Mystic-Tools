using System.Drawing;
using MsiHid;

namespace Mystic_Tools
{

    /// <summary>
    /// キーボードの設定を行うクラスです。
    /// </summary>
    public class GK50LP_TKL : Keyboard
    {

        public GK50LP_TKL()
        {
            for (int i = 0; i < Pages.Length; i++)
            {
                Pages[i] = new byte[65];
            }

            mapWidth = 3767;
            mapHeight = 1227;
        }

        /// <summary>
        /// キーボードの言語を取得します。
        /// </summary>
        public void GetLanguage()
        {
            ushort zero = 0;
            ushort versionCode = 0;
            HID_Basic.GetAttributes(device, ref zero, ref zero, ref versionCode);
            string text = versionCode.ToString("X");
            if (text == "10")
            {
                Language = EnumLanguage.US;
                return;
            }
            if (text == "20")
            {
                Language = EnumLanguage.EU;
                return;
            }
            if (text == "30")
            {
                Language = EnumLanguage.JP;
                return;
            }
            if (!(text == "40"))
            {
                return;
            }
            Language = EnumLanguage.KR;
        }

        /// <summary>
        /// キーボードの設定の読み書きの初期化を行います。
        /// </summary>
        /// <returns>初期化が成功した場合はtrue、それ以外の場合はfalse。</returns>
        public bool Init()
        {
            device = HID_Basic.openMyDevice_Read(3504, 2908, 1, 0, 1);
            if (device != nint.Zero)
            {
                GetLanguage();
                EnableRemoteControl();
                GetCustomize();
                return true;
            }
            return false;
        }

        public bool CloseDevice()
        {
            DisableRemoteControl();
            if (HID_Basic.CloseDevice(device) == 1)
            {
                device = nint.Zero;
                return true;
            }
            return false;
        }

        public void ReadFWVersion()
        {
            Array.Clear(SetFeatureData, 0, 65);
            SetFeatureData[1] = 18;
            SetFeatureData[2] = 32;
            HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            while (GetFeatureData[1] != 18 || GetFeatureData[2] != 32)
            {
                Array.Clear(SetFeatureData, 0, 65);
                SetFeatureData[1] = 18;
                SetFeatureData[2] = 32;
                HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
                HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
                Thread.Sleep(10);
            }
            Version = string.Concat(new string[]
            {
                Convert.ToChar(GetFeatureData[11]).ToString(),
                ".",
                Convert.ToChar(GetFeatureData[15]).ToString(),
                Convert.ToChar(GetFeatureData[17]).ToString(),
                ".",
                Convert.ToChar(GetFeatureData[21]).ToString(),
                Convert.ToChar(GetFeatureData[23]).ToString()
            });
            OldVersion = Convert.ToChar(GetFeatureData[21]).ToString() + Convert.ToChar(GetFeatureData[23]).ToString();
            CloseDevice();
        }

        public void EnableRemoteControl()
        {
            Array.Clear(SetFeatureData, 0, 65);
            SetFeatureData[1] = 65;
            SetFeatureData[2] = 128;
            HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
        }

        public void DisableRemoteControl()
        {
            Array.Clear(SetFeatureData, 0, 65);
            SetFeatureData[1] = 65;
            SetFeatureData[2] = 0;
            HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
        }

        public void GetStyle()
        {
            Array.Clear(SetFeatureData, 0, 65);
            SetFeatureData[1] = 82;
            SetFeatureData[2] = 40;
            HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            while (GetFeatureData[1] != 82 || GetFeatureData[2] != 40)
            {
                Thread.Sleep(5);
                Array.Clear(SetFeatureData, 0, 65);
                SetFeatureData[1] = 82;
                SetFeatureData[2] = 40;
                HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
                HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            }
            CurrentStyle = GetFeatureData[5];
            GetLEDData(CurrentStyle);
            GetRGBColor(CurrentStyle);
        }

        public void GetLEDData(byte Style)
        {
            switch (Style)
            {
                case 0:
                    GetPages(0);
                    CurrentBrightness = Pages[0][16];
                    return;
                case 1:
                    GetPages(1);
                    CurrentR = Pages[1][49];
                    CurrentG = Pages[1][50];
                    CurrentB = Pages[1][51];
                    CurrentBrightness = Pages[1][52];
                    return;
                case 2:
                    GetPages(2);
                    CurrentR = Pages[2][9];
                    CurrentG = Pages[2][10];
                    CurrentB = Pages[2][11];
                    CurrentBrightness = Pages[2][12];
                    CurrentSpeed = Pages[2][15];
                    return;
                case 3:
                    GetPages(2);
                    CurrentR = Pages[2][33];
                    CurrentG = Pages[2][34];
                    CurrentB = Pages[2][35];
                    CurrentBrightness = Pages[2][36];
                    CurrentRGBColor = Pages[2][37];
                    CurrentDirection = Pages[2][38];
                    CurrentSpeed = Pages[2][29];
                    return;
                case 4:
                    GetPages(2);
                    CurrentR = Pages[2][57];
                    CurrentG = Pages[2][58];
                    CurrentB = Pages[2][59];
                    CurrentBrightness = Pages[2][60];
                    CurrentDirection = Pages[2][62];
                    CurrentFadeOutSpeed = Pages[2][63];
                    CurrentSpeed = Pages[2][53];
                    return;
                case 5:
                    GetPages(3);
                    CurrentR = Pages[3][21];
                    CurrentG = Pages[3][22];
                    CurrentB = Pages[3][23];
                    CurrentBrightness = Pages[3][24];
                    CurrentDirection = Pages[3][26];
                    CurrentFadeOutSpeed = Pages[3][27];
                    CurrentSpeed = Pages[3][17];
                    return;
                case 6:
                    GetPages(3);
                    CurrentBrightness = Pages[3][52];
                    CurrentDirection = Pages[3][55];
                    CurrentSpeed = Pages[3][45];
                    return;
                case 7:
                    GetPages(4);
                    CurrentBrightness = Pages[4][20];
                    CurrentDirection = Pages[4][23];
                    CurrentSpeed = Pages[4][13];
                    return;
                case 8:
                    GetPages(4);
                    CurrentR = Pages[4][45];
                    CurrentG = Pages[4][46];
                    CurrentB = Pages[4][47];
                    CurrentBrightness = Pages[4][48];
                    CurrentDirection = Pages[4][50];
                    CurrentFadeOutSpeed = Pages[4][51];
                    CurrentSpeed = Pages[4][41];
                    return;
                case 9:
                    GetPages(5);
                    CurrentR = Pages[5][9];
                    CurrentG = Pages[5][10];
                    CurrentB = Pages[5][11];
                    CurrentBrightness = Pages[5][12];
                    CurrentDirection = Pages[5][14];
                    CurrentFadeOutSpeed = Pages[5][20];
                    CurrentSpeed = Pages[5][5];
                    return;
                case 10:
                    GetPages(5);
                    CurrentR = Pages[5][33];
                    CurrentG = Pages[5][34];
                    CurrentB = Pages[5][35];
                    CurrentBrightness = Pages[5][36];
                    CurrentDirection = Pages[5][42];
                    CurrentSpeed = Pages[5][29];
                    return;
                case 11:
                    GetPages(5);
                    CurrentBrightness = Pages[5][60];
                    CurrentR = byte.MaxValue;
                    CurrentG = 0;
                    CurrentB = 0;
                    return;
                default:
                    return;
            }
        }

        /// <summary>
        /// 指定したページ番号のページを取得します。
        /// </summary>
        /// <param name="pageNumber">ページ番号</param>
        public void GetPages(byte pageNumber)
        {
            Array.Clear(SetFeatureData, 0, 65);
            SetFeatureData[1] = 86;
            SetFeatureData[2] = 32;
            SetFeatureData[3] = pageNumber;
            HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            while (GetFeatureData[1] != 86 || GetFeatureData[2] != 32 || GetFeatureData[3] != pageNumber)
            {
                Thread.Sleep(5);
                Array.Clear(SetFeatureData, 0, 65);
                SetFeatureData[1] = 86;
                SetFeatureData[2] = 32;
                SetFeatureData[3] = pageNumber;
                HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
                HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            }
            for (int i = 0; i < 65; i++)
            {
                Pages[pageNumber][i] = GetFeatureData[i];
            }
        }


        public void GetRGBColor(byte Style)
        {
            if (Style == 2 || Style == 4 || Style == 5 || Style == 8 || Style == 9 || Style == 10)
            {
                Array.Clear(SetFeatureData, 0, 65);
                SetFeatureData[1] = 82;
                SetFeatureData[2] = 146;
                SetFeatureData[3] = Style;
                HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
                HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
                while (GetFeatureData[1] != 82 || GetFeatureData[2] != 146 || GetFeatureData[3] != Style)
                {
                    Array.Clear(SetFeatureData, 0, 65);
                    SetFeatureData[1] = 82;
                    SetFeatureData[2] = 146;
                    SetFeatureData[3] = Style;
                    HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
                    HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
                    Thread.Sleep(10);
                }
                CurrentRGBColor = GetFeatureData[5];
            }
        }

        public void SetRGBColor(byte Style, bool bRGBColor)
        {
            if (bRGBColor)
            {
                Array.Clear(SetFeatureData, 0, 65);
                SetFeatureData[1] = 81;
                SetFeatureData[2] = 146;
                SetFeatureData[3] = Style;
                HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
                HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
                return;
            }
            Array.Clear(SetFeatureData, 0, 65);
            SetFeatureData[1] = 81;
            SetFeatureData[2] = 146;
            SetFeatureData[3] = Style;
            SetFeatureData[5] = 1;
            HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
        }

        public void SetStyle(byte Style)
        {
            Array.Clear(SetFeatureData, 0, 65);
            SetFeatureData[1] = 81;
            SetFeatureData[2] = 40;
            SetFeatureData[5] = Style;
            HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            Save();
        }

        public void Save()
        {
            Array.Clear(SetFeatureData, 0, 65);
            SetFeatureData[1] = 80;
            SetFeatureData[2] = 85;
            HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
        }

        public void SetCustomOverLap(byte Brightness)
        {
            GetPages(0);
            Pages[0][2] = 33;
            Pages[0][16] = Brightness;
            Pages[0][40] = Brightness;
            HID_Basic.WriteDeviceOutput(device, Pages[0], 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            SetStyle(Convert.ToByte(LED_Style.CustomOverLap));
        }

        public void SetSteady(byte R, byte G, byte B, byte Brightness)
        {
            GetPages(1);
            Pages[1][2] = 33;
            Pages[1][49] = R;
            Pages[1][50] = G;
            Pages[1][51] = B;
            Pages[1][52] = Brightness;
            HID_Basic.WriteDeviceOutput(device, Pages[1], 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            SetStyle(Convert.ToByte(LED_Style.Steady));
        }

        public void SetBreath(byte R, byte G, byte B, byte Brightness, byte Speed, bool RGBColor)
        {
            SetRGBColor(Convert.ToByte(LED_Style.Breath), RGBColor);
            GetPages(2);
            Pages[2][2] = 33;
            Pages[2][9] = R;
            Pages[2][10] = G;
            Pages[2][11] = B;
            Pages[2][12] = Brightness;
            Pages[2][15] = Speed;
            HID_Basic.WriteDeviceOutput(device, Pages[2], 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            SetStyle(Convert.ToByte(LED_Style.Breath));
        }

        public void SetWave(byte R, byte G, byte B, byte Brightness, byte Speed, byte Direction, bool RGBColor)
        {
            GetPages(2);
            Pages[2][2] = 33;
            Pages[2][33] = R;
            Pages[2][34] = G;
            Pages[2][35] = B;
            Pages[2][36] = Brightness;
            Pages[2][37] = RGBColor ? (byte)0 : (byte)1;
            Pages[2][38] = Direction;
            Pages[2][29] = Speed;
            HID_Basic.WriteDeviceOutput(device, Pages[2], 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            SetStyle(Convert.ToByte(LED_Style.Wave));
        }

        public void SetRadar(byte R, byte G, byte B, byte Brightness, byte Speed, byte Direction, byte FadeOutSpeed, bool RGBColor)
        {
            SetRGBColor(Convert.ToByte(LED_Style.Radar), RGBColor);
            GetPages(2);
            Pages[2][2] = 33;
            Pages[2][57] = R;
            Pages[2][58] = G;
            Pages[2][59] = B;
            Pages[2][60] = Brightness;
            Pages[2][62] = Direction;
            Pages[2][63] = FadeOutSpeed;
            Pages[2][53] = Speed;
            HID_Basic.WriteDeviceOutput(device, Pages[2], 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            SetStyle(Convert.ToByte(LED_Style.Radar));
        }

        public void SetRadarSwirl(byte R, byte G, byte B, byte Brightness, byte Speed, byte Direction, byte FadeOutSpeed, bool RGBColor)
        {
            SetRGBColor(Convert.ToByte(LED_Style.RadarSwirl), RGBColor);
            GetPages(3);
            Pages[3][2] = 33;
            Pages[3][21] = R;
            Pages[3][22] = G;
            Pages[3][23] = B;
            Pages[3][24] = Brightness;
            Pages[3][26] = Direction;
            Pages[3][27] = FadeOutSpeed;
            Pages[3][17] = Speed;
            HID_Basic.WriteDeviceOutput(device, Pages[3], 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            SetStyle(Convert.ToByte(LED_Style.RadarSwirl));
        }

        public void SetWhirlpoolFix(byte Brightness, byte Speed, byte Direction)
        {
            GetPages(3);
            Pages[3][2] = 33;
            Pages[3][52] = Brightness;
            Pages[3][55] = Direction;
            Pages[3][45] = Speed;
            HID_Basic.WriteDeviceOutput(device, Pages[3], 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            SetStyle(Convert.ToByte(LED_Style.WhirlpoolFix));
        }

        public void SetWhirlpool(byte Brightness, byte Speed, byte Direction)
        {
            GetPages(4);
            Pages[4][2] = 33;
            Pages[4][20] = Brightness;
            Pages[4][23] = Direction;
            Pages[4][13] = Speed;
            HID_Basic.WriteDeviceOutput(device, Pages[4], 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            SetStyle(Convert.ToByte(LED_Style.Whirlpool));
        }

        public void SetHorizon(byte R, byte G, byte B, byte Brightness, byte Speed, byte Direction, byte FadeOutSpeed, bool RGBColor)
        {
            SetRGBColor(Convert.ToByte(LED_Style.Horizon), RGBColor);
            GetPages(4);
            Pages[4][2] = 33;
            Pages[4][45] = R;
            Pages[4][46] = G;
            Pages[4][47] = B;
            Pages[4][48] = Brightness;
            Pages[4][50] = Direction;
            Pages[4][51] = FadeOutSpeed;
            Pages[4][41] = Speed;
            HID_Basic.WriteDeviceOutput(device, Pages[4], 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            SetStyle(Convert.ToByte(LED_Style.Horizon));
        }

        public void SetRipple(byte R, byte G, byte B, byte Brightness, byte Speed, byte Direction, byte FadeOutSpeed1, byte FadeOutSpeed2, bool RGBColor)
        {
            SetRGBColor(Convert.ToByte(LED_Style.Ripple), RGBColor);
            GetPages(5);
            Pages[5][2] = 33;
            Pages[5][9] = R;
            Pages[5][10] = G;
            Pages[5][11] = B;
            Pages[5][12] = Brightness;
            Pages[5][14] = Direction;
            Pages[5][19] = FadeOutSpeed1;
            Pages[5][20] = FadeOutSpeed2;
            Pages[5][5] = Speed;
            HID_Basic.WriteDeviceOutput(device, Pages[5], 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            SetStyle(Convert.ToByte(LED_Style.Ripple));
        }

        public void SetReactive(byte R, byte G, byte B, byte Brightness, byte Speed, byte Direction, bool RGBColor)
        {
            SetRGBColor(Convert.ToByte(LED_Style.Reactive), RGBColor);
            GetPages(5);
            Pages[5][2] = 33;
            Pages[5][33] = R;
            Pages[5][34] = G;
            Pages[5][35] = B;
            Pages[5][36] = Brightness;
            Pages[5][42] = Direction;
            Pages[5][29] = Speed;
            HID_Basic.WriteDeviceOutput(device, Pages[5], 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            SetStyle(Convert.ToByte(LED_Style.Reactive));
        }

        
        public override void SetCustomizeRGBColor(int keyCode, byte R, byte G, byte B, byte Brightness = 255)
        {
            //num5, 1 == keyCode となるばしょのRGBを変更するようにすればうまく動くはず　というかBrightnessいらなくね？ 255固定で
            GK50LP_TKL_JP[keyCode, 1] = R;
            GK50LP_TKL_JP[keyCode, 2] = G;
            GK50LP_TKL_JP[keyCode, 3] = B;
        }

        public override void SetCustomizeRGBColor(int keyCode, Color color, byte Brightness = 255)
        {
            //num5, 1 == keyCode となるばしょのRGBを変更するようにすればうまく動くはず　というかBrightnessいらなくね？ 255固定で
            GK50LP_TKL_JP[keyCode, 1] = color.R;
            GK50LP_TKL_JP[keyCode, 2] = color.G;
            GK50LP_TKL_JP[keyCode, 3] = color.B;
        }

        public void ResetCustomize()
        {
            if (Language == EnumLanguage.US || Language == EnumLanguage.KR)
            {
                for (int i = 0; i < 87; i += 1)
                {
                    GK50LP_TKL_US[i, 1] = 0;
                    GK50LP_TKL_US[i, 2] = 0;
                    GK50LP_TKL_US[i, 3] = 0;
                }
            }
            else if (Language == EnumLanguage.EU)
            {
                for (int i = 0; i < 88; i += 1)
                {
                    GK50LP_TKL_EU[i, 1] = 0;
                    GK50LP_TKL_EU[i, 2] = 0;
                    GK50LP_TKL_EU[i, 3] = 0;
                }
            }
            else if (Language == EnumLanguage.JP)
            {
                for (int i = 0; i < 92; i += 1)
                {
                    GK50LP_TKL_JP[i, 1] = 0;
                    GK50LP_TKL_JP[i, 2] = 0;
                    GK50LP_TKL_JP[i, 3] = 0;
                }
            }
        }

        /// <summary>
        /// カスタマイズ設定を取得します。
        /// </summary>
        public void GetCustomize()
        {
            CurrentBrightness = Pages[5][60];
            for (int pageNumber = 6; pageNumber <= 12; pageNumber++)
            {
                GetPages((byte)pageNumber);
                for (int i = 0; i < 60; i++)
                {
                    Page_Customize[(pageNumber - 6) * 60 + i] = Pages[pageNumber][i + 5];
                }
            }

            if (Language == EnumLanguage.US || Language == EnumLanguage.KR)
            {
                for (int i = 0; i < 87; i += 1)
                {
                    int index = GK50LP_TKL_US[i, 0];
                    GK50LP_TKL_US[i, 1] = Page_Customize[index];
                    GK50LP_TKL_US[i, 2] = Page_Customize[(index + 1)];
                    GK50LP_TKL_US[i, 3] = Page_Customize[(index + 2)];
                }
            }
            else if (Language == EnumLanguage.EU)
            {
                for (int i = 0; i < 88; i += 1)
                {
                    int index = GK50LP_TKL_EU[i, 0];
                    GK50LP_TKL_EU[i, 1] = Page_Customize[index];
                    GK50LP_TKL_EU[i, 2] = Page_Customize[(index + 1)];
                    GK50LP_TKL_EU[i, 3] = Page_Customize[(index + 2)];
                }
            }
            else if (Language == EnumLanguage.JP)
            {
                for (int i = 0; i < 92; i += 1)
                {
                    int index = GK50LP_TKL_JP[i, 0];
                    GK50LP_TKL_JP[i, 1] = Page_Customize[index];
                    GK50LP_TKL_JP[i, 2] = Page_Customize[(index + 1)];
                    GK50LP_TKL_JP[i, 3] = Page_Customize[(index + 2)];
                }
            }
        }


        /// <summary>
        /// カスタマイズ設定を設定します。
        /// </summary>
        /// <param name="brightness">明るさ</param>
        public override void SetCustomize(byte brightness)
        {
            Pages[5][2] = 33;
            Pages[5][60] = brightness;
            int[,] currentLanguageData;
            int languageDataLength;

            if (Language == EnumLanguage.US || Language == EnumLanguage.KR)
            {
                currentLanguageData = GK50LP_TKL_US;
                languageDataLength = 87;
            }
            else if (Language == EnumLanguage.EU)
            {
                currentLanguageData = GK50LP_TKL_EU;
                languageDataLength = 88;
            }
            else // Language == EnumLanguage.JP
            {
                currentLanguageData = GK50LP_TKL_JP;
                languageDataLength = 92;
            }

            for (int i = 0; i < languageDataLength; i += 1)
            {
                int index = currentLanguageData[i, 0];
                Page_Customize[index] = Convert.ToByte(currentLanguageData[i, 1]);
                Page_Customize[(index + 1)] = Convert.ToByte(currentLanguageData[i, 2]);
                Page_Customize[(index + 2)] = Convert.ToByte(currentLanguageData[i, 3]);
            }

            for (int pageNumber = 6; pageNumber <= 12; pageNumber++)
            {
                Pages[pageNumber][2] = 33;
                for (int i = 0; i < 60; i++)
                {
                    Pages[pageNumber][i + 5] = Page_Customize[(pageNumber - 6) * 60 + i];
                }
                HID_Basic.WriteDeviceOutput(device, Pages[pageNumber], 65UL);
            }

            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            SetStyle(Convert.ToByte(LED_Style.Customize));
        }

        public void SetOff()
        {
            SetStyle(Convert.ToByte(LED_Style.Off));
        }

        public void Reset()
        {
            for (int i = 0; i < 3; i++)
            {
                for (byte b = 0; b < 30; b += 1)
                {
                    Array.Clear(SetFeatureData, 0, 65);
                    SetFeatureData[1] = 81;
                    SetFeatureData[2] = 24;
                    SetFeatureData[3] = b;
                    SetFeatureData[5] = byte.MaxValue;
                    HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
                    HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
                }
                Save();
            }
        }

        public void Reset_Command()
        {
            Array.Clear(SetFeatureData, 0, 65);
            SetFeatureData[1] = 65;
            SetFeatureData[2] = 3;
            HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            Thread.Sleep(10);
            Array.Clear(SetFeatureData, 0, 65);
            SetFeatureData[1] = 80;
            SetFeatureData[2] = 16;
            SetFeatureData[5] = 1;
            HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            Thread.Sleep(10);
            Array.Clear(SetFeatureData, 0, 65);
            SetFeatureData[1] = 65;
            SetFeatureData[2] = 0;
            HID_Basic.WriteDeviceOutput(device, SetFeatureData, 65UL);
            HID_Basic.ReadDeviceInput(device, GetFeatureData, 65UL);
            Thread.Sleep(10);
        }

        public int GetKeyIndexByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return -1;
            }
            if (!keyIndexDict_TKL_JP.ContainsKey(key))
            {
                return -1;
            }
            return keyIndexDict_TKL_JP[key];
        }
  
        public nint device { get; private set; } = nint.Zero;

        public static string Version { get; private set; } = "";

        public static string OldVersion { get; private set; } = "";

        private const int VID_GK50 = 3504;

        private const int PID_GK50 = 2908;

        private byte[] SetFeatureData { get; } = new byte[65];

        private byte[] GetFeatureData { get; } = new byte[65];

        public byte CurrentStyle { get; private set; }

        public byte CurrentR { get; private set; } = byte.MaxValue;

        public byte CurrentG { get; private set; }

        public byte CurrentB { get; private set; }

        public byte CurrentDirection { get; private set; }

        public byte CurrentSpeed { get; private set; }

        public byte CurrentFadeOutSpeed { get; private set; }

        public byte CurrentBrightness { get; private set; }

        public byte CurrentRGBColor { get; private set; }

        public static byte[][] Pages { get; } = new byte[13][];

        public static byte[] Page_Customize { get; } = new byte[420];

        public EnumLanguage Language { get; private set; } = EnumLanguage.Error;

        public static int[,] GK50LP_TKL_US = new int[,]
        {
			{ 0, 0, 0, 0},
            { 36, 0, 0, 0},
            { 54, 0, 0, 0},
            { 72, 0, 0, 0},
            { 90, 0, 0, 0},
            { 126, 0, 0, 0},
            { 144, 0, 0, 0},
            { 162, 0, 0, 0},
            { 180, 0, 0, 0},
            { 198, 0, 0, 0},
            { 216, 0, 0, 0},
            { 234, 0, 0, 0},
            { 252, 0, 0, 0},
            { 270, 0, 0, 0},
            { 288, 0, 0, 0},
            { 306, 0, 0, 0},
            { 273, 0, 0, 0},
            { 291, 0, 0, 0},
            { 309, 0, 0, 0},
            { 276, 0, 0, 0},
            { 294, 0, 0, 0},
            { 312, 0, 0, 0},
            { 300, 0, 0, 0},
            { 285, 0, 0, 0},
            { 303, 0, 0, 0},
            { 321, 0, 0, 0},
            { 3, 0, 0, 0},
            { 21, 0, 0, 0},
            { 39, 0, 0, 0},
            { 57, 0, 0, 0},
            { 75, 0, 0, 0},
            { 93, 0, 0, 0},
            { 111, 0, 0, 0},
            { 129, 0, 0, 0},
            { 147, 0, 0, 0},
            { 165, 0, 0, 0},
            { 183, 0, 0, 0},
            { 201, 0, 0, 0},
            { 219, 0, 0, 0},
            { 255, 0, 0, 0},
            { 6, 0, 0, 0},
            { 24, 0, 0, 0},
            { 42, 0, 0, 0},
            { 60, 0, 0, 0},
            { 78, 0, 0, 0},
            { 96, 0, 0, 0},
            { 114, 0, 0, 0},
            { 132, 0, 0, 0},
            { 150, 0, 0, 0},
            { 168, 0, 0, 0},
            { 186, 0, 0, 0},
            { 204, 0, 0, 0},
            { 222, 0, 0, 0},
            { 258, 0, 0, 0},
            { 9, 0, 0, 0},
            { 27, 0, 0, 0},
            { 45, 0, 0, 0},
            { 63, 0, 0, 0},
            { 81, 0, 0, 0},
            { 99, 0, 0, 0},
            { 117, 0, 0, 0},
            { 135, 0, 0, 0},
            { 153, 0, 0, 0},
            { 171, 0, 0, 0},
            { 189, 0, 0, 0},
            { 207, 0, 0, 0},
            { 261, 0, 0, 0},
            { 12, 0, 0, 0},
            { 48, 0, 0, 0},
            { 66, 0, 0, 0},
            { 84, 0, 0, 0},
            { 102, 0, 0, 0},
            { 120, 0, 0, 0},
            { 138, 0, 0, 0},
            { 156, 0, 0, 0},
            { 174, 0, 0, 0},
            { 192, 0, 0, 0},
            { 210, 0, 0, 0},
            { 246, 0, 0, 0},
            { 15, 0, 0, 0},
            { 33, 0, 0, 0},
            { 51, 0, 0, 0},
            { 123, 0, 0, 0},
            { 195, 0, 0, 0},
            { 231, 0, 0, 0},
            { 267, 0, 0, 0},
            { 249, 0, 0, 0}
        };

        public static int[,] GK50LP_TKL_EU = new int[,]
        {
            { 0, 0, 0, 0},
            { 36, 0, 0, 0},
            { 54, 0, 0, 0},
            { 72, 0, 0, 0},
            { 90, 0, 0, 0},
            { 126, 0, 0, 0},
            { 144, 0, 0, 0},
            { 162, 0, 0, 0},
            { 180, 0, 0, 0},
            { 198, 0, 0, 0},
            { 216, 0, 0, 0},
            { 234, 0, 0, 0},
            { 252, 0, 0, 0},
            { 270, 0, 0, 0},
            { 288, 0, 0, 0},
            { 306, 0, 0, 0},
            { 273, 0, 0, 0},
            { 291, 0, 0, 0},
            { 309, 0, 0, 0},
            { 276, 0, 0, 0},
            { 294, 0, 0, 0},
            { 312, 0, 0, 0},
            { 300, 0, 0, 0},
            { 285, 0, 0, 0},
            { 303, 0, 0, 0},
            { 321, 0, 0, 0},
            { 3, 0, 0, 0},
            { 21, 0, 0, 0},
            { 39, 0, 0, 0},
            { 57, 0, 0, 0},
            { 75, 0, 0, 0},
            { 93, 0, 0, 0},
            { 111, 0, 0, 0},
            { 129, 0, 0, 0},
            { 147, 0, 0, 0},
            { 165, 0, 0, 0},
            { 183, 0, 0, 0},
            { 201, 0, 0, 0},
            { 219, 0, 0, 0},
            { 255, 0, 0, 0},
            { 6, 0, 0, 0},
            { 24, 0, 0, 0},
            { 42, 0, 0, 0},
            { 60, 0, 0, 0},
            { 78, 0, 0, 0},
            { 96, 0, 0, 0},
            { 114, 0, 0, 0},
            { 132, 0, 0, 0},
            { 150, 0, 0, 0},
            { 168, 0, 0, 0},
            { 186, 0, 0, 0},
            { 204, 0, 0, 0},
            { 222, 0, 0, 0},
            { 9, 0, 0, 0},
            { 27, 0, 0, 0},
            { 45, 0, 0, 0},
            { 63, 0, 0, 0},
            { 81, 0, 0, 0},
            { 99, 0, 0, 0},
            { 117, 0, 0, 0},
            { 135, 0, 0, 0},
            { 153, 0, 0, 0},
            { 171, 0, 0, 0},
            { 189, 0, 0, 0},
            { 207, 0, 0, 0},
            { 261, 0, 0, 0},
            { 12, 0, 0, 0},
            { 48, 0, 0, 0},
            { 66, 0, 0, 0},
            { 84, 0, 0, 0},
            { 102, 0, 0, 0},
            { 120, 0, 0, 0},
            { 138, 0, 0, 0},
            { 156, 0, 0, 0},
            { 174, 0, 0, 0},
            { 192, 0, 0, 0},
            { 210, 0, 0, 0},
            { 246, 0, 0, 0},
            { 15, 0, 0, 0},
            { 33, 0, 0, 0},
            { 51, 0, 0, 0},
            { 123, 0, 0, 0},
            { 195, 0, 0, 0},
            { 231, 0, 0, 0},
            { 267, 0, 0, 0},
            { 249, 0, 0, 0},
            { 30, 0, 0, 0},
            { 243, 0, 0, 0}
        };

        public static int[,] GK50LP_TKL_JP = new int[,]
        {   //未使用, keyCode, R, G, B, 未使用
            { 0, 0, 0, 0},
            { 36, 0, 0, 0},
            { 54, 0, 0, 0},
            { 72, 0, 0, 0},
            { 90, 0, 0, 0},
            { 126, 0, 0, 0},
            { 144, 0, 0, 0},
            { 162, 0, 0, 0},
            { 180, 0, 0, 0},
            { 198, 0, 0, 0},
            { 216, 0, 0, 0},
            { 234, 0, 0, 0},
            { 252, 0, 0, 0},
            { 270, 0, 0, 0},
            { 288, 0, 0, 0},
            { 306, 0, 0, 0},
            { 273, 0, 0, 0},
            { 291, 0, 0, 0},
            { 309, 0, 0, 0},
            { 276, 0, 0, 0},
            { 294, 0, 0, 0},
            { 312, 0, 0, 0},
            { 300, 0, 0, 0},
            { 285, 0, 0, 0},
            { 303, 0, 0, 0},
            { 321, 0, 0, 0},
            { 3, 0, 0, 0},
            { 21, 0, 0, 0},
            { 39, 0, 0, 0},
            { 57, 0, 0, 0},
            { 75, 0, 0, 0},
            { 93, 0, 0, 0},
            { 111, 0, 0, 0},
            { 129, 0, 0, 0},
            { 147, 0, 0, 0},
            { 165, 0, 0, 0},
            { 183, 0, 0, 0},
            { 201, 0, 0, 0},
            { 219, 0, 0, 0},
            { 255, 0, 0, 0},
            { 6, 0, 0, 0},
            { 24, 0, 0, 0},
            { 42, 0, 0, 0},
            { 60, 0, 0, 0},
            { 78, 0, 0, 0},
            { 96, 0, 0, 0},
            { 114, 0, 0, 0},
            { 132, 0, 0, 0},
            { 150, 0, 0, 0},
            { 168, 0, 0, 0},
            { 186, 0, 0, 0},
            { 204, 0, 0, 0},
            { 222, 0, 0, 0},
            { 9, 0, 0, 0},
            { 27, 0, 0, 0},
            { 45, 0, 0, 0},
            { 63, 0, 0, 0},
            { 81, 0, 0, 0},
            { 99, 0, 0, 0},
            { 117, 0, 0, 0},
            { 135, 0, 0, 0},
            { 153, 0, 0, 0},
            { 171, 0, 0, 0},
            { 189, 0, 0, 0},
            { 207, 0, 0, 0},
            { 261, 0, 0, 0},
            { 12, 0, 0, 0},
            { 48, 0, 0, 0},
            { 66, 0, 0, 0},
            { 84, 0, 0, 0},
            { 102, 0, 0, 0},
            { 120, 0, 0, 0},
            { 138, 0, 0, 0},
            { 156, 0, 0, 0},
            { 174, 0, 0, 0},
            { 192, 0, 0, 0},
            { 210, 0, 0, 0},
            { 246, 0, 0, 0},
            { 15, 0, 0, 0},
            { 33, 0, 0, 0},
            { 51, 0, 0, 0},
            { 123, 0, 0, 0},
            { 195, 0, 0, 0},
            { 231, 0, 0, 0},
            { 267, 0, 0, 0},
            { 249, 0, 0, 0},
            { 237, 0, 0, 0},
            { 243, 0, 0, 0},
            { 228, 0, 0, 0},
            { 87, 0, 0, 0},
            { 159, 0, 0, 0},
            { 177, 0, 0, 0}
        };

        public static Dictionary<string, int> keyIndexDict_TKL_JP = new Dictionary<string, int>(){
            {"ESC", 0 },
            {"F1", 1 },
            {"F2", 2 },
            {"F3", 3 },
            {"F4", 4 },
            {"F5", 5 },
            {"F6", 6 },
            {"F7", 7 },
            {"F8", 8 },
            {"F9", 9 },
            {"F10", 10 },
            {"F11", 11 },
            {"F12", 12 },
            {"PRINT", 13 },
            {"SCROLL", 14 },
            {"PAUSE", 15 },
            {"INSERT", 16 },
            {"HOME", 17 },
            {"PGUP", 18 },
            {"DEL", 19 },
            {"END", 20 },
            {"PGDN", 21 },
            {"UP_ARROW", 22 },
            {"L_ARROW", 23 },
            {"DN_ARROW", 24 },
            {"R_ARROW", 25 },
            {"TILDE", 26 }, //半角全角
            {"1", 27 },
            {"2", 28 },
            {"3", 29 },
            {"4", 30 },
            {"5", 31 },
            {"6", 32 },
            {"7", 33 },
            {"8", 34 },
            {"9", 35 },
            {"0", 36 },
            {"NEG", 37 }, //=
            {"EQUATION", 38 }, //~
            {"BACKSPACE", 39 },
            {"TAB", 40 },
            {"Q", 41 },
            {"W", 42 },
            {"E", 43 },
            {"R", 44 },
            {"T", 45 },
            {"Y", 46 },
            {"U", 47 },
            {"I", 48 },
            {"O", 49 },
            {"P", 50 },
            {"L_BRACKETS", 51 }, //`
            {"R_BRACKETS", 52 }, //{
            {"CAP", 53 }, //CAPS
            {"A", 54 },
            {"S", 55 },
            {"D", 56 },
            {"F", 57 },
            {"G", 58 },
            {"H", 59 },
            {"J", 60 },
            {"K", 61 },
            {"L", 62 },
            {"SEMICOLON", 63 }, //+
            {"APOSTROPHE", 64 }, //*
            {"ENTER", 65 },
            {"L_SHIFT", 66 },
            {"Z", 67 },
            {"X", 68 },
            {"C", 69 },
            {"V", 70 },
            {"B", 71 },
            {"N", 72 },
            {"M", 73 },
            {"COMMA", 74 }, //<
            {"DOT", 75 }, //>
            {"SLASH", 76 }, //?
            {"R_SHIFT", 77 },
            {"L_CTRL", 78 },
            {"L_WIN", 79 },
            {"L_ALT", 80 },
            {"SPACE", 81 },
            {"R_ALT", 82 },
            {"R_WIN", 83 },
            {"R_CTRL", 84 },
            {"FN0", 85 }, //DRAGON
            {"CODE14", 86 }, //|
            {"CODE42", 87 }, //}
            {"CODE56", 88 }, //_
            {"CODE131", 89 }, //無変換
            {"CODE132", 90 }, //変換
            {"CODE133", 91 } //カタカナひらがな
        };

        public enum LED_Style : byte
        {
            CustomOverLap,
            Steady,
            Breath,
            Wave,
            Radar,
            RadarSwirl,
            WhirlpoolFix,
            Whirlpool,
            Horizon,
            Ripple,
            Reactive,
            Customize,
            Off
        }

        public enum EnumLanguage
        {
            Error,
            US,
            EU,
            JP,
            KR
        }
    }
}
