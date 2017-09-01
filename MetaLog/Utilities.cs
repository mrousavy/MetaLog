using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace MetaLog {
    public static class Utilities {
        private const string TreeStart = "┌";
        private const string TreeItem = "├";
        private const string TreeEnd = "└";
        private const string SubTreeStart = "┬";
        private const string HSpacer = "─";

        /// <summary>
        /// The new line character. Default: \n, May be <see cref="Environment.NewLine"/>
        /// </summary>
        public static string Nl { get; set; } = "\n"; //Environment.NewLine ?

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
                return execAssembly?.GetName()?.Name ?? "MetaLog"; //return caller name or "MetaLog"
            }
        }

        /// <summary>
        /// Build a log message
        /// </summary>
        /// <returns>The built log message</returns>
        internal static string BuildMessage(LogSeverity severity, string text, string callerFile, string callerMember, int callerLine) {
            string time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            string file = Path.GetFileNameWithoutExtension(callerFile);

            return $"[{severity}] [{time}] [{file}.{callerMember}:{callerLine}]: {text}{Nl}";
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

        /// <summary>
        /// Walk up an <see cref="Exception.InnerException"/> tree
        /// and return the result in string form.
        /// </summary>
        /// <param name="exception">The original <see cref="Exception"/></param>
        /// <param name="indent">The amount of spaces for indentation
        /// (will increase by 4 each inner-exception)</param>
        /// <returns>A built tree of <see cref="Exception.InnerException"/>s</returns>
        public static string RecurseException(Exception exception, int indent = 0) {
            string nl = Environment.NewLine;
            string message = string.Empty;
            if (indent == 0) message += $"Begin Exception Tree:{nl}";

            message += $"{exception.GetType()}: {exception.Message}{nl}{exception.StackTrace}"; //construct message

            if (exception.InnerException != null) {
                message += RecurseException(exception.InnerException, indent + 4);
            } else {
                message = BuildTree(message, indent > 0);
            }

            return message;
        }

        /// <summary>
        /// Indent all lines of the given text by the given amount of whitespaces
        /// </summary>
        /// <param name="text">The input text</param>
        /// <param name="amount">The amount of whitespaces to indent</param>
        public static string Indent(string text, int amount) {
            string indent = new string(' ', amount);
            string[] lines = text.Split(new[] { Nl },
                StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++) {
                lines[i] = $"{indent}{lines[i]}{Nl}";
            }

            string result = string.Concat(lines);
            return result;
        }

        /// <summary>
        /// Resest the indent of all lines of the given text
        /// </summary>
        /// <param name="text">The input text</param>
        public static string ResetIndent(string text) {
            string[] lines = text.Split(new[] { Nl },
                StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++) {
                lines[i] = $"{lines[i].TrimStart()}{Nl}";
            }

            string result = string.Concat(lines);
            return result;
        }

        /// <summary>
        /// Build a tree of the given input. Example:
        /// <para/>
        /// ┌ Tree Start <para/>
        /// ├ Tree item 1 <para/>
        /// ├ Tree item 2 <para/>
        /// ├ Tree item 3 <para/>
        /// └ Tree End
        /// </summary>
        /// <param name="input">The given input string</param>
        /// <param name="isSubtree">Indicating whether this is a
        /// subtree</param>
        /// <param name="isEnd">Indicating whether this is the
        /// last tree</param>
        /// <returns>A built tree</returns>
        public static string BuildTree(string input, bool isSubtree = false, bool isEnd = true) {
            string[] lines = input.Split(new[] {Environment.NewLine},
                StringSplitOptions.RemoveEmptyEntries);
            string start = isSubtree ? SubTreeStart : TreeStart; //make ┬ or ┌ 

            switch (lines.Length) {
                case 0:
                    return string.Empty;
                case 1:
                    return $"{SubTreeStart} {lines[0]}";
            }
            
            string nl = Environment.NewLine;

            string trimmed = lines[0].TrimStart(); //remove first whitespaces
            string indent = new string(' ', lines[0].Length - trimmed.Length); //get original whitespace indent

            lines[0] = $"{indent}{start} {trimmed}";

            for (int i = 1; i < lines.Length - 1; i++) {
                lines[i] = $"{indent}{TreeItem} {lines[i]}"; //make ├ {line}
            }

            string result;
            if (isEnd) {
                lines[lines.Length - 1] += $"{indent}{TreeEnd}{lines[lines.Length - 1]}";  //make └
                result = string.Join(nl, lines);
            } else {
                lines[lines.Length - 1] += $"{indent}{TreeItem}{lines[lines.Length - 1]}";  //make ├
                result = string.Join(nl, lines);
                result += $"{nl}{indent}{TreeEnd}{HSpacer}"; //make └─
            }
            return result;
        }
    }
}
