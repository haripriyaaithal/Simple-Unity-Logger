using SimpleLogger.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleLogger.IO
{
    public abstract class BaseLogWriter : ILogWriter
    {
        public virtual void WriteLogs(IEnumerable<LogData> logs, string fileName, string description,
            Action<string, string> onComplete) {  }

        protected string ProcessLogs(IEnumerable<LogData> logs)
        {
            var stringBuilder = new StringBuilder();
            foreach (var log in logs)
            {
                stringBuilder.AppendLine($"[{log.time}]");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(log.info);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(log.stackstrace);
                stringBuilder.AppendLine("-----------------------------------------------");
                stringBuilder.AppendLine();
            }
            stringBuilder.AppendLine("End of logs.");

            return stringBuilder.ToString();
        }
    }
}