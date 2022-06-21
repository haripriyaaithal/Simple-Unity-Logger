using SimpleLogger.Data;
using System;
using System.Collections.Generic;

namespace SimpleLogger.IO
{
    public interface ILogWriter
    {
        /// <param name="logs">List of log data that needs to be saved.</param>
        /// <param name="fileName">File name of the log file that needs to be created.</param>
        /// <param name="description">Description for the logs.</param>
        /// <param name="onComplete">Callback function which is invoked after the saving process is complete or when error occurs.
        /// It takes two string parameters, first string returns the URL if the save was successful, second string return error message if any.</param>
        public void WriteLogs(IEnumerable<LogData> logs, string fileName, string description, Action<string, string> onComplete);
    }
}