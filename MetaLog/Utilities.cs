using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace MetaLog
{
    public static class Utilities
    {
        private const string TreeStart = "┌";
        private const string TreeItem = "├";
        private const string TreeEnd = "└";
        private const string SubTreeStart = "┬";
        private const string HSpacer = "─";

        /// <summary>
        ///     Get the recommended log file path for this executing assembly (<code>%AppData%/MetaLog/YourProject.log</code>)
        /// </summary>
        public static string RecommendedLogFile => Path.Combine(MetaLogAppData, $"{ExecutingAssemblyName}.log");

        /// <summary>
        ///     The new line character (<see cref="Environment.NewLine" />)
        /// </summary>
        public static string Nl { get; set; } = "\n";

        /// <summary>
        ///     Path to %AppData%
        /// </summary>
        public static string AppData { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        /// <summary>
        ///     Path to %AppData%/MetaLog
        /// </summary>
        public static string MetaLogAppData
        {
            get
            {
                string path = Path.Combine(AppData, "MetaLog");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        /// <summary>
        ///     Get the Assembly this MetaLogger is called by
        /// </summary>
        public static string ExecutingAssemblyName
        {
            get
            {
                var execAssembly = Assembly.GetExecutingAssembly();
                return execAssembly?.GetName()?.Name ?? "MetaLog"; //return caller name or "MetaLog"
            }
        }

        /// <summary>
        ///     Build a log message
        /// </summary>
        /// <returns>The built log message</returns>
        internal static string BuildMessage(LogSeverity severity, string text, string callerFile, string callerMember,
            int callerLine)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            string file = Path.GetFileNameWithoutExtension(callerFile);

            string strSeverity = $"[{severity}]";
            strSeverity =
                BringOnSameLength(strSeverity, 10); // Longest Severity ("Critical": 8) + brackets "[": 1 & "]": 1 = 10
            string strMemberInfo = $"[{file}.{callerMember}:{callerLine}]:";
            strMemberInfo = BringOnSameLength(strMemberInfo, 40); // unknown, just guess 40

            return $"{strSeverity} [{time}] {strMemberInfo} {text}{Nl}";
        }

        /// <summary>
        ///     Bring the given input to the given length by adding whitespaces to the end
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="length">The length for this string to be</param>
        /// <returns></returns>
        private static string BringOnSameLength(string input, int length)
        {
            if (input.Length >= length)
                return input; // return on wrong input

            string spaces = new string(' ', length - input.Length);
            return input + spaces;
        }

        /// <summary>
        ///     Censor a given string.
        /// </summary>
        /// <param name="text">The text to censor</param>
        /// <param name="censorPercent">
        ///     The amount of text to censor
        ///     (from left to right) in percent (from 0 to 1)
        /// </param>
        /// <param name="censorChar">
        ///     A custom character to use
        ///     for censoring
        /// </param>
        public static string Censor(string text, double censorPercent = 0.4, char censorChar = '•')
        {
            int length = text.Length;
            int toCensor = (int) Math.Floor(length * censorPercent);
            string censored = text.Substring(0, toCensor) + new string(censorChar, length - toCensor);
            return censored;
        }

        /// <summary>
        ///     Walk up an <see cref="Exception.InnerException" /> tree
        ///     and return the result in string form.
        /// </summary>
        /// <param name="exception">The original <see cref="Exception" /></param>
        /// <param name="indent">
        ///     The amount of spaces for indentation
        ///     (will increase by 4 each inner-exception)
        /// </param>
        /// <param name="subtree">The private recurse helper subtree bool</param>
        /// <returns>A built tree of <see cref="Exception.InnerException" />s</returns>
        public static string RecurseException(Exception exception, int indent = 0, bool subtree = false)
        {
            string tree = exception.StackTrace.IsValid()
                ? BuildTree(exception.StackTrace, subtree)
                : string.Empty;

            string message = $"{exception.GetType()}: {exception.Message}{Nl}{tree}{Nl}";
            if (exception.InnerException != null)
                message += Indent($"Inner Exception:{Nl}{RecurseException(exception.InnerException, indent + 4, true)}",
                    indent);

            return message;
        }

        /// <summary>
        ///     Indent all lines of the given text by the given amount of whitespaces
        /// </summary>
        /// <param name="text">The input text</param>
        /// <param name="amount">The amount of whitespaces to indent</param>
        public static string Indent(string text, int amount)
        {
            string indent = new string(' ', amount);
            string[] lines = text.Split(new[] {Nl},
                StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = $"{indent}{lines[i]}";
                if (i != lines.Length - 1)
                    lines[i] += Nl; // add \n to every but last line
            }

            return string.Concat(lines);
        }

        /// <summary>
        ///     Resest the indent of all lines of the given text
        /// </summary>
        /// <param name="text">The input text</param>
        public static string ResetIndent(string text)
        {
            string[] lines = text.Split(new[] {Nl},
                StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
                lines[i] = $"{lines[i].TrimStart()}{Nl}";

            return string.Concat(lines);
        }

        /// <summary>
        ///     Build a tree of the given input. Example:
        ///     <para />
        ///     ┌ Tree Start
        ///     <para />
        ///     ├ Tree item 1
        ///     <para />
        ///     ├ Tree item 2
        ///     <para />
        ///     ├ Tree item 3
        ///     <para />
        ///     └ Tree End
        /// </summary>
        /// <param name="input">The given input string</param>
        /// <param name="isSubtree">
        ///     Indicating whether this is a
        ///     subtree
        /// </param>
        /// <param name="isEnd">
        ///     Indicating whether this is the
        ///     last tree
        /// </param>
        /// <param name="baseIndent">
        ///     The amount of whitespaces
        ///     to indent every tree child
        /// </param>
        /// <returns>A built tree</returns>
        public static string BuildTree(string input, bool isSubtree = false, bool isEnd = true, int baseIndent = 0)
        {
            string[] lines = input.Split(new[] {Nl},
                StringSplitOptions.RemoveEmptyEntries);
            string start = isSubtree ? SubTreeStart : TreeStart; // make ┬ or ┌ 

            switch (lines.Length)
            {
                case 0:
                    return string.Empty;
                case 1:
                    return $"{HSpacer} {lines[0]}";
            }

            string trimmed = lines[0].TrimStart(' ', '\t'); // remove first whitespaces
            int whitespacesCount = lines[0].Length - trimmed.Length + baseIndent; // number of whitespaces

            lines[0] = Indent($"{start} {trimmed}", whitespacesCount);

            for (int i = 1; i < lines.Length - 1; i++)
                lines[i] = Indent($"{TreeItem} {lines[i]}", whitespacesCount); // make ├ {line}

            string result;
            if (isEnd)
            {
                string text = $"{TreeEnd} {lines[lines.Length - 1]}";
                lines[lines.Length - 1] = Indent(text, whitespacesCount); // make └
                result = string.Join(Nl, lines);
            } else
            {
                string text = $"{TreeItem} {lines[lines.Length - 1]}";
                lines[lines.Length - 1] = Indent(text, whitespacesCount); // make ├

                result = string.Join(Nl, lines);
                //result += Nl;
                result += Indent($"{TreeEnd}{HSpacer}", whitespacesCount); // make └─
            }

            return result;
        }
    }
}