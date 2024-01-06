using System.Runtime.InteropServices;
using System.Text;

namespace Mystic_Tools.ActiveWindow
{
    /// <summary>
    /// アクティブなウィンドウのパスを取得するクラスです。
    /// </summary>
    public static class ActiveWindowPath
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("psapi.dll")]
        private static extern uint GetProcessImageFileName(IntPtr hProcess, StringBuilder lpImageFileName, uint nSize);

        /// <summary>
        /// アクティブなウィンドウのパスを取得します。
        /// </summary>
        /// <returns>アクティブなウィンドウのパス</returns>
        public static string GetPath()
        {
            // アクティブなウィンドウのハンドルを取得
            IntPtr hWnd = GetForegroundWindow();

            // プロセスIDを取得
            GetWindowThreadProcessId(hWnd, out uint processId);

            // プロセスハンドルを取得
            IntPtr hProcess = OpenProcess(0x0400 | 0x0010, false, processId);

            // プロセスのイメージファイル名を取得
            StringBuilder path = new StringBuilder(1024);
            GetProcessImageFileName(hProcess, path, (uint)path.Capacity);

            // デバイスパスをDOSパスに変換
            string? dosPath = DevicePathMapper.FromDevicePath(path.ToString()) ?? "";

            return dosPath;
        }
    }

}
