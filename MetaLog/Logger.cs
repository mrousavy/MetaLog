using System;
using System.IO;

namespace MetaLog {
    /// <summary>
    /// A static <see cref="ILogger"/>
    /// </summary>
    public static class Logger {
        private static bool _useStream;
        private static string _logFile = Path.Combine(Helper.MetaLogAppData, $"{Helper.ExecutingAssemblyName}.log");

        #region Properties
        /// <summary>
        /// Lock object so no logging interferes
        /// </summary>
        private static object Lock { get; } = new object();
        
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
