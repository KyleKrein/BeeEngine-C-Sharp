using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BeeEngine.OpenTK;

public static class Log
{
    public static GameLogger Logger { get; }

    static Log()
    {
        Logger = new GameLogger();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info(string o)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Information)
            Logger.Info(o);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug(string o)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Debug)
            Logger.Debug(o);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning(string o)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Warning)
            Logger.Warning(o);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error(string o)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Error)
            Logger.Error(o);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info<T>(string format, T arg)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Information)
            Logger.Info(format, arg);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info<T0, T1>(string format, T0 arg0, T1 arg1)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Information)
            Logger.Info(format, arg0, arg1);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Information)
            Logger.Info(format, arg0, arg1, arg2);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug<T>(string format, T arg)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Debug)
            Logger.Debug(format, arg);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug<T0, T1>(string format, T0 arg0, T1 arg1)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Debug)
            Logger.Debug(format, arg0, arg1);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Debug)
            Logger.Debug(format, arg0, arg1, arg2);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning<T>(string format, T arg)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Warning)
            Logger.Warning(format, arg);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning<T0, T1>(string format, T0 arg0, T1 arg1)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Warning)
            Logger.Warning(format, arg0, arg1);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Warning)
            Logger.Warning(format, arg0, arg1, arg2);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error<T>(string format, T arg)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Error)
            Logger.Error(format, arg);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error<T0, T1>(string format, T0 arg0, T1 arg1)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Error)
            Logger.Error(format, arg0, arg1);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Error)
            Logger.Error(format, arg0, arg1, arg2);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info(string format, params object[] args)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Information)
            Logger.Info(format, args);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug(string format, params object[] args)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Debug)
            Logger.Debug(format, args);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning(string format, params object[] args)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Warning)
            Logger.Warning(format, args);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error(string format, params object[] args)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Error)
            Logger.Error(format, args);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert(bool isOk ,string format, params object[] args)
    {
        if (isOk)
        {
            return;
        }
        Error(format, args);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert<T>(bool isOk ,string format, T arg)
    {
        if (isOk)
        {
            return;
        }
        Error(format, arg);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert<T0, T1>(bool isOk ,string format, T0 arg0, T1 arg1)
    {
        if (isOk)
        {
            return;
        }
        Error(format, arg0, arg1);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert<T0, T1, T2>(bool isOk ,string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (isOk)
        {
            return;
        }
        Error(format, arg0, arg1, arg2);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert(bool isOk ,string o)
    {
        if (isOk)
        {
            return;
        }
        Error(o);
    }
}
public static class DebugLog
{
    public static GameLogger Logger { get; }

    static DebugLog()
    {
        Logger = Log.Logger;
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info(string o)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Information)
            Logger.Info(o);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug(string o)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Debug)
            Logger.Debug(o);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning(string o)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Warning)
            Logger.Warning(o);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error(string o)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Error)
            Logger.Error(o);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info<T>(string format, T arg)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Information)
            Logger.Info(format, arg);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info<T0, T1>(string format, T0 arg0, T1 arg1)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Information)
            Logger.Info(format, arg0, arg1);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Information)
            Logger.Info(format, arg0, arg1, arg2);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug<T>(string format, T arg)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Debug)
            Logger.Debug(format, arg);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug<T0, T1>(string format, T0 arg0, T1 arg1)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Debug)
            Logger.Debug(format, arg0, arg1);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Debug)
            Logger.Debug(format, arg0, arg1, arg2);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning<T>(string format, T arg)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Warning)
            Logger.Warning(format, arg);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning<T0, T1>(string format, T0 arg0, T1 arg1)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Warning)
            Logger.Warning(format, arg0, arg1);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Warning)
            Logger.Warning(format, arg0, arg1, arg2);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error<T>(string format, T arg)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Error)
            Logger.Error(format, arg);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error<T0, T1>(string format, T0 arg0, T1 arg1)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Error)
            Logger.Error(format, arg0, arg1);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Error)
            Logger.Error(format, arg0, arg1, arg2);
    }
    
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info(string format, params object[] args)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Information)
            Logger.Info(format, args);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug(string format, params object[] args)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Debug)
            Logger.Debug(format, args);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning(string format, params object[] args)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Warning)
            Logger.Warning(format, args);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error(string format, params object[] args)
    {
        if(Logger.CurrentLogLevel >= LogLevel.Error)
            Logger.Error(format, args);
    }
    
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert(bool isOk ,string format, params object[] args)
    {
        if (isOk)
        {
            return;
        }
        Error(format, args);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert<T>(bool isOk ,string format, T arg)
    {
        if (isOk)
        {
            return;
        }
        Error(format, arg);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert<T0, T1>(bool isOk ,string format, T0 arg0, T1 arg1)
    {
        if (isOk)
        {
            return;
        }
        Error(format, arg0, arg1);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert<T0, T1, T2>(bool isOk ,string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (isOk)
        {
            return;
        }
        Error(format, arg0, arg1, arg2);
    }
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert(bool isOk ,string o)
    {
        if (isOk)
        {
            return;
        }
        Error(o);
    }
}