using BeeEngine.Mathematics;
using BeeEngine.Events;
using BeeEngine;
using Cysharp.Text;
using ImGuiNET;
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
        Z = -0.1f,
        Width = 1.5f,
        Height = 1f
    };

    private Color[] colors = new Color[2]
    {
        Color.Cyan,
        Color.Violet
    };
    
    private float rotation = 0;
    public override void OnUpdate()
    { 
        //using var t = new Timer(); 
        Renderer2D.ResetStatistics();
        
        
        _cameraController.OnUpdate();
        Renderer2D.BeginScene(_cameraController);

        for (int i = 0; i < 24*5; i++)
        {
            for (int j = 0; j < 30*5; j++)
            {
                //Renderer2D.DrawRectangle(j, i, 1, 1, Color.Yellow);
                Renderer2D.DrawImage(j,i,0,1,1,_forest, Color.White, rotation*Time.DeltaTime, 1);
            }
        }
        
        /*
        Renderer2D.DrawRectangle(0,0,0.5f,0.5f, color);
        Renderer2D.DrawRectangle(-1,0,0.5f,0.5f, Color.Azure);
        Renderer2D.DrawRectangle(-0.5f,0,0.5f,0.5f, Color.Crimson);
        Renderer2D.DrawRectangle(0.5f,0.5f, 0,0.5f,0.5f, Color.Green, MathHelper.DegreesToRadians(45));
        Renderer2D.DrawImage(ref _forestImageProperties, _forest, 1);
        */
        
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