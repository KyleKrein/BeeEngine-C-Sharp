namespace BeeEngine
{
    public interface ILogger
    {
        void Log(string o, LogLevel logLevel);
        void Debug(string o);
        void Error(string o);
        void Warning(string o);
        void Info(string o);
    }
}