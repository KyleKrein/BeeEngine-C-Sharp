using System.Diagnostics.CodeAnalysis;

namespace BeeEngine
{
    public interface ILogger
    {
        void Log(string o, LogLevel logLevel);
        void Info([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object[] args);
        void Debug([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object[] args);
        void Error([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object[] args);
        void Warning([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object[] args);
        void Debug(string o);
        void Error(string o);
        void Warning(string o);
        void Info(string o);
    }
}