using System.Runtime.CompilerServices;
using BeeEngine;
using BeeEngine.Mathematics;
using BeeEngine.OpenTK;
using BeeEngine.OpenTK.Core;
using BeeEngine.OpenTK.Events;
using BeeEngine.OpenTK.Platform.OpenGL;
using BeeEngine.OpenTK.Renderer;
using Cysharp.Text;
using ImGuiNET;
using Timer = BeeEngine.OpenTK.Timer;
using Vector4 = System.Numerics.Vector4;

namespace Test.Implementations;

public class TestLayer2D: Layer
{
    private OrthographicCameraController _cameraController;
    private Texture2D _forest;
    public override void OnAttach()
    {
        _cameraController = new OrthographicCameraController(true);
        _forest = Texture2D.CreateFromFile(@"Assets\Textures\forest.png");
    }

    public override void OnDetach()
    {
        
    }

    public override void OnEvent(ref EventDispatcher e)
    {
        _cameraController.OnEvent(ref e);
    }

    private RectangleProperties _forestImageProperties = new RectangleProperties()
    {
        X = 1f,
        Y = 0.2f,
        Rotation = 0,
        Z = -0.1f
    };

    private Color[] colors = new Color[2]
    {
        Color.Cyan,
        Color.Violet
    };
    private float lastTime = Time.TotalTime;
    private float fps = 0;
    private float currentFps = 0;
    public override void OnUpdate()
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
        
        //using var t = new Timer(); 
        Renderer2D.ResetStatistics();
        
        
        _cameraController.OnUpdate();
        Renderer2D.BeginScene(_cameraController);

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 1000/4; j++)
            {
                //Renderer2D.DrawRectangle(j, i, 1, 1, Color.Yellow);
                Renderer2D.DrawImage(j,i,1,1,_forest);
            }
        }
        
        /*Renderer2D.DrawRectangle(0,0,0.5f,0.5f, color);
        Renderer2D.DrawRectangle(-1,0,0.5f,0.5f, Color.Azure);
        Renderer2D.DrawRectangle(-0.5f,0,0.5f,0.5f, Color.Crimson);
        Renderer2D.DrawRectangle(0.5f,0.5f, 0,0.5f,0.5f, Color.Green, MathHelper.DegreesToRadians(45));
        Renderer2D.DrawImage(ref _forestImageProperties, _forest, 10);
        */

        Renderer2D.EndScene();
    }
    public override void OnGUIRendering()
    {
        ImGui.Begin("Rectangle");
        ImGui.ColorEdit4("", ref color);
        ImGui.End();

        ImGui.Begin("Performance");
        ImGui.Text(ZString.Format("Fps: {0}", currentFps));
        ImGui.Text(ZString.Format("LastFrame: {0} ms", Time.DeltaTime/1000));
        ImGui.End();
    }

    private Vector4 color = Color.Aquamarine;
}