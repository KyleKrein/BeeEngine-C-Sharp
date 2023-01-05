using System.Diagnostics;
using System.Globalization;

namespace BeeEngine.OpenTK.Core;

public static class Instrumentor
{
    private static string? _currentSession;
    private static StreamWriter? _streamWriter;
    private static int _profileCount;
    public static bool IsProfilingInProgress => _currentSession != null;
    static Instrumentor()
    {
        _currentSession = null;
        _streamWriter = null;
        _profileCount = 0;
    }
    [Conditional("DEBUG")]
    public static void BeginSession(string name, string filepath)
    {
        _streamWriter = new StreamWriter(filepath);
        WriteHeader();
        _currentSession = name;
    }
    [Conditional("DEBUG")]
    public static void EndSession()
    {
        WriteFooter();
        _streamWriter.Close();
        _currentSession = null;
        _profileCount = 0;
    }
     internal static void WriteProfile(in ProfileResult result)
    {
        if (_profileCount++ > 0)
            _streamWriter.Write(",");

        string name = result.Name;
        name = name.Replace('"', '\'');

        _streamWriter.Write("{");
        _streamWriter.Write("\"cat\":\"function\",");
        _streamWriter.Write("\"dur\":");
        _streamWriter.Write((result.End - result.Start).ToString(CultureInfo.InvariantCulture));
        _streamWriter.Write(',');
        _streamWriter.Write("\"name\":\"");
        _streamWriter.Write(name);
        _streamWriter.Write("\",");
        _streamWriter.Write("\"ph\":\"X\",");
        _streamWriter.Write("\"pid\":0,");
        _streamWriter.Write("\"tid\":");
        _streamWriter.Write(result.ThreadID);
        _streamWriter.Write(",");
        _streamWriter.Write("\"ts\":");
        _streamWriter.Write(result.Start.ToString(CultureInfo.InvariantCulture));
        _streamWriter.Write("}");

        _streamWriter.Flush();
    }


    private static void WriteHeader()
    {
        _streamWriter.Write("{\"otherData\": {},\"traceEvents\":[");
        _streamWriter.Flush();
    }
    private static void WriteFooter()
    {
        _streamWriter.Write("]}");
        _streamWriter.Flush();
    }
}

struct ProfileResult
{
    public string Name;
    public double Start, End;
    public int ThreadID;

    public ProfileResult(string name, double start, double end, int threadId)
    {
        Name = name;
        Start = start;
        ThreadID = threadId;
        End = end;
    }
}
