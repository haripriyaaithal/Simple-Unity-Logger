using SimpleLogger.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SimpleLogger.IO
{
    public class LocalLogWriter : BaseLogWriter
    {
        public override async void WriteLogs(IEnumerable<LogData> logs, string fileName, string description, Action<string, string> onComplete)
        {
            var logString = $"{description}\n\n{ProcessLogs(logs)}";
            var path = Path.Combine(UnityEngine.Application.persistentDataPath, "Logs");
            await SaveLogsAsync(logString, path, fileName, onComplete);
        }

        private async Task SaveLogsAsync(string logString, string path, string fileName, Action<string, string> onComplete)
        {
            await Task.Run(async () =>
            {
                try
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    var filePath = Path.Combine(path, $"{fileName} - {DateTime.Now.ToString("dd MMMM - hh_mm_ss tt")}.txt");
                    await File.WriteAllTextAsync(filePath, logString);
                    onComplete.Invoke(filePath, string.Empty);
                }
                catch (Exception e)
                {
                    onComplete.Invoke(string.Empty, e.Message);
                }
            });
        }
    }
}