using System;

namespace MetaLog.Tests
{
    public static class Helper
    {
        public static Exception BuildException() => new Exception("Outest Exception",
            new Exception("Some inner", new Exception("Innest Exception")));
    }
}