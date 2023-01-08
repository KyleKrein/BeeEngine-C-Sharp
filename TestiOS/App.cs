using BeeEngine;
using BeeEngine.Events;
using BeeEngine.Mathematics;

namespace TestiOS;

public class App: Application
{
    private OrthographicCameraController _cameraController;
    protected override void OnEvent(ref EventDispatcher e)
    {
        
    }

    protected override void Initialize()
    {
        _cameraController = new OrthographicCameraController();
        RenderCommand.SetClearColor(Color.CornflowerBlue);
    }

    protected override void LoadContent()
    {
        
    }

    protected override void UnloadContent()
    {
        
    }

    protected override void Update()
    {
        
    }

    protected override void FixedUpdate()
    {
        
    }

    protected override void Render()
    {
        RenderCommand.Clear();
        Renderer2D.BeginScene(_cameraController);
        
        Renderer2D.DrawRectangle(0,0, 0.5f, 0.5f, Color.Green);
        
        Renderer2D.EndScene();
    }
}