namespace SimpleLogger.Data
{
    [System.Serializable]
    public class LogData
    {
        public string info;
        public string stackstrace;
        public UnityLogType logType;
        public string time;
    }

    public enum UnityLogType : byte
    {
        Log = 1,
        Warning = 2,
        Error = 4
    }
}