using BeeEngine.Mathematics;
using BeeEngine.OpenTK;
using BeeEngine.OpenTK.Events;
using BeeEngine.OpenTK.Renderer;

namespace Test.Implementations;

public class TestApp: Application
{
    private OrthographicCamera _camera = new OrthographicCamera(-1.6f, 1.6f, -0.9f, 0.9f);
    
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

    private float cameraSpeed = 2f;
    private float rotationSpeed = 180f;
    private float lastTime = Time.TotalTime;
    private float fps = 0;
    private Vector3 _cameraPosition = Vector3.Zero;
    private float _cameraAngle = 0;
    protected override void Update()
    {
        if (Input.KeyPressed(Key.W))
        {
            _cameraPosition.Y += cameraSpeed * Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.A))
        {
            _cameraPosition.X -= cameraSpeed* Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.D))
        {
            _cameraPosition.X += cameraSpeed* Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.S))
        {
            _cameraPosition.Y -= cameraSpeed* Time.DeltaTime;
        }

        if (Input.KeyPressed(Key.Q))
        {
            _cameraAngle += rotationSpeed * Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.E))
        {
            _cameraAngle -= rotationSpeed* Time.DeltaTime;
        }

        if (Time.TotalTime - lastTime >= 1)
        {
            Log.Info("{0} FPS", fps);
            lastTime = Time.TotalTime;
            fps = 0;
        }
        RenderCommand.Clear();
        _camera.Position = _cameraPosition;
        _camera.Rotation = _cameraAngle;
        fps++;

        //_camera.Position = new Vector3(0.5f, 0.5f, 0.0f);
        //_camera.Rotation = 45;
        Renderer.BeginScene(_camera);
        
        //Log.Info("DeltaTime: {0}s ({1}ms, FPS: {2}, Global Time: {3})", Time.DeltaTime, Time.DeltaTime*1000, fps, Time.TotalTime);
        //_camera.Rotation += 1f;
    }

    protected override void FixedUpdate()
    {
        
    }

    protected override void Render()
    {
        Renderer.EndScene();
        //_triangle.Unbind();
    }
}