using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetaLog.Tests {
    [TestClass]
    public class LoggerTests {
        public static ILogger logger;

        [TestInitialize]
        public void Initialize() {
            logger = new MetaLog.Logger.New();
        }

        [TestMethod]
        public void TestLog1() {

        }
        [TestMethod]
        public void TestLog2() {

        }
        [TestMethod]
        public void TestLog3() {

        }
        [TestMethod]
        public void TestLog4() {

        }
    }
}
