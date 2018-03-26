using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetaLog.Tests
{
    [TestClass]
    public class LoggerTests
    {
        public static ILogger Logger;

        public static string LogDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MetaLog.Tests");

        public static string LogFile = Path.Combine(LogDir, "log.log");

        [TestInitialize]
        public void Init()
        {
            if (!Directory.Exists(LogDir)) Directory.CreateDirectory(LogDir);
            Logger = MetaLog.Logger.New(LogFile, LogSeverity.Info);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Logger.Dispose();
        }

        [TestMethod]
        public void TestThrowExceptionLog()
        {
            Logger.Log(LogSeverity.Info, "Starting Exception-Log Test.");
            try
            {
                throw Helper.BuildException();
            } catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex);
            }

            Logger.Log(LogSeverity.Info, "Exception-Log Test is about to finish.");
        }

        [TestMethod]
        public void TestExceptionLog()
        {
            var ex = new Exception("Exception message.");
            Logger.Log(LogSeverity.Error, ex);
        }

        [TestMethod]
        public void TestExceptionLogAsync()
        {
            var ex = new Exception("Exception message.");
            var task = Logger.LogAsync(LogSeverity.Error, ex);
            task.GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestLog()
        {
            Logger.Log(LogSeverity.Error, "Testing basic text logging.");
        }

        [TestMethod]
        public void TestLogAsync()
        {
            var task = Logger.LogAsync(LogSeverity.Error, "Testing basic text logging.");
            task.GetAwaiter().GetResult();
        }
    }
}