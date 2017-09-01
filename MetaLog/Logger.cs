using System;
using System.IO;

namespace MetaLog {
    /// <summary>
    /// A static <see cref="ILogger"/>
    /// </summary>
    public static class Logger {
        private static bool _useStream;
        private static string _logFile;

        private static object Lock { get; } = new object();

        /// <summary>
        /// The <see cref="FileStream"/> used for writing to the LogFile
        /// </summary>
        private static FileStream FileStream { get; set; }
        /// <summary>
        /// The log file this <see cref="ILogger"/> instance logs to
        /// (Setting this to null will close the Stream if 
        /// <see cref="Logger.UseStream"/> is set to true
        /// </summary>
        public static string LogFile {
            get => _logFile;
            set {
                if (value == _logFile) return; //don't reopen stream if it's same value
                _logFile = value;
                ReOpenStream();
            }
        }
        /// <summary>
        /// Whether this <see cref="ILogger"/> uses a single
        /// (File)-<see cref="Stream"/> for logging, or opens new
        /// ones each log. (<see cref="Stream"/>s may be faster, but
        /// locks the file until the <see cref="ILogger"/> gets disposed, see: 
        /// <see href="https://en.wikipedia.org/wiki/File_locking">file locking</see>)
        /// </summary>
        public static bool UseStream { get; set; }
        /// <summary>
        /// The minimum <see cref="LogSeverity"/> to log by this Logger instance
        /// (It is recommended to use higher values such as <see cref="LogSeverity.Error"/>
        /// for releases)
        /// </summary>
        public static LogSeverity MinimumSeverity { get; set; }

        private static void ReOpenStream() {
            lock (Lock) { //lock to our lock object so we don't close a stream mid-write
                FileStream?.Dispose(); //dispose the stream if open
                if (LogFile != null && UseStream) { //open filestream if Path is not null and Logger uses streams
                    //create a new filestream to the LogFile (create if file does not exist, and seek to end)
                    FileStream = new FileStream(LogFile, FileMode.Append, FileAccess.Write);
                }
            }
        }


        /// <summary>
        /// Log a new message to the <see cref="LogFile"/>
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity"/> of this message</param>
        /// <param name="message">The actual log-message</param>
        public static void Log(LogSeverity severity, string message) {
            
        }

        /// <summary>
        /// Log a new <see cref="Exception"/> tree (up to most 
        /// inner <see cref="Exception"/>) to the <see cref="LogFile"/>
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity"/> of this message</param>
        /// <param name="exception">An occured <see cref="Exception"/></param>
        public static void Log(LogSeverity severity, Exception exception) {
            
        }
    }
}
