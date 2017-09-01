using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MetaLog {
    public class MetaLogger : ILogger {

        public LogSeverity MinimumSeverity { get; set; }
        #endregion

        #region Functions

        private void ToggleStream() {
        }

        public void Dispose() {
        }

        #endregion

        #region Logging

        public void Log(LogSeverity severity, string message,
        }

        public void Log(LogSeverity severity, Exception exception, int indent = 2,
        }

        public async Task LogAsync(LogSeverity severity, string message,
        }

        public async Task LogAsync(LogSeverity severity, Exception exception, int indent = 2,
        }

        #endregion
    }
}
