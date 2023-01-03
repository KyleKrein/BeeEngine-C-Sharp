using System.Runtime.CompilerServices;
using BeeEngine;
using BeeEngine.Mathematics;
using BeeEngine.OpenTK;
using BeeEngine.OpenTK.Core;
using BeeEngine.OpenTK.Events;
using BeeEngine.OpenTK.Platform.OpenGL;
using BeeEngine.OpenTK.Renderer;
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

    public override void OnUpdate()
    {
        //using var t = new Timer(); 
        
        
        _cameraController.OnUpdate();
        Renderer2D.BeginScene(_cameraController);
        
        
        Renderer2D.DrawImage(-0.5f, 0.2f, 1,1, _forest);
        Renderer2D.DrawRectangle(0,0,0.5f,0.5f, color);
        Renderer2D.DrawImage(0f, 0f, -0.1f, 10f,10f, _forest, color);
    }
    public override void OnGUIRendering()
    {
        ImGui.Begin("Rectangle");
        ImGui.ColorEdit4("", ref color);
        ImGui.End();
    }

    private Vector4 color = Color.Aquamarine;
}