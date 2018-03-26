using System;

namespace MetaLog.Tests
{
    public static class Helper
    {
        public static Exception BuildException() =>
            new Exception("Outest Exception",
                new Exception("Inner #1",
                    new Exception("Inner #2",
                        new Exception("Inner #3",
                            new Exception("Innest Exception")))));
    }
}