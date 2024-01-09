using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;


namespace Mystic_Tools
{
    /// <summary>
    /// 設定ファイルを解析し、設定情報を提供するクラスです。
    /// </summary>
    internal class Config
    {
        public Config(string filePath)
        {
            data = parser.ReadFile(filePath);
            if (data["Settings"] == null ||
                data["Settings"]["SettingName"] == null ||
                data["Settings"]["Path"] == null ||
                data["Settings"]["Brightness"] == null)
            {
                throw new Exception("Settings format error");
            }

            SettingName = data["Settings"]["SettingName"];
            ExecutablePath = data["Settings"]["Path"];
            Brightness = Convert.ToByte(data["Settings"]["Brightness"]);

            if (data["Color"] == null)
            {
                throw new Exception("Color not found");
            }

            foreach (var keyData in data["Color"])
            {
                string[] rgbStrings = keyData.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                byte[] rgbBytes = rgbStrings.Select(s => byte.Parse(s.Trim())).ToArray();
                color.Add(keyData.KeyName, rgbBytes);
            }
        }

        private readonly FileIniDataParser parser = new();
        private readonly IniData data;
        public string SettingName { get; }
        public string ExecutablePath { get; }
        public byte Brightness { get; }
        public Dictionary<string, byte[]> color { get; } = new();
        private static readonly char[] separator = { ',' };
    }
}
