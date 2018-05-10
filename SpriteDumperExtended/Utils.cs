using UnityEngine;
using System.IO;

namespace SpriteDumperExtended
{
    public class Utils
    {
        // http://stackoverflow.com/a/847251
        public static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        public static bool CreateDir(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch { return false; }
                return true;
            }
            return false;
        }

        public static bool DeleteDir(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, true);
                }
                catch { return false; }
                return true;
            }
            return false;
        }

        public static void Log(string msg)
        {
            Debug.Log("[SpriteDumper] " + msg);
            //DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SpriteDumper] " + msg);
        }

        public static void LogError(string msg)
        {
            Debug.Log("[SpriteDumper] " + msg);
            //DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, "[SpriteDumper] " + msg);
        }
    }
}
