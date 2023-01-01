using BeeEngine.OpenTK;
using BeeEngine.OpenTK.Renderer;

namespace Test.Implementations;

public class TestApp: Application
{
    
    public TestApp(string title, int width, int height) : base(new WindowProps(title, width, height, VSync.On, false))
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
        RenderCommand.Clear();
        Renderer.BeginScene();
        Renderer.EndScene();
        //_triangle.Unbind();
    }
}