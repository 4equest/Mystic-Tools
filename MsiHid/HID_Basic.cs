using System.Runtime.InteropServices;

namespace MsiHid
{
    /// <summary>
    /// HIDデバイスの基本操作を提供するクラスです。
    /// </summary>
    public static class HID_Basic
    {
        [DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private static extern int Init(byte[] s);

        /// <summary>
        /// HIDデバイスを初期化します。 おそらく完全な初期化？
        /// </summary>
        /// <param name="psw">パスワード</param>
        /// <returns>初期化が成功したかどうか</returns>
        public static bool Init(string psw)
        {
            if (psw.Length < 7) return false;

            byte[] array = psw.Take(7).Select(c => Convert.ToByte(c)).ToArray();
            return Init(array) == 0;
        }


        [DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern IntPtr openMyDevice(ushort VID, ushort PID, ushort MI, ushort COL, ushort deviceNum = 1);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern IntPtr openMyDevice_Read(ushort VID, ushort PID, ushort MI, ushort COL, ushort deviceNum = 1);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern IntPtr openMyDevice_Overlapped(ushort VID, ushort PID, ushort MI, ushort COL, ushort deviceNum = 1);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int CloseDevice(IntPtr DevHandle);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int GetAttributes(IntPtr DevHandle, ref ushort VID, ref ushort PID, ref ushort REV);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int SetFeature(IntPtr DevHandle, byte[] Data, ulong length);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int GetFeature(IntPtr DevHandle, byte[] Data, ulong length);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int GetInputReport(IntPtr DevHandle, byte[] Data, ulong length);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int SetOutputReport(IntPtr DevHandle, byte[] Data, ulong length);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int GetManufacturerString(IntPtr DevHandle, byte[] Data, ulong length);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int GetProductString(IntPtr DevHandle, byte[] Data, ulong length);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int GetSerialNumberString(IntPtr DevHandle, byte[] Data, ulong length);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int ReadDeviceInput(IntPtr DevHandle, byte[] Data, ulong length);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int WriteDeviceOutput(IntPtr DevHandle, byte[] Data, ulong length);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int ReadDeviceInput_Overlapped(IntPtr DevHandle, byte[] Data, ulong length);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int WriteDeviceOutput_Overlapped(IntPtr DevHandle, byte[] Data, ulong length);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int IsDeviceConnect(ushort VID, ushort PID, ushort MI, ushort COL);

		[DllImport("MsiHid_x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int IsDeviceConnect_Number(ushort VID, ushort PID, ushort MI, ushort COL);

		internal const string DLL_FileName = "MsiHid_x64.dll";
	}
}
