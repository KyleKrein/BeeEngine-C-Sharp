using System.Diagnostics;
using BeeEngine.OpenTK.Core;
using ImGuiNET;

namespace BeeEngine.OpenTK.Profiling;

public class DebugLayer: Layer
{
    private int _frameCounter = 0;
    public override void OnUpdate()
    {
        if (Instrumentor.IsProfilingInProgress)
        {
            _frameCounter++;
            if (_frameCounter >= _numberOfFramesToCapture)
            {
                _frameCounter = 0;
                Instrumentor.EndSession();
                //Process.Start("chrome://tracing/");
            }
        }
        /*if (Input.KeyPressed(Key.G))
        {
            if (Instrumentor.IsProfilingInProgress)
            {
                Instrumentor.EndSession();
            }
            else
            {
                Instrumentor.BeginSession("Runtime", "runtime_profiling.json");
            }
        }*/
    }

    private int _numberOfFramesToCapture = 10;
    public override unsafe void OnGUIRendering()
    {
        ImGui.Begin("Debug settings");
        ImGui.Text("Number of frames to capture");
        ImGui.InputInt("", ref _numberOfFramesToCapture);
        if (ImGui.Button("Start profiling"))
        {
            Instrumentor.BeginSession("Runtime", "runtime_profiling.json");
        }
        if (ImGui.Button("Capture 1 frame"))
        {
            _numberOfFramesToCapture = 1;
            Instrumentor.BeginSession("Runtime", "runtime_profiling.json");
        }
        if (ImGui.Button("Capture 10 frames"))
        {
            _numberOfFramesToCapture = 10;
            Instrumentor.BeginSession("Runtime", "runtime_profiling.json");
        }
        ImGui.End();
    }
}