using BeeEngine.Mathematics;
using Cysharp.Text;
using ImGuiNET;

namespace BeeEngine.Gui;

public struct FpsCounter
{
    private float lastTime = Time.TotalTime;
    private float fps = 0;
    private float currentFps = 0;

    public FpsCounter()
    {
    }

    public void Update()
    {
        var now = Time.TotalTime;
        if (now - lastTime >= 1)
        {
            Log.Info("{0} FPS", fps);
            currentFps = fps;
            lastTime = now;
            fps = 0;
        }
        
        fps++;
    }

    public void Render()
    {
        ImGui.Begin("Performance");
        ImGui.Text(ZString.Format("Fps: {0}", currentFps));
        ImGui.Text(ZString.Format("LastFrame: {0} ms", MathU.Round(Time.DeltaTime * 1000f, 3)));
        ImGui.End();
    }
}