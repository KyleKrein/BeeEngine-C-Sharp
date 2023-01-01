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
        BufferLayout layout = new()
        {
            {ShaderDataType.Float3, "Position"}
        };
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