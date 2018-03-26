using System;
using System.IO;
using System.Text;

// ReSharper disable ExplicitCallerInfoArgument

namespace MetaLog
{
    /// <summary>
    ///     A static <see cref="ILogger" />
    /// </summary>
    public static class Logger
    {
        public static ILogger Instance { get; } = new MetaLogger(Console.OpenStandardOutput());

        #region ctor

        /// <summary>
        ///     Create a new <see cref="ILogger" /> instance with the given properties
        /// </summary>
        /// <param name="logfile">The file to log to</param>
        /// <returns>An initialized <see cref="ILogger" /></returns>
        public static ILogger New(string logfile) => new MetaLogger(logfile);

        /// <summary>
        ///     Create a new <see cref="ILogger" /> instance with the given properties
        /// </summary>
        /// <param name="logfile">The file to log to</param>
        /// <param name="minSeverity">The minimum severity to log messages to</param>
        /// <returns>An initialized <see cref="ILogger" /></returns>
        public static ILogger New(string logfile, LogSeverity minSeverity) => new MetaLogger(logfile, minSeverity);

        /// <summary>
        ///     Create a new <see cref="ILogger" /> instance with the given properties
        /// </summary>
        /// <param name="logfile"></param>
        /// <param name="minSeverity">The minimum severity to log messages to</param>
        /// <param name="encoding">The encoding to use for writing strings</param>
        /// <returns>An initialized <see cref="ILogger" /></returns>
        public static ILogger New(string logfile, LogSeverity minSeverity, Encoding encoding) =>
            new MetaLogger(logfile, minSeverity, encoding);

        /// <summary>
        ///     Create a new <see cref="ILogger" /> instance with the given properties
        /// </summary>
        /// <param name="stream">The stream to log to</param>
        /// <returns>An initialized <see cref="ILogger" /></returns>
        public static ILogger New(Stream stream) => new MetaLogger(stream);

        /// <summary>
        ///     Create a new <see cref="ILogger" /> instance with the given properties
        /// </summary>
        /// <param name="stream">The stream to log to</param>
        /// <param name="minSeverity">The minimum severity to log messages to</param>
        /// <returns>An initialized <see cref="ILogger" /></returns>
        public static ILogger New(Stream stream, LogSeverity minSeverity) => new MetaLogger(stream, minSeverity);

        /// <summary>
        ///     Create a new <see cref="ILogger" /> instance with the given properties
        /// </summary>
        /// <param name="stream">The stream to log to</param>
        /// <param name="minSeverity">The minimum severity to log messages to</param>
        /// <param name="encoding">The encoding to use for writing strings</param>
        /// <returns>An initialized <see cref="ILogger" /></returns>
        public static ILogger New(Stream stream, LogSeverity minSeverity, Encoding encoding) 
            => new MetaLogger(stream, minSeverity, encoding);

        #endregion
    }
}