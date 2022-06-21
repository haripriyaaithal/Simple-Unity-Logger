using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleLogger.Data;

namespace SimpleLogger
{
    public class LogManager : MonoBehaviour
    {
        [SerializeField] private bool _useTargetFrameRate;
        [SerializeField] private int _targetFrameRate = 60;

        public static LogManager Instance;

        private readonly IList<LogData> _logData = new List<LogData>();

        private void Awake()
        {
#if !USE_SIMPLE_LOGGER
            Destroy(gameObject);
            return;
#endif

            if (_useTargetFrameRate)
                Application.targetFrameRate = _targetFrameRate;

            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        private void OnEnable()
        {
            Application.logMessageReceived += OnLogReceived;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= OnLogReceived;
        }

        private void OnLogReceived(string condition, string stacktrace, LogType type)
        {
            var logType = GetUnityLogType(type);
            var log = new LogData
            {
                info = condition,
                stackstrace = stacktrace,
                logType = logType,
                time = DateTime.Now.ToString("hh:mm:ss tt")
            };

            _logData.Add(log);
        }

        private UnityLogType GetUnityLogType(LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    return UnityLogType.Log;
                case LogType.Warning:
                    return UnityLogType.Warning;
                default:
                    return UnityLogType.Error;
            }
        }

        public IEnumerable<LogData> GetLogs()
        {
            return _logData;
        }

        public IEnumerable<LogData> GetLogs(byte type, string text = "")
        {
            var logs = ((List<LogData>)_logData).FindAll(log =>
            {
                var logType = (byte)log.logType;
                return (logType & type) == logType;
            });

            if (string.IsNullOrEmpty(text))
                return logs;

            return logs.FindAll(log => log.info.Contains(text, StringComparison.OrdinalIgnoreCase)
                                       || log.stackstrace.Contains(text, StringComparison.OrdinalIgnoreCase));
        }

        public void ClearLogs()
        {
            _logData.Clear();
        }
    }
}