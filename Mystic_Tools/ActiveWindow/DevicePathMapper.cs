using System.Runtime.InteropServices;
using System.Text;

namespace Mystic_Tools.ActiveWindow
{
    /// <summary>
    /// デバイスパスをDOSパスに変換するクラスです。
    /// </summary>
    public static class DevicePathMapper
    {
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern uint QueryDosDevice([In] string lpDeviceName, [Out] StringBuilder lpTargetPath, [In] int ucchMax);

        /// <summary>
        /// デバイスパスをDOSパスに変換します。
        /// </summary>
        /// <param name="devicePath">デバイスパス</param>
        /// <returns>DOSパス</returns>
        public static string? FromDevicePath(string devicePath)
        {
            // ドライブ情報を取得
            var drive = Array.Find(DriveInfo.GetDrives(), d => devicePath.StartsWith(d.GetDevicePath(), StringComparison.InvariantCultureIgnoreCase));

            // デバイスパスをDOSパスに変換
            return drive != null ? devicePath.ReplaceFirst(drive.GetDevicePath(), drive.GetDriveLetter()) : "";
        }

        /// <summary>
        /// ドライブのデバイスパスを取得します。
        /// </summary>
        /// <param name="driveInfo">ドライブ情報</param>
        /// <returns>デバイスパス</returns>
        private static string? GetDevicePath(this DriveInfo driveInfo)
        {
            var devicePathBuilder = new StringBuilder(128);
            return QueryDosDevice(driveInfo.GetDriveLetter(), devicePathBuilder, devicePathBuilder.Capacity + 1) != 0 ? devicePathBuilder.ToString() : "";
        }

        /// <summary>
        /// ドライブの文字を取得します。
        /// </summary>
        /// <param name="driveInfo">ドライブ情報</param>
        /// <returns>ドライブの文字</returns>
        private static string GetDriveLetter(this DriveInfo driveInfo)
        {
            return driveInfo.Name.Substring(0, 2);
        }

        /// <summary>
        /// 文字列の最初の一部を置換します。
        /// </summary>
        /// <param name="text">元の文字列</param>
        /// <param name="search">検索文字列</param>
        /// <param name="replace">置換文字列</param>
        /// <returns>置換後の文字列</returns>
        private static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            return pos < 0 ? text : text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
