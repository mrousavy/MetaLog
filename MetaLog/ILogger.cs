using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MetaLog {
    /// <summary>
    ///     A MetaLog logging severity enum
    /// </summary>
    public enum LogSeverity {
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
    public interface ILogger : IDisposable {
        /// <summary>
        ///     The <see cref="Encoding" /> this <see cref="ILogger" /> instance uses
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        ///     The log file this <see cref="ILogger" /> instance logs to
        /// </summary>
        string LogFile { get; set; }

        /// <summary>
        ///     Whether this <see cref="ILogger" /> uses a single
        ///     (File)-<see cref="Stream" /> for logging, or opens new
        ///     ones each log. (<see cref="Stream" />s may be faster, but
        ///     locks the file until the <see cref="ILogger" /> gets disposed, see:
        ///     <see href="https://en.wikipedia.org/wiki/File_locking">file locking</see>)
        /// </summary>
        bool UseStream { get; set; }

        /// <summary>
        ///     The minimum <see cref="LogSeverity" /> to log by this Logger instance
        ///     (It is recommended to use higher values such as <see cref="LogSeverity.Error" />
        ///     for release builds)
        /// </summary>
        LogSeverity MinimumSeverity { get; set; }

        /// <summary>
        ///     Log a new message to the <see cref="LogFile" />
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity" /> of this message</param>
        /// <param name="message">The actual log-message</param>
        /// <param name="member">The calling member for this Log message</param>
        /// <param name="file">The calling source file for this Log message</param>
        /// <param name="line">The line number in the calling file for this Log message</param>
        void Log(LogSeverity severity, string message,
            [CallerFilePath] string file = null,
            [CallerMemberName] string member = null,
            [CallerLineNumber] int line = 0);

        /// <summary>
        ///     Log a new <see cref="Exception" /> tree (up to most
        ///     inner <see cref="Exception" />) to the <see cref="LogFile" />
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity" /> of this message</param>
        /// <param name="exception">An occured <see cref="Exception" /></param>
        /// <param name="member">The calling member for this Log message</param>
        /// <param name="indent">The amount of whitespaces to put before the Exception tree</param>
        /// <param name="file">The calling source file for this Log message</param>
        /// <param name="line">The line number in the calling file for this Log message</param>
        void Log(LogSeverity severity, Exception exception, int indent = 2,
            [CallerFilePath] string file = null,
            [CallerMemberName] string member = null,
            [CallerLineNumber] int line = 0);

        /// <summary>
        ///     Log a new message to the <see cref="LogFile" /> async
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity" /> of this message</param>
        /// <param name="message">The actual log-message</param>
        /// <param name="member">The calling member for this Log message</param>
        /// <param name="file">The calling source file for this Log message</param>
        /// <param name="line">The line number in the calling file for this Log message</param>
        Task LogAsync(LogSeverity severity, string message,
            [CallerFilePath] string file = null,
            [CallerMemberName] string member = null,
            [CallerLineNumber] int line = 0);

        /// <summary>
        ///     Log a new <see cref="Exception" /> tree (up to most
        ///     inner <see cref="Exception" />) to the <see cref="LogFile" /> async
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity" /> of this message</param>
        /// <param name="exception">An occured <see cref="Exception" /></param>
        /// <param name="member">The calling member for this Log message</param>
        /// <param name="indent">The amount of whitespaces to put before the Exception tree</param>
        /// <param name="file">The calling source file for this Log message</param>
        /// <param name="line">The line number in the calling file for this Log message</param>
        Task LogAsync(LogSeverity severity, Exception exception, int indent = 2,
            [CallerFilePath] string file = null,
            [CallerMemberName] string member = null,
            [CallerLineNumber] int line = 0);
    }
}