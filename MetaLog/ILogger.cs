using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MetaLog
{
    /// <summary>
    ///     A MetaLog logging severity enum
    /// </summary>
    public enum LogSeverity
    {
        /// <summary>
        ///     Indicating the log-message is for debugging
        /// </summary>
        Debug,

        /// <summary>
        ///     Indicating the log-message is some kind of
        ///     information or progress update
        /// </summary>
        Info,

        /// <summary>
        ///     Indicating the log-message is an ignorable
        ///     warning or usual exception
        /// </summary>
        Warning,

        /// <summary>
        ///     Indicating the log-message is a runtime-affecting
        ///     error or unexpected exception
        /// </summary>
        Error,

        /// <summary>
        ///     Indicating the log-message is an unexpected exception
        ///     which may prevent the application from continuing
        /// </summary>
        Critical
    }

    /// <summary>
    ///     A MetaLog logger
    /// </summary>
    public interface ILogger : IDisposable
    {
        /// <summary>
        ///     The <see cref="Encoding" /> this <see cref="ILogger" /> instance uses
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        ///     The <see cref="Stream"/> this <see cref="ILogger"/> instance writes to
        /// </summary>
        Stream Stream { get; set; }

        /// <summary>
        ///     The minimum <see cref="LogSeverity" /> to log by this Logger instance
        ///     (It is recommended to use higher values such as <see cref="LogSeverity.Error" />
        ///     for release builds)
        /// </summary>
        LogSeverity MinimumSeverity { get; set; }

        /// <summary>
        ///     Log a new message to the log stream
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity" /> of this message</param>
        /// <param name="message">The actual log-message</param>
        /// <param name="callerMember">The calling member for this Log message</param>
        /// <param name="callerFile">The calling source file for this Log message</param>
        /// <param name="callerLine">The line number in the calling file for this Log message</param>
        void Log(LogSeverity severity, string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);

        /// <summary>
        ///     Log a new <see cref="Exception" /> tree (up to most
        ///     inner <see cref="Exception" />) to the <see cref="LogFile" />
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity" /> of this message</param>
        /// <param name="exception">An occured <see cref="Exception" /></param>
        /// <param name="indent">The amount of whitespaces to put before the Exception tree</param>
        /// <param name="callerFile">The calling source file for this Log message</param>
        /// <param name="callerMember">The calling member for this Log message</param>
        /// <param name="callerLine">The line number in the calling file for this Log message</param>
        void Log(LogSeverity severity, Exception exception, int indent = 2,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);

        /// <summary>
        ///     Log a new message to the <see cref="LogFile" /> async
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity" /> of this message</param>
        /// <param name="message">The actual log-message</param>
        /// <param name="callerFile">The calling source file for this Log message</param>
        /// <param name="callerMember">The calling member for this Log message</param>
        /// <param name="callerLine">The line number in the calling file for this Log message</param>
        Task LogAsync(LogSeverity severity, string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);

        /// <summary>
        ///     Log a new <see cref="Exception" /> tree (up to most
        ///     inner <see cref="Exception" />) to the <see cref="LogFile" /> async
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity" /> of this message</param>
        /// <param name="exception">An occured <see cref="Exception" /></param>
        /// <param name="indent">The amount of whitespaces to put before the Exception tree</param>
        /// <param name="callerFile">The calling source file for this Log message</param>
        /// <param name="callerMember">The calling member for this Log message</param>
        /// <param name="callerLine">The line number in the calling file for this Log message</param>
        Task LogAsync(LogSeverity severity, Exception exception, int indent = 2,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);

        /// <summary>
        ///     Log a new message to the <see cref="LogFile" /> of severity <see cref="LogSeverity.Debug" />
        /// </summary>
        /// <param name="message">The actual log-message</param>
        /// <param name="callerMember">The calling member for this Log message</param>
        /// <param name="callerFile">The calling source file for this Log message</param>
        /// <param name="callerLine">The line number in the calling file for this Log message</param>
        void Debug(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);

        /// <summary>
        ///     Log a new message to the <see cref="LogFile" /> of severity <see cref="LogSeverity.Info" />
        /// </summary>
        /// <param name="message">The actual log-message</param>
        /// <param name="callerMember">The calling member for this Log message</param>
        /// <param name="callerFile">The calling source file for this Log message</param>
        /// <param name="callerLine">The line number in the calling file for this Log message</param>
        void Info(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);

        /// <summary>
        ///     Log a new message to the <see cref="LogFile" /> of severity <see cref="LogSeverity.Warning" />
        /// </summary>
        /// <param name="message">The actual log-message</param>
        /// <param name="callerMember">The calling member for this Log message</param>
        /// <param name="callerFile">The calling source file for this Log message</param>
        /// <param name="callerLine">The line number in the calling file for this Log message</param>
        void Warning(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);

        /// <summary>
        ///     Log a new message to the <see cref="LogFile" /> of severity <see cref="LogSeverity.Error" />
        /// </summary>
        /// <param name="message">The actual log-message</param>
        /// <param name="callerMember">The calling member for this Log message</param>
        /// <param name="callerFile">The calling source file for this Log message</param>
        /// <param name="callerLine">The line number in the calling file for this Log message</param>
        void Error(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);

        /// <summary>
        ///     Log a new message to the <see cref="LogFile" /> of severity <see cref="LogSeverity.Critical" />
        /// </summary>
        /// <param name="message">The actual log-message</param>
        /// <param name="callerMember">The calling member for this Log message</param>
        /// <param name="callerFile">The calling source file for this Log message</param>
        /// <param name="callerLine">The line number in the calling file for this Log message</param>
        void Critical(string message,
            [CallerFilePath] string callerFile = null,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);
    }
}