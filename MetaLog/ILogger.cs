namespace MetaLog {
    public enum LogSeverity {
        Debug,
        Info,
        Warning,
        Error,
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
