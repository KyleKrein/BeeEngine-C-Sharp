#if PROFILING
using AspectInjector.Broker;
#endif
using System.Linq;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BeeEngine.Core;
using Cysharp.Text;

namespace BeeEngine.OpenTK.Profiling;
#if PROFILING
[Conditional("PROFILING")]
[Aspect(Scope.Global)]
[Injection(typeof(ProfileMethod))]
public class ProfileMethod: Attribute
{
    
    [AspectInjector.Broker.Advice(Kind.Before)]
    public void Before([Argument(Source.Name)] string name, [Argument(Source.Type)] Type hostType)
    {
        //Don't forget to remove unused parameters as it improves performance!
        //Alt+Enter or Ctrl+. on Method name or Advice attribute to add more Arguments

        //Log.Info("Calling {0} from {1}", name, hostType.Name);
        Start(name, hostType.Name);
    }

    [AspectInjector.Broker.Advice(Kind.After)]
    public void After([Argument(Source.Name)] string name, [Argument(Source.ReturnValue)] object retValue, [Argument(Source.Type)] Type hostType)
    {
        //Don't forget to remove unused parameters as it improves performance!
        //Alt+Enter or Ctrl+. on Method name or Advice attribute to add more Arguments
        End(name, hostType.Name);
        //Console.WriteLine($"Finished {name} from {hostType.Name} with result: {retValue}");
    }
    private readonly DateTime InvalidDateTime = new DateTime(2000, 12, 25);
    private Dictionary<string, DateTime> _methods = new Dictionary<string, DateTime>();
    
    [Conditional("DEBUG")]
    public void Start( string memberName = "",  string memberType = "")
    {
        if (!Instrumentor.IsProfilingInProgress)
        {
            return;
        }
        string name = ZString.Concat(memberType, ':', memberName);
        if (!_methods.ContainsKey(name))
        {
            _methods.Add(name, DateTime.Now);
            return;
        }
        _methods[name] = DateTime.Now;
    }
    [Conditional("DEBUG")]
    public void End(string memberName = "", string memberType = "")
    {
        if (!Instrumentor.IsProfilingInProgress)
        {
            return;
        }
        
        var name = ZString.Concat(memberType, ':', memberName);
        if (!_methods.ContainsKey(name))
        {
            _methods.Add(name, InvalidDateTime);            
            return;
        }
        double start = _methods[name].TimeOfDay.TotalMicroseconds * 0.001f;
        double end = DateTime.Now.TimeOfDay.TotalMicroseconds * 0.001f;
        DebugLog.Assert(_methods[name] != InvalidDateTime, "Invalid time of start in {0}", name);
        _methods[name] = InvalidDateTime;
        LogToFile(name, start, end);
    }

    private void LogToFile(string memberName, double start, double end)
    {
        Instrumentor.WriteProfile(new ProfileResult(memberName, start, end, Thread.CurrentThread.ManagedThreadId));
    }
}
#else
[Conditional("PROFILING")]
public class ProfileMethod: Attribute
{
}
#endif