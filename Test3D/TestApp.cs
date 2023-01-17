using BeeEngine;
using BeeEngine.Events;

namespace Test3D;

public class TestApp: Application
{
    public TestApp(WindowProps initSettings) : base(initSettings)
    {
        
    }

    protected override void OnEvent(ref EventDispatcher e)
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

    protected override void Update()
    {
        
    }

    protected override void FixedUpdate()
    {
        
    }

    protected override void Render()
    {
        
    }
}