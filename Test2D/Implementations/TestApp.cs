using BeeEngine;
using BeeEngine.Events;

namespace Test.Implementations;

public class TestApp: Application
{
    
    public TestApp(string title, int width, int height) : base(new WindowProps(title, width, height, VSync.Off, false))
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
    
    protected override void Update()
    {

        

        
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