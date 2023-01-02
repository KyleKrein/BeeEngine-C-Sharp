using BeeEngine.Mathematics;
using BeeEngine.OpenTK;
using BeeEngine.OpenTK.Events;
using BeeEngine.OpenTK.Renderer;

namespace Test.Implementations;

public class TestApp: Application
{
    private OrthographicCamera _camera = new OrthographicCamera(-2, 2, -2, 2);
    
    public TestApp(string title, int width, int height) : base(new WindowProps(title, width, height, VSync.Off, false))
    {
        
    }

    protected override void Initialize()
    {
        PushLayer(new ExampleLayer());
    }
    

    protected override void LoadContent()
    {
        
    }

    protected override void UnloadContent()
    {
        
    }

    private float cameraSpeed = 0.1f;
    private float lastTime = Time.TotalTime;
    private float fps = 0;
    protected override void Update()
    {
        var cameraPosition = _camera.Position;
        if (Input.KeyPressed(Key.W))
        {
            cameraPosition.Y += cameraSpeed * Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.A))
        {
            cameraPosition.X -= cameraSpeed* Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.D))
        {
            cameraPosition.X += cameraSpeed* Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.S))
        {
            cameraPosition.Y -= cameraSpeed* Time.DeltaTime;
        }

        if (Time.TotalTime - lastTime >= 1)
        {
            Log.Info("{0} FPS", fps);
            lastTime = Time.TotalTime;
            fps = 0;
        }
        _camera.Position = cameraPosition;
        //Log.Info("DeltaTime: {0}s ({1}ms, FPS: {2}, Global Time: {3})", Time.DeltaTime, Time.DeltaTime*1000, fps, Time.TotalTime);
        //_camera.Rotation += 1f;
    }

    protected override void FixedUpdate()
    {
        
    }

    protected override void Render()
    {
        fps++;
        RenderCommand.Clear();
        Renderer.BeginScene(_camera);
        Renderer.EndScene();
        //_triangle.Unbind();
    }
}