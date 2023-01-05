using System.Diagnostics;
using System.Runtime.CompilerServices;
using BeeEngine.OpenTK.Core;
using Cysharp.Text;

namespace BeeEngine.OpenTK;

public ref struct Timer
{
#if DEBUG
    private readonly string _name;
    private DateTime _start;
    public Action<string, TimeSpan> Func;
    public Timer([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
    {
        _start = DateTime.Now;
        _name = memberName;
    }

    public void Stop()
    {
        TimeSpan result = DateTime.Now - _start;
        Func(_name, result);
    }
#endif
    
    public void Dispose()
    {
#if DEBUG
      Stop();  
#endif
    }
}

public static class DebugTimer
{
    private static readonly DateTime InvalidDateTime = new DateTime(2000, 12, 25);
    private static Dictionary<string, DateTime> _methods = new Dictionary<string, DateTime>();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetName(string memberName, string memberPath)
    {
        return ZString.Format("{0}: {1}", ResourceManager.GetNameFromPath(memberPath), memberName);
    }
    [Conditional("DEBUG")]
    public static void Start([System.Runtime.CompilerServices.CallerMemberName] string memberName = "", [System.Runtime.CompilerServices.CallerFilePath] string memberPath = "")
    {
        if (!Instrumentor.IsProfilingInProgress)
        {
            return;
        }
        string name = GetName(memberName, memberPath);
        if (!_methods.ContainsKey(name))
        {
            _methods.Add(name, DateTime.Now);
            return;
        }
        _methods[name] = DateTime.Now;
    }
    [Conditional("DEBUG")]
    public static void End([System.Runtime.CompilerServices.CallerMemberName] string memberName = "", [System.Runtime.CompilerServices.CallerFilePath] string memberPath = "")
    {
        if (!Instrumentor.IsProfilingInProgress)
        {
            return;
        }
        var name = GetName(memberName, memberPath);
        double start = _methods[name].TimeOfDay.TotalMicroseconds * 0.001f;
        double end = DateTime.Now.TimeOfDay.TotalMicroseconds * 0.001f;
        DebugLog.Assert(_methods[name] != InvalidDateTime, "Invalid time of start in {0}", name);
        _methods[name] = InvalidDateTime;
        DebugTimer.Log(name, start, end);
    }

    private static void Log(string memberName, double start, double end)
    {
        Instrumentor.WriteProfile(new ProfileResult(memberName, start, end, Thread.CurrentThread.ManagedThreadId));
    }
}
