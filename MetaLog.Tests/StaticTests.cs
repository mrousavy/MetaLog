using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetaLog.Tests
{
    [TestClass]
    public class StaticTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Logger.UseStream = true;
        }

        [TestMethod]
        public void StaticTestThrowExceptionLog()
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
        public void StaticTestExceptionLog()
        {
            var ex = new Exception("Exception message.");
            Logger.Log(LogSeverity.Error, ex);
        }

        [TestMethod]
        public void StaticTestExceptionLogAsync()
        {
            var ex = new Exception("Exception message.");
            var task = Logger.LogAsync(LogSeverity.Error, ex);
            task.GetAwaiter().GetResult();
        }

        [TestMethod]
        public void StaticTestLog()
        {
            Logger.Log(LogSeverity.Error, "Testing basic text logging.");
        }

        [TestMethod]
        public void StaticTestLogAsync()
        {
            var task = Logger.LogAsync(LogSeverity.Error, "Testing basic text logging.");
            task.GetAwaiter().GetResult();
        }
    }
}