using MsiHid;
using Mystic_Tools.ActiveWindow;
using Mystic_Tools.Utils;
using System.Diagnostics;
using System.Drawing;
using NAudio.Wave;

namespace Mystic_Tools
{
    /// <summary>
    /// キーボードの管理を行うクラスです。
    /// </summary>
    internal class KeyboardManager
    {
        /// <summary>
        /// KeyboardManagerクラスの新しいインスタンスを初期化します。
        /// </summary>
        public KeyboardManager()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(ProcessExit);
            watcher = new Watcher();
            watcher.WindowChanged += (sender, e) =>
            {
                WhenActiveWindowChanged();
            };
        }

        /// <summary>
        /// ウィンドウの監視を開始します。
        /// </summary>
        public void StartWindowWatcher()
        {
            watcher.Start();

            foreach (string file in Directory.EnumerateFiles(configDirectory, "*.ini"))
            {
                configs.Add(new Config(file));
                Console.WriteLine("Setting Loaded : {0}", configs[^1].SettingName);
            }
        }

        /// <summary>
        /// ウィンドウの監視を停止します。
        /// </summary>
        public void StopWindowWatcher()
        {
            watcher.Stop();
        }

        private void WhenActiveWindowChanged()
        {
            string activeWindowPath = ActiveWindowPath.GetPath();
            if (activeWindowPath == oldActiveWindowPath)
            {
                return;
            }

            Console.WriteLine(activeWindowPath);

            Config? config = configs.FirstOrDefault(c => c.ExecutablePath == activeWindowPath);
            if (config == null)
            {
                return;
            }

            Console.WriteLine(HID_Basic.IsDeviceConnect(3504, 2908, 0, 0));
            keyboard = new GK50LP_TKL();
            Console.WriteLine(keyboard.Init());

            keyboard.ResetCustomize();
            foreach (var key in config.color)
            {
                Console.WriteLine(key.Key + ": " + key.Value[0] + ", " + key.Value[1] + ", " + key.Value[2]);
                keyboard.SetCustomizeRGBColor(keyboard.GetKeyIndexByKey(key.Key), key.Value[0], key.Value[1], key.Value[2]);

            }
            keyboard.SetCustomize(config.Brightness);
            Console.WriteLine(keyboard.CloseDevice());

            oldActiveWindowPath = ActiveWindowPath.GetPath();
        }

        public void VideoPlay(string video_path)
        {
            var player = new WasapiOut();
            var reader = new MediaFoundationReader(video_path);
            player.Init(reader);

            Stopwatch stopwatch = new();

            VideoFrameAnalyzer videoFrameAnalyzer = new(video_path);

            Console.WriteLine(HID_Basic.IsDeviceConnect(3504, 2908, 0, 0));
            GK50LP_TKL keyboard = new();
            Console.WriteLine(keyboard.Init());

            int currentFrame = 0;
            Bitmap frameBitmap = videoFrameAnalyzer.GetFrameColorAtFrame(currentFrame);

            CoordinateConverter coordinateConverter = new CoordinateConverter(frameBitmap.Width, frameBitmap.Height);
            Process.Start("C:\\Program Files\\VideoLAN\\VLC\\vlc.exe", video_path);
            Thread.Sleep(500);
            //player.Play();
            stopwatch.Start();
            while (true)
            {
                long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                currentFrame = (int)(elapsedMilliseconds / 1000.0 * videoFrameAnalyzer.FrameRate);

                if (currentFrame >= videoFrameAnalyzer.FrameCount)
                {
                    break;
                }
                
                frameBitmap = videoFrameAnalyzer.GetFrameColorAtFrame(currentFrame);

                for (int i = 0; i < 92; i ++)
                {
                    int[] keyCoordinate = coordinateConverter.KeyIndexToCoordinate(i);
                    keyboard.SetCustomizeRGBColor(i, frameBitmap.GetPixel(keyCoordinate[0] + offsetX, keyCoordinate[1] + offsetY));
                }

                keyboard.SetCustomize(255);
                Thread.Sleep(80);

            }

            Console.WriteLine(keyboard.CloseDevice());

        }
        private void ProcessExit(object? sender, EventArgs? e)
        {
            keyboard?.CloseDevice();
            watcher?.Stop();
        }

        public int offsetX = 0;

        public int offsetY = 40;

        private static GK50LP_TKL? keyboard;

        public string configDirectory = "config";

        public List<Config> configs = [];

        private readonly Watcher watcher;

        private string oldActiveWindowPath = string.Empty;

    }
}
