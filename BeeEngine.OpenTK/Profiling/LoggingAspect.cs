/*using System.Reflection;
using System.Runtime.CompilerServices;
using AspectInjector;
using AspectInjector.Broker;
using System;
using System.Linq;
using Cysharp.Text;

namespace BeeEngine.OpenTK.Profiling;
[Aspect(Scope.Global)]
[Injection(typeof(Logging))]
public class Logging: Attribute
{

    [Advice(Kind.After)]
    public void After([Argument(Source.Name)] string name, [Argument(Source.ReturnValue)] object retValue, [Argument(Source.Type)] Type hostType)
    {
        //Don't forget to remove unused parameters as it improves performance!
        //Alt+Enter or Ctrl+. on Method name or Advice attribute to add more Arguments

        Log.Info($"Finished {name} from {hostType.Name} with result: {retValue}");
    }

    [Advice(Kind.Before)]
    public void Before([Argument(Source.Name)] string name, [Argument(Source.Arguments)] object[] args, [Argument(Source.Type)] Type hostType)
    {
        //Don't forget to remove unused parameters as it improves performance!
        //Alt+Enter or Ctrl+. on Method name or Advice attribute to add more Arguments

        Log.Info($"Calling {name} from {hostType.Name} with args: {ZString.Join(',', args.Select(a => ToString()))}");
    }
}*/