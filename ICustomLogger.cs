using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggersComparison
{

    public enum LoggingLevel
    {
        INF,
        WRN,
        ERR
    }
    public interface ICustomLogger
    {
        public string Location { get; set; }
        void Log(string EventId, string DiagnosticMessage, LoggingLevel Level);
        Task LogAsync(string EventId, string DiagnosticMessage, LoggingLevel Level);
    }
}
