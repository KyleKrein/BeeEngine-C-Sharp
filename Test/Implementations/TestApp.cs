using BeeEngine.OpenTK;
using BeeEngine.OpenTK.Renderer;

namespace Test.Implementations;

public class TestApp: Application
{
    private VertexArray _triangle;
    private Shader _triangleShader;
    public TestApp(string title, int width, int height) : base(new WindowProps(title, width, height, VSync.On, false))
    {
        float[] vertices = new[]
        {
            0.5f, 0.0f, 0.0f,
            -1.0f, 0.0f, 0.0f,
            0.0f, 0.5f, 0.0f
        };
        uint[] indices = new uint[]
        {
            1 ,2 ,3
        };

        var layout = new BufferLayout()
        {
            {ShaderDataType.Float3, "a_Position"}
        };
        var vertexBuffer = VertexBuffer.Create(vertices, 0);
        vertexBuffer.Layout = layout;
        vertexBuffer.Bind();
        var indexBuffer = IndexBuffer.Create(indices);
        indexBuffer.Bind();
        _triangle = VertexArray.Create();
        _triangle.AddVertexBuffer(vertexBuffer);
        _triangle.SetIndexBuffer(indexBuffer);
        const string vertexSrc = @"
        #version 330 core
        layout (location = 0) in vec3 a_Position;
            out vec3 _Position;
        void main()
        {
        _Position - a_Position;
        gl_Position = vec4(a_Position, 1.0);
        }
            ";
        const string fragmentSrc = @"#version 330 core
layout (location = 0) out vec4 color;
in vec3 a_Position;
void main(){
color = vec4(1.0, 0.0, 0.0, 1.0);
}
";
        _triangleShader = Shader.Create(vertexSrc, fragmentSrc);
        _triangle.Bind();
    }

    protected override void Initialize()
    {
        
    }

    protected override void LoadContent()
    {
        
    }

    protected override void UnloadContent()
    {
        _triangleShader.Dispose();
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
        _triangleShader.Bind();
        Renderer.Submit(_triangle);
        Renderer.EndScene();
    }
}