using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable ExplicitCallerInfoArgument

namespace MetaLog
{
    public class MetaLogger : ILogger
    {

        #region ctor

        /// <summary>
        ///     Create a new MetaLogger with the given filename
        /// </summary>
        /// <param name="logFile">The file to log to</param>
        public MetaLogger(string logFile) 
            : this(logFile, LogSeverity.Info)
        {
            _closeStream = true;
        }

        /// <summary>
        ///     Create a new MetaLogger with the given filename
        /// </summary>
        /// <param name="logFile">The file to log to</param>
        /// <param name="minSeverity">The minimum severity to log messages to</param>
        public MetaLogger(string logFile, LogSeverity minSeverity)
            : this(new FileStream(logFile, FileMode.OpenOrCreate), minSeverity, Encoding.Unicode)
        {
            _closeStream = true;
        }

        /// <summary>
        ///     Create a new MetaLogger with the given filename
        /// </summary>
        /// <param name="logFile">The file to log to</param>
        /// <param name="minSeverity">The minimum severity to log messages to</param>
        /// <param name="encoding">The encoding to use for writing strings</param>
        public MetaLogger(string logFile, LogSeverity minSeverity, Encoding encoding) 
            : this(new FileStream(logFile, FileMode.OpenOrCreate), minSeverity, encoding)
        {
            _closeStream = true;
        }

        /// <summary>
        ///     Create a new MetaLogger with the given stream to write to
        /// </summary>
        /// <param name="stream">The stream to log to</param>
        public MetaLogger(Stream stream)
            : this(stream, LogSeverity.Info)
        { }

        /// <summary>
        ///     Create a new MetaLogger with the given stream to write to
        /// </summary>
        /// <param name="stream">The stream to log to</param>
        /// <param name="minSeverity">The minimum severity to log messages to</param>
        public MetaLogger(Stream stream, LogSeverity minSeverity)
            : this(stream, minSeverity, Encoding.Unicode)
        { }

        /// <summary>
        ///     Create a new MetaLogger with the given stream to write to
        /// </summary>
        /// <param name="stream">The stream to log to</param>
        /// <param name="minSeverity">The minimum severity to log messages to</param>
        /// <param name="encoding">The encoding to use for writing strings</param>
        public MetaLogger(Stream stream, LogSeverity minSeverity, Encoding encoding)
        {
            if(stream == null || !stream.CanWrite)
                throw new ArgumentException(nameof(stream));

            Stream = stream;
            MinimumSeverity = minSeverity;
            Encoding = encoding ?? throw new ArgumentException(nameof(encoding));
        }

        #endregion

        #region Privates

        private readonly bool _closeStream;
        private object Lock { get; } = new object();

        #endregion

        #region Properties

        public Stream Stream { get; set; }

        public Encoding Encoding { get; set; }

        public LogSeverity MinimumSeverity { get; set; }

        #endregion

        #region Functions

        public void Dispose()
        {
            if (!_closeStream) return;

            lock (Lock)
            {
                // lock so we don't interrupt a FileStream's write op
                Stream?.Dispose();
            }
        }

        #endregion

        #region Logging

        //////////////////////////////////
        //            LOGGER            //
        //////////////////////////////////

        public void Log(LogSeverity severity, string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0)
        {
            if (severity < MinimumSeverity)
                return; //don't log if it's below min severity

            string text =
                Utilities.BuildMessage(severity, message, callerFile, callerMember, callerLine); //construct the message
            WriteText(text);
        }

        public void Log(LogSeverity severity, Exception exception, int indent = 2,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0)
        {
            if (severity < MinimumSeverity)
                return; //don't log if it's below min severity

            string message = Utilities.RecurseException(exception, 2); //build exception tree
            message = Utilities.BuildTree(message);
            message = Utilities.Indent(message, indent);
            message = $"BEGIN EXCEPTION TREE:{Utilities.Nl}{message}";
            string text =
                Utilities.BuildMessage(severity, message, callerFile, callerMember, callerLine); //construct the message
            WriteText(text);
        }

        public Task LogAsync(LogSeverity severity, string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0)
        {
            if (severity < MinimumSeverity)
                return Task.CompletedTask; //don't log if it's below min severity

            return Task.Run(() =>
            {
                string text =
                    Utilities.BuildMessage(severity, message, callerFile, callerMember,
                        callerLine); //construct the message
                WriteText(text);
            });
        }

        public Task LogAsync(LogSeverity severity, Exception exception, int indent = 2,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0)
        {
            if (severity < MinimumSeverity)
                return Task.CompletedTask; //don't log if it's below min severity

            return Task.Run(() =>
            {
                string message = Utilities.RecurseException(exception, 2); //build exception tree
                message = Utilities.BuildTree(message);
                message = Utilities.Indent(message, indent);
                message = $"BEGIN EXCEPTION TREE:{Utilities.Nl}{message}";
                string text =
                    Utilities.BuildMessage(severity, message, callerFile, callerMember,
                        callerLine); //construct the message
                WriteText(text);
            });
        }

        private void WriteText(string text)
        {
            lock (Lock)
            {
                // lock to sync object to prevent inconsistency
                byte[] bytes = Encoding.GetBytes(text);
                Stream.Write(bytes, 0, bytes.Length);
            }
        }

        //////////////////////////////////
        //    DIRECT-SEVERITY-LOGGER    //
        //////////////////////////////////
        public void Debug(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0) =>
            Log(LogSeverity.Debug, message, callerFile, callerMember, callerLine);

        public void Info(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0) =>
            Log(LogSeverity.Info, message, callerFile, callerMember, callerLine);

        public void Warning(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0) =>
            Log(LogSeverity.Warning, message, callerFile, callerMember, callerLine);

        public void Error(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0) =>
            Log(LogSeverity.Error, message, callerFile, callerMember, callerLine);

        public void Critical(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0) =>
            Log(LogSeverity.Critical, message, callerFile, callerMember, callerLine);


        //////////////////////////////////
        // ASYNC DIRECT-SEVERITY-LOGGER //
        //////////////////////////////////
        public Task DebugAsync(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0) =>
            LogAsync(LogSeverity.Debug, message, callerFile, callerMember, callerLine);

        public Task InfoAsync(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0) =>
            LogAsync(LogSeverity.Info, message, callerFile, callerMember, callerLine);

        public Task WarningAsync(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0) =>
            LogAsync(LogSeverity.Warning, message, callerFile, callerMember, callerLine);

        public Task ErrorAsync(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0) =>
            LogAsync(LogSeverity.Error, message, callerFile, callerMember, callerLine);

        public Task CriticalAsync(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0) =>
            LogAsync(LogSeverity.Critical, message, callerFile, callerMember, callerLine);

        #endregion
    }
}