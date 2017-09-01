using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MetaLog {
    public class MetaLogger : ILogger {
        private bool _useStream;
        private string _logFile = Path.Combine(Utilities.MetaLogAppData, $"{Utilities.ExecutingAssemblyName}.log");

        #region ctor
        /// <summary>
        /// Create a new MetaLogger with the given filename
        /// </summary>
        /// <param name="filename">The LogFile</param>
        public MetaLogger(string filename) : this(filename, LogSeverity.Info, false) { }

        /// <summary>
        /// Create a new MetaLogger with the given filename
        /// </summary>
        /// <param name="filename">The LogFile</param>
        /// <param name="minSeverity">The LogFile</param>
        public MetaLogger(string filename, LogSeverity minSeverity) : this(filename, minSeverity, false) { }

        /// <summary>
        /// Create a new MetaLogger with the given filename
        /// </summary>
        /// <param name="filename">The LogFile</param>
        /// <param name="minSeverity">The LogFile</param>
        /// <param name="useStream">The LogFile</param>
        public MetaLogger(string filename, LogSeverity minSeverity, bool useStream) {
            LogFile = filename;
            MinimumSeverity = minSeverity;
            UseStream = useStream;
        }
        #endregion

        #region Properties
        private object Lock { get; } = new object();
        private FileStream FileStream { get; set; }

        public Encoding Encoding { get; set; } = Encoding.Unicode;

        public string LogFile {
            get => _logFile;
            set {
                if (value == _logFile) return; //don't reopen stream if it's same value
                _logFile = value;
                ToggleStream();
            }
        }

        public bool UseStream {
            get => _useStream;
            set {
                if (value == _useStream) return; //don't change stream if it's same value
                _useStream = value;
                ToggleStream();
            }
        }

        public LogSeverity MinimumSeverity { get; set; }
        #endregion

        #region Functions

        private void ToggleStream() {
            if (FileStream?.Name == LogFile) return; //no need to reopen stream if it's on our file

            lock (Lock) { //lock to our lock object so we don't close a stream mid-write
                FileStream?.Dispose(); //dispose the stream if open
                if (LogFile != null && UseStream) { //open filestream if Path is not null and Logger uses streams
                    //create a new filestream to the LogFile (create if file does not exist, and seek to end)
                    FileStream = new FileStream(LogFile, FileMode.Append, FileAccess.Write);
                    FileStream.Position = FileStream.Length; //set position to end
                }
            }
        }

        public void Dispose() {
            lock (Lock) {
                //lock so we don't interrupt a FileStream's write op
                FileStream?.Dispose();
            }
        }

        #endregion

        #region Logging

        public void Log(LogSeverity severity, string message,
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

        public void Log(LogSeverity severity, Exception exception, int indent = 2,
            [CallerFilePath] string file = null,
            [CallerMemberName] string member = null,
            [CallerLineNumber] int line = 0) {
            if (severity < MinimumSeverity) return; //don't log if it's below min severity

            string message = Utilities.RecurseException(exception, 2); //build exception tree
            message = Utilities.BuildTree(message);
            message = Utilities.Indent(message, indent);
            message = $"BEGIN EXCEPTION TREE:{Utilities.Nl}{message}";
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

        public async Task LogAsync(LogSeverity severity, string message,
            [CallerFilePath] string file = null,
            [CallerMemberName] string member = null,
            [CallerLineNumber] int line = 0) {
            if (severity < MinimumSeverity) return; //don't log if it's below min severity

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            new Thread(() => {
                try {
                    string text = Utilities.BuildMessage(severity, message, file, member, line); //construct the message

                    lock (Lock) {
                        //lock to sync object to prevent inconsistency
                        if (UseStream) {
                            byte[] bytes = Encoding.GetBytes(text);
                            FileStream.Write(bytes, 0, bytes.Length);
                        } else {
                            File.AppendAllText(LogFile, text);
                        }
                    }
                    tcs.SetResult(true);
                } catch (Exception ex) {
                    tcs.SetException(ex);
                }
            }).Start();

            await tcs.Task;
        }

        public async Task LogAsync(LogSeverity severity, Exception exception, int indent = 2,
            [CallerFilePath] string file = null,
            [CallerMemberName] string member = null,
            [CallerLineNumber] int line = 0) {
            if (severity < MinimumSeverity) return; //don't log if it's below min severity

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            new Thread(() => {
                try {
                    string message = Utilities.RecurseException(exception, 2); //build exception tree
                    message = Utilities.BuildTree(message);
                    message = Utilities.Indent(message, indent);
                    message = $"BEGIN EXCEPTION TREE:{Utilities.Nl}{message}";
                    string text = Utilities.BuildMessage(severity, message, file, member, line); //construct the message

                    lock (Lock) {
                        //lock to sync object to prevent inconsistency
                        if (UseStream) {
                            //write via filestream
                            byte[] bytes = Encoding.GetBytes(text);
                            FileStream.Write(bytes, 0, bytes.Length);
                        } else {
                            //write via Sytem.IO.File helper
                            File.AppendAllText(LogFile, text);
                        }
                    }
                    tcs.SetResult(true);
                } catch (Exception ex) {
                    tcs.SetException(ex);
                }
            }).Start();

            await tcs.Task;
        }

        #endregion
    }
}
