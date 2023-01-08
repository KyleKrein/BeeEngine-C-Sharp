using BeeEngine;
using BeeEngine.Events;
using BeeEngine.Mathematics;

namespace TestiOS;

public class App: Application
{
    private OrthographicCamera _cameraController;
    protected override void OnEvent(ref EventDispatcher e)
    {
        
    }

    protected override void Initialize()
    {
        //_cameraController = new OrthographicCameraController();
        //RenderCommand.SetClearColor(Color.CornflowerBlue);
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

    private float rotation = 0;
    protected override void Render()
    {
        if (_cameraController is null)
        {
            _cameraController = new OrthographicCamera(0.9f,0.9f,1.8f,1.8f);
        }
        RenderCommand.Clear();
        Renderer2D.BeginScene(_cameraController);
        
        Renderer2D.DrawRectangle(-0.5f,0f,0.1f, 0.5f, 0.5f, Color.Green, 0);
        Renderer2D.DrawRectangle(0.5f,0.2f, 0f, 0.5f, 0.2f, Color.GreenYellow, MathU.DegreesToRadians(rotation++));
        
        Renderer2D.EndScene();
    }
}