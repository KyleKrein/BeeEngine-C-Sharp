using OpenTK.Windowing.GraphicsLibraryFramework;

namespace BeeEngine.OpenTK;

public static class Time
{
    /*public TimeSpan TotalTime { get; internal set; } = TimeSpan.Zero;
    public TimeSpan ElapsedTime { get; internal set; } = TimeSpan.Zero;*/
    private static float _globalTime = 0.0f;
    public static float DeltaTime { get; private set; } = 1f / 60f;

    internal static void Update()
    {
        float currentTime = (float) GLFW.GetTime();
        DeltaTime = currentTime - _globalTime;
        _globalTime = currentTime;
    }
}