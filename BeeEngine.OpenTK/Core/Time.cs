using OpenTK.Windowing.GraphicsLibraryFramework;

namespace BeeEngine.OpenTK;

public static class Time
{
    /*public TimeSpan TotalTime { get; internal set; } = TimeSpan.Zero;
    public TimeSpan ElapsedTime { get; internal set; } = TimeSpan.Zero;*/
    private static float _globalTime = 0.0f;
    public static float TotalTime => _globalTime;
    public static float DeltaTime { get; private set; } = 1f / 60f;

    internal static void Update()
    {
        float currentTime = (float) GLFW.GetTime();
        DeltaTime = currentTime - _globalTime;
        _globalTime = currentTime;
    }
}

public ref struct TimeStep
{
    private readonly float _time;

    public TimeStep(float seconds)
    {
        _time = seconds;
    }

    public float Seconds => _time;
    public float Milliseconds => _time * 1000;
}