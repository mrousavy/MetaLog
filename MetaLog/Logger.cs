using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace MetaLog {
    /// <summary>
    /// A static <see cref="ILogger"/>
    /// </summary>
    public static class Logger {
        private static bool _useStream;
        private static string _logFile = Path.Combine(Utilities.MetaLogAppData, $"{Utilities.ExecutingAssemblyName}.log");

        #region Properties
        /// <summary>
        /// Lock object so no logging interferes
        /// </summary>
        private static object Lock { get; } = new object();

        /// <summary>
        /// The <see cref="Encoding"/> this <see cref="ILogger"/> instance uses
        /// </summary>
        private static readonly Encoding Encoding = Encoding.Unicode;
        /// <summary>
        /// The <see cref="FileStream"/> used for writing to the LogFile
        /// </summary>
        private static FileStream FileStream { get; set; }
        /// <summary>
        /// The path to the log file this <see cref="ILogger"/> 
        /// instance logs to.
        /// By default, this value is %appdata%/MetaLog/[YourAssemblyName].log
        /// <para/>
        /// (Setting this to null will close the
        /// Stream if <see cref="UseStream"/> is set to true)
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">Thrown when the directory of the
        /// file does not exist. Create the Directory of the <see cref="LogFile"/>
        /// before setting this value</exception>
        public static string LogFile {
            get => _logFile;
            set {
                if (value == _logFile) return; //don't reopen stream if it's same value
                _logFile = value;
                ToggleStream();
            }
        }
        /// <summary>
        /// Whether this <see cref="ILogger"/> uses a single
        /// (File)-<see cref="Stream"/> for logging, or opens new
        /// ones each log. (<see cref="Stream"/>s may be faster, but
        /// locks the file until the <see cref="ILogger"/> gets disposed, see: 
        /// <see href="https://en.wikipedia.org/wiki/File_locking">file locking</see>)
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">Thrown when the directory of the
        /// file does not exist. Create the Directory of the <see cref="LogFile"/>
        /// before setting this value</exception>
        public static bool UseStream {
            get => _useStream;
            set {
                if (value == _useStream) return; //don't change stream if it's same value
                _useStream = value;
                ToggleStream();
            }
        }
        /// <summary>
        /// The minimum <see cref="LogSeverity"/> to log by this <see cref="Logger"/>
        /// <em>(It is recommended to use higher values such as <see cref="LogSeverity.Error"/>
        /// for release builds)</em>
        /// </summary>
        public static LogSeverity MinimumSeverity { get; set; }
        #endregion

        #region Functions

        /// <summary>
        /// <em>(Re-)</em>open or close the <see cref="FileStream"/> to 
        /// the <em>(new)</em> <see cref="LogFile"/> depending on the 
        /// <see cref="UseStream"/> property
        /// </summary>
        private static void ToggleStream() {
            lock (Lock) { //lock to our lock object so we don't close a stream mid-write
                FileStream?.Dispose(); //dispose the stream if open
                if (LogFile != null && UseStream) { //open filestream if Path is not null and Logger uses streams
                    //create a new filestream to the LogFile (create if file does not exist, and seek to end)
                    FileStream = new FileStream(LogFile, FileMode.Append, FileAccess.Write);
                    FileStream.Position = FileStream.Length; //set position to end
                }
            }
        }

        /// <summary>
        /// Release all resources and close any Streams
        /// </summary>
        public static void Dispose() {
            FileStream?.Dispose();
        }

        #endregion

        #region Logging

        /// <summary>
        /// Log a new message to the <see cref="LogFile"/>
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity"/> of this message</param>
        /// <param name="message">The actual log-message</param>
        /// <param name="member">The calling member for this Log message</param>
        /// <param name="file">The calling source file for this Log message</param>
        /// <param name="line">The line number in the calling file for this Log message</param>
        public static void Log(LogSeverity severity, string message,
            [CallerFilePath] string file = null,
            [CallerMemberName] string member = null,
            [CallerLineNumber] int line = 0) {
            if (severity < MinimumSeverity) return; //don't log if it's below min severity

            string text = Utilities.BuildMessage(severity, message, file, member, line); //construct the message

            lock (Lock) { //lock to sync object to prevent inconsistency
                if (UseStream) {
                    byte[] bytes = Encoding.GetBytes(text);
                    FileStream.Write(bytes, 0, bytes.Length);
                } else {
                    File.AppendAllText(LogFile, text);
                }
            }
        }

        /// <summary>
        /// Log a new <see cref="Exception"/> tree (up to most 
        /// inner <see cref="Exception"/>) to the <see cref="LogFile"/>
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity"/> of this message</param>
        /// <param name="exception">An occured <see cref="Exception"/></param>
        /// <param name="member">The calling member for this Log message</param>
        /// <param name="file">The calling source file for this Log message</param>
        /// <param name="line">The line number in the calling file for this Log message</param>
        public static void Log(LogSeverity severity, Exception exception,
            [CallerFilePath] string file = null,
            [CallerMemberName] string member = null,
            [CallerLineNumber] int line = 0) {
            if (severity < MinimumSeverity) return; //don't log if it's below min severity

            string message = Utilities.RecurseException(exception); //build exception tree
            string text = Utilities.BuildMessage(severity, message, file, member, line); //construct the message

            lock (Lock) { //lock to sync object to prevent inconsistency
                if (UseStream) {
                    //write via filestream
                    byte[] bytes = Encoding.GetBytes(text);
                    FileStream.Write(bytes, 0, bytes.Length);
                } else {
                    //write via Sytem.IO.File helper
                    File.AppendAllText(LogFile, text);
                }
            }
        }

        #endregion
    }
}
