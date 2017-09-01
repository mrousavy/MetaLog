using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetaLog.Tests {
    [TestClass]
    public class StaticTests {

        [TestInitialize]
        public void Initialize() {
            MetaLog.Logger.UseStream = true;
        }

        [TestMethod]
        public void StaticTestThrowExceptionLog() {
            MetaLog.Logger.Log(MetaLog.LogSeverity.Info, "Starting Exception-Log Test.");
            try {
                throw Helper.BuildException();
            } catch (Exception ex) {
                MetaLog.Logger.Log(MetaLog.LogSeverity.Error, ex);
            }
            MetaLog.Logger.Log(MetaLog.LogSeverity.Info, "Exception-Log Test is about to finish.");
        }
        [TestMethod]
        public void StaticTestExceptionLog() {
            var ex = new Exception("Exception message.");
            MetaLog.Logger.Log(MetaLog.LogSeverity.Error, ex);
        }
        [TestMethod]
        public void StaticTestExceptionLogAsync() {
            var ex = new Exception("Exception message.");
            Task task = MetaLog.Logger.LogAsync(MetaLog.LogSeverity.Error, ex);
            task.GetAwaiter().GetResult();
        }
        [TestMethod]
        public void StaticTestLog() {
            MetaLog.Logger.Log(MetaLog.LogSeverity.Error, "Testing basic text logging.");
        }
        [TestMethod]
        public void StaticTestLogAsync() {
            Task task = MetaLog.Logger.LogAsync(MetaLog.LogSeverity.Error, "Testing basic text logging.");
            task.GetAwaiter().GetResult();
        }
    }
}
