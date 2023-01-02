using BeeEngine.Mathematics;
using BeeEngine.OpenTK;
using BeeEngine.OpenTK.Renderer;
using OpenTK.Graphics.OpenGL4;

namespace Test.Implementations;

public class ExampleLayer: Layer
{
    private VertexArray _triangle;
    private Shader _triangleShader;

    
    public override void OnAttach()
    {
        RenderCommand.SetClearColor(Color.Black);
        _triangle = VertexArray.Create();
        float[] vertices = {
            -0.5f, -0.5f, 0.0f, 0.8f, 0.2f, 0.8f, 1.0f,
            0.5f, -0.5f, 0.0f, 0.2f, 0.3f, 0.8f, 1.0f,
            0.0f,  0.5f, 0.0f, 0.8f, 0.8f, 0.2f, 1.0f
        };
        
        var vertexBuffer = VertexBuffer.Create(vertices, 0);
        var layout = new BufferLayout()
        {
            {ShaderDataType.Float3, "a_Position"},
            {ShaderDataType.Float4, "a_Color"}
        };
        
        vertexBuffer.Layout = layout;
        
        _triangle.AddVertexBuffer(vertexBuffer);
        
        uint[] indices = new uint[]
        {
            0 ,1 ,2
        };
        var indexBuffer = IndexBuffer.Create(indices);
        
        _triangle.SetIndexBuffer(indexBuffer);
        const string vertexSrc = @"
        #version 330 core
			
			layout(location = 0) in vec3 a_Position;
			layout(location = 1) in vec4 a_Color;

			uniform mat4 u_ViewProjection;
			uniform mat4 u_Transform;

			out vec3 v_Position;
			out vec4 v_Color;
			void main()
			{
				v_Position = a_Position;
				v_Color = a_Color;
				gl_Position =  u_ViewProjection * u_Transform * vec4(a_Position, 1.0);	
			}
            ";
        const string fragmentSrc = @"
#version 330 core
			
			layout(location = 0) out vec4 color;
			in vec3 v_Position;
			in vec4 v_Color;
			void main()
			{
				color = vec4(v_Position * 0.5 + 0.5, 1.0);
				color = v_Color;
			}
";
        _triangleShader = Shader.Create(vertexSrc, fragmentSrc);
    }

    public override void OnUpdate()
    {
        Renderer.Submit(_triangle, _triangleShader, Matrix4.CreateTranslation(0,0,0));
    }

    public override void OnDetach()
    {
        _triangleShader.Dispose();
        _triangle.Dispose();
    }
}
