using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaLog.Tests {
    public static class Helper {
        public static Exception BuildException() {
            return new Exception("Outest Exception", new Exception("Some inner", new Exception("Innest Exceptin")));
        }
    }
}
