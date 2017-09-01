using System;

namespace MetaLog {
    public static class Helper
    {
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
