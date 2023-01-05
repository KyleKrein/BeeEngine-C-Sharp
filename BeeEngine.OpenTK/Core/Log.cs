using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Cysharp.Text;

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
        if (Logger.CurrentLogLevel >= LogLevel.Information)
            Logger.Info(o);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug(string o)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Debug)
            Logger.Debug(o);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning(string o)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Warning)
            Logger.Warning(o);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error(string o)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Error)
            Logger.Error(o);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info<T>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T arg)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Information)
        {
            var o = ZString.Format(format, arg);
            Logger.Info(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Information)
        {
            var o = ZString.Format(format, arg0, arg1);
            Logger.Info(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Information)
        {
            var o = ZString.Format(format, arg0, arg1, arg2);
            Logger.Info(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug<T>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T arg)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Debug)
        {
            var o = ZString.Format(format, arg);
            Logger.Debug(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Debug)
        {
            var o = ZString.Format(format, arg0, arg1);
            Logger.Debug(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Debug)
        {
            var o = ZString.Format(format, arg0, arg1, arg2);
            Logger.Debug(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning<T>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T arg)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Warning)
        {
            var o = ZString.Format(format, arg);
            Logger.Warning(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Warning)
        {
            var o = ZString.Format(format, arg0, arg1);
            Logger.Warning(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Warning)
        {
            var o = ZString.Format(format, arg0, arg1, arg2);
            Logger.Warning(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error<T>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T arg)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Error)
        {
            var o = ZString.Format(format, arg);
            Logger.Error(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Error)
        {
            var o = ZString.Format(format, arg0, arg1);
            Logger.Error(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Error)
        {
            var o = ZString.Format(format, arg0, arg1, arg2);
            Logger.Error(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, params object[] args)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Information)
        {
            var o = string.Format(format, args);
            Logger.Info(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, params object[] args)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Debug)
        {
            var o = string.Format(format, args);
            Logger.Debug(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, params object[] args)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Warning)
        {
            var o = string.Format(format, args);
            Logger.Warning(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, params object[] args)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Information)
        {
            var o = string.Format(format, args);
            Logger.Error(o);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, params object[] args)
    {
        if (isOk)
        {
            return;
        }

        Error(format, args);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert<T>(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T arg)
    {
        if (isOk)
        {
            return;
        }

        Error(format, arg);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert<T0, T1>(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1)
    {
        if (isOk)
        {
            return;
        }

        Error(format, arg0, arg1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert<T0, T1, T2>(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (isOk)
        {
            return;
        }

        Error(format, arg0, arg1, arg2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert(bool isOk, string o)
    {
        if (isOk)
        {
            return;
        }

        Error(o);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertAndThrow(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, params object[] args)
    {
        if (isOk)
        {
            return;
        }

        var final = string.Format(format, args);
        Error(final);
        throw new Exception(final);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertAndThrow<T>(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T arg)
    {
        if (isOk)
        {
            return;
        }

        var final = ZString.Format(format, arg);
        Error(final);
        throw new Exception(final);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertAndThrow<T0, T1>(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1)
    {
        if (isOk)
        {
            return;
        }

        var final = ZString.Format(format, arg0, arg1);
        Error(final);
        throw new Exception(final);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertAndThrow<T0, T1, T2>(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (isOk)
        {
            return;
        }

        var final = ZString.Format(format, arg0, arg1, arg2);
        Error(final);
        throw new Exception(final);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertAndThrow(bool isOk, string o)
    {
        if (isOk)
        {
            return;
        }

        Error(o);
        throw new Exception(o);
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
        if (Logger.CurrentLogLevel >= LogLevel.Information)
            Logger.Info(o);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug(string o)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Debug)
            Logger.Debug(o);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning(string o)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Warning)
            Logger.Warning(o);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error(string o)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Error)
            Logger.Error(o);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info<T>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T arg)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Information)
        {
            var o = ZString.Format(format, arg);
            Logger.Info(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Information)
        {
            var o = ZString.Format(format, arg0, arg1);
            Logger.Info(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Information)
        {
            var o = ZString.Format(format, arg0, arg1, arg2);
            Logger.Info(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug<T>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T arg)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Debug)
        {
            var o = ZString.Format(format, arg);
            Logger.Debug(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Debug)
        {
            var o = ZString.Format(format, arg0, arg1);
            Logger.Debug(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Debug)
        {
            var o = ZString.Format(format, arg0, arg1, arg2);
            Logger.Debug(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning<T>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T arg)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Warning)
        {
            var o = ZString.Format(format, arg);
            Logger.Warning(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Warning)
        {
            var o = ZString.Format(format, arg0, arg1);
            Logger.Warning(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Warning)
        {
            var o = ZString.Format(format, arg0, arg1, arg2);
            Logger.Warning(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error<T>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T arg)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Error)
        {
            var o = ZString.Format(format, arg);
            Logger.Error(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error<T0, T1>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Error)
        {
            var o = ZString.Format(format, arg0, arg1);
            Logger.Error(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error<T0, T1, T2>([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Error)
        {
            var o = ZString.Format(format, arg0, arg1, arg2);
            Logger.Error(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, params object[] args)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Information)
        {
            var o = string.Format(format, args);
            Logger.Info(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, params object[] args)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Debug)
        {
            var o = string.Format(format, args);
            Logger.Debug(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, params object[] args)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Warning)
        {
            var o = string.Format(format, args);
            Logger.Warning(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error([StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, params object[] args)
    {
        if (Logger.CurrentLogLevel >= LogLevel.Information)
        {
            var o = string.Format(format, args);
            Logger.Error(o);
        }
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, params object[] args)
    {
        if (isOk)
        {
            return;
        }

        Error(format, args);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert<T>(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T arg)
    {
        if (isOk)
        {
            return;
        }

        Error(format, arg);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert<T0, T1>(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1)
    {
        if (isOk)
        {
            return;
        }

        Error(format, arg0, arg1);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert<T0, T1, T2>(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (isOk)
        {
            return;
        }

        Error(format, arg0, arg1, arg2);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert(bool isOk, string o)
    {
        if (isOk)
        {
            return;
        }

        Error(o);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertAndThrow(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, params object[] args)
    {
        if (isOk)
        {
            return;
        }

        var final = string.Format(format, args);
        Error(final);
        throw new Exception(final);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertAndThrow<T>(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T arg)
    {
        if (isOk)
        {
            return;
        }

        var final = ZString.Format(format, arg);
        Error(final);
        throw new Exception(final);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertAndThrow<T0, T1>(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1)
    {
        if (isOk)
        {
            return;
        }

        var final = ZString.Format(format, arg0, arg1);
        Error(final);
        throw new Exception(final);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertAndThrow<T0, T1, T2>(bool isOk, [StringSyntax(StringSyntaxAttribute.CompositeFormat)]string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (isOk)
        {
            return;
        }

        var final = ZString.Format(format, arg0, arg1, arg2);
        Error(final);
        throw new Exception(final);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertAndThrow(bool isOk, string o)
    {
        if (isOk)
        {
            return;
        }

        Error(o);
        throw new Exception(o);
    }
}