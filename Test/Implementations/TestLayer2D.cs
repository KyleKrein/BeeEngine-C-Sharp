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

    private RectangleProperties _forestImageProperties = new RectangleProperties()
    {
        X = -0.5f,
        Y = 0.2f,
        Rotation = 35,
        Z = -0.1f
    };

    private Color[] colors = new Color[2]
    {
        Color.Cyan,
        Color.Violet
    };
    public override void OnUpdate()
    {
        //using var t = new Timer(); 
        
        
        _cameraController.OnUpdate();
        Renderer2D.BeginScene(_cameraController);

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                Renderer2D.DrawRectangle(j, i, 1, 1, colors[j%2]);
            }
        }
        
        /*Renderer2D.DrawRectangle(0,0,0.5f,0.5f, color);
        Renderer2D.DrawRectangle(-1,0,0.5f,0.5f, Color.Azure);
        Renderer2D.DrawRectangle(-0.5f,0,0.5f,0.5f, Color.Crimson);
        Renderer2D.DrawRectangle(0.5f,0,0.5f,0.5f, Color.Green);*/
        //Renderer2D.DrawImage(ref _forestImageProperties, _forest);

        Renderer2D.EndScene();
    }
    public override void OnGUIRendering()
    {
        ImGui.Begin("Rectangle");
        ImGui.ColorEdit4("", ref color);
        ImGui.End();
    }

    private Vector4 color = Color.Aquamarine;
}