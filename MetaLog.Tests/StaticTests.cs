using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetaLog.Tests {
    [TestClass]
    public class StaticTests {

        [TestInitialize]
        public void Initialize() {
            MetaLog.Logger.UseStream = true;
            MetaLog.Logger.Encoding = Encoding.ASCII;
        }

        [TestMethod]
        public void TestStaticLog1() {
            MetaLog.Logger.Log(MetaLog.LogSeverity.Info, "Starting Test 1.");
            MetaLog.Logger.Log(MetaLog.LogSeverity.Info, Helper.BuildException());
            MetaLog.Logger.Log(MetaLog.LogSeverity.Info, "Test 1 is about to finish.");
        }
        [TestMethod]
        public void TestStaticLog2() {

        }
        [TestMethod]
        public void TestStaticLog3() {

        }
        [TestMethod]
        public void TestStaticLog4() {

        }
    }
}
