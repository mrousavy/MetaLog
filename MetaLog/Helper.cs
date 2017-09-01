using System;
using System.IO;
using System.Reflection;

namespace MetaLog {
    public static class Helper {
        /// <summary>
        /// Path to %appdata%
        /// </summary>
        public static string AppData { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        /// <summary>
        /// Path to %appdata%/MetaLog
        /// </summary>
        public static string MetaLogAppData {
            get {
                string path = Path.Combine(AppData, "MetaLog");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }

        /// <summary>
        /// Get the Assembly this MetaLogger is called by
        /// </summary>
        public static string ExecutingAssemblyName {
            get {
                var execAssembly = Assembly.GetExecutingAssembly();
                return execAssembly.FullName;
            }
        }

        /// <summary>
        /// Censor a given string.
        /// </summary>
        /// <param name="text">The text to censor</param>
        /// <param name="censorPercent">The amount of text to censor 
        /// (from left to right) in percent (from 0 to 1)</param>
        /// <param name="censorChar">A custom character to use
        /// for censoring</param>
        public static string Censor(string text, double censorPercent = 0.4, char censorChar = '•') {
            int length = text.Length;
            int toCensor = (int)Math.Floor(length * censorPercent);
            string censored = text.Substring(0, toCensor) + new string(censorChar, length - toCensor);
            return censored;
        }
    }
}
