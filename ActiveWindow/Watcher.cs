using System.Runtime.InteropServices;
using Timer = System.Timers.Timer;

namespace Mystic_Tools.ActiveWindow
{
    /// <summary>
    /// アクティブなウィンドウの変更を監視するクラスです。
    /// </summary>
    public class Watcher
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private IntPtr previousWindowHandle;
        private Timer? timer;

        /// <summary>
        /// ウィンドウが変更されたときに発生するイベントです。
        /// </summary>
        public event EventHandler? WindowChanged;

        /// <summary>
        /// Watcherクラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="interval">監視の間隔（ミリ秒）</param>
        public Watcher(int interval = 200)
        {
            timer = new Timer { Interval = interval };
            timer.Elapsed += Timer_Tick;
        }

        /// <summary>
        /// 監視を開始します。
        /// </summary>
        public void Start()
        {
            timer?.Start();
        }

        /// <summary>
        /// 監視を停止します。
        /// </summary>
        public void Stop()
        {
            timer?.Stop();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            IntPtr currentWindowHandle = GetForegroundWindow();
            if (currentWindowHandle != previousWindowHandle)
            {
                OnWindowChanged(EventArgs.Empty);
                previousWindowHandle = currentWindowHandle;
            }
        }

        /// <summary>
        /// WindowChangedイベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected virtual void OnWindowChanged(EventArgs e)
        {
            WindowChanged?.Invoke(this, e);
        }
    }
}
