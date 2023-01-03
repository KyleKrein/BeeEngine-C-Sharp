using BeeEngine.OpenTK;
using BeeEngine.OpenTK.Events;
using BeeEngine.OpenTK.Renderer;

namespace Test.Implementations;

public class TestApp: Application
{
    
    public TestApp(string title, int width, int height) : base(new WindowProps(title, width, height, VSync.On, false))
    {
        
    }

    

    protected override void Initialize()
    {
        //PushLayer(new ExampleLayer());
        PushLayer(new TestLayer2D());
    }
    

    protected override void LoadContent()
    {
        
    }

    protected override void UnloadContent()
    {
        
    }
    private float lastTime = Time.TotalTime;
    private float fps = 0;
    protected override void Update()
    {

        if (Time.TotalTime - lastTime >= 1)
        {
            Log.Info("{0} FPS", fps);
            lastTime = Time.TotalTime;
            fps = 0;
        }
        
        fps++;

        
    }
    
    protected override void OnEvent(ref EventDispatcher e)
    {
        
    }

    protected override void FixedUpdate()
    {
        
    }

    protected override void Render()
    {
        
    }
}