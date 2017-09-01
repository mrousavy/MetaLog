namespace MetaLog {

    /// <summary>
    /// A MetaLog logging severity enum
    /// </summary>
    public enum LogSeverity {
        /// <summary>
        /// Indicating the log-message is for debugging
        /// </summary>
        Debug,
        /// <summary>
        /// Indicating the log-message is some kind of 
        /// information or progress update
        /// </summary>
        Info,
        /// <summary>
        /// Indicating the log-message is an ignorable
        /// warning or usual exception
        /// </summary>
        Warning,
        /// <summary>
        /// Indicating the log-message is a runtime-affecting
        /// error or unexpected exception
        /// </summary>
        Error,
        /// <summary>
        /// Indicating the log-message is an unexpected exception
        /// which may prevent the application from continuing
        /// </summary>
        Critical
    }

    /// <summary>
    /// A MetaLog logger
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// The log file this <see cref="ILogger"/> instance logs to
        /// </summary>
        string LogFile { get; }
        /// <summary>
        /// The minimum <see cref="LogSeverity"/> to log by this Logger instance
        /// (It is recommended to use higher values such as <see cref="LogSeverity.Error"/>
        /// for releases)
        /// </summary>
        LogSeverity MinimumSeverity { get; }

        /// <summary>
        /// Log a new message to the <see cref="LogFile"/>
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity"/> of this message</param>
        /// <param name="message">The actual log-message</param>
        void Log(LogSeverity severity, string message);
    }
}
