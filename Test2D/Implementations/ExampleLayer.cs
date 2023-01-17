using BeeEngine.Mathematics;
using BeeEngine.Events;
using BeeEngine;
using ImGuiNET;
using Vector4 = System.Numerics.Vector4;

namespace Test.Implementations;

public class ExampleLayer: Layer
{
    private VertexArray _triangle;
    private VertexArray _square;

    private Shader _flatColorShader;
    private Shader _triangleShader;
    private Shader _textureShader;

    private VertexArray _cube;
    private Matrix4 _cubeTransform = Matrix4.Identity;

    private Texture2D _forestTexture;
    private Texture2D _archerTexture;

    private OrthographicCameraController _cameraController;
    
    public override void OnAttach()
    {
	    _textureShader = LoadShader(@"Assets\Shaders\Texture.glsl");
	    _forestTexture = Texture2D.CreateFromFile(@"Assets\Textures\forest.png");
	    _archerTexture = Texture2D.CreateFromFile(@"Assets\Textures\archer_red.png");
	    
	    _textureShader.Bind();
	    _textureShader.UploadUniformInt("u_Texture", 0);
	    
	    float[] squareVertices =
	    {
			-0.5f, -0.5f, 0.0f, 0.0f, 0.0f, //bottom left
			 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, //bottom right
			 0.5f,  0.5f, 0.0f, 1.0f, 1.0f,// top right
			-0.5f,  0.5f, 0.0f, 0.0f, 1.0f,// top left
	    };
	    uint[] squareIndices =
	    {
		    0, 1, 2, 2, 3, 0
	    };

	    _square = VertexArray.Create();
	    var squareVertexBuffer = VertexBuffer.Create(squareVertices);
	    var squareIndexBuffer = IndexBuffer.Create(squareIndices);
	    BufferLayout squareLayout = new BufferLayout()
	    {
		    {ShaderDataType.Float3, "a_Position"},
		    {ShaderDataType.Float2, "a_TexCoord"}
	    };
	    squareVertexBuffer.Layout = squareLayout;
	    _square.AddVertexBuffer(squareVertexBuffer);
	    _square.SetIndexBuffer(squareIndexBuffer);
	    
	    _flatColorShader = LoadShader(@"Assets\Shaders\FlatColor.glsl");
	    
	    float[] cubeVertices =
	    {
		    -0.5f, -0.5f,  0.5f, 0.8f, 0.2f, 0.8f, 0.8f, //0
		    0.5f, -0.5f,  0.5f,  0.2f, 0.3f, 0.8f, 1.0f,//1
		    -0.5f,  0.5f,  0.5f, 0.8f, 0.8f, 0.2f, 1.0f, //2
		    0.5f,  0.5f,  0.5f, 0.66f, 0.25f, 0.66f, 1.0f,//3
		    -0.5f, -0.5f, -0.5f, 0.25f, 0.6f, 0.6f, 1.0f, //4
		    0.5f, -0.5f, -0.5f, 0.7f, 0.3f, 0.6f, 1.0f,//5
		    -0.5f,  0.5f, -0.5f, 0.8f, 0.6f, 0.6f, 1.0f,//6
		    0.5f,  0.5f, -0.5f, 0.2f, 0.2f, 0.6f, 1.0f,  //7
	    };

	    uint[] cubeIndices =
	    {
		    //Top
		    2, 6, 7,
		    2, 3, 7,

		    //Bottom
		    0, 4, 5,
		    0, 1, 5,

		    //Left
		    0, 2, 6,
		    0, 4, 6,

		    //Right
		    1, 3, 7,
		    1, 5, 7,

		    //Front
		    0, 2, 3,
		    0, 1, 3,

		    //Back
		    4, 6, 7,
		    4, 5, 7
	    };
	    ///////
        RenderCommand.SetClearColor(Color.Black);
        _triangle = VertexArray.Create();
        float[] vertices = {
            -0.5f, -0.5f, 0.0f, 0.8f, 0.2f, 0.8f, 1.0f,
            0.5f, -0.5f, 0.0f, 0.2f, 0.3f, 0.8f, 1.0f,
            0.0f,  0.5f, 0.0f, 0.8f, 0.8f, 0.2f, 1.0f
        };
        
        var vertexBuffer = VertexBuffer.Create(vertices);
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
        _triangleShader = Shader.Create("Triangle shader",vertexSrc, fragmentSrc);
        AddShader(_triangleShader);
        /////
        /*var cubeVertexBuffer = VertexBuffer.Create(cubeVertices, 0);
        cubeVertexBuffer.Layout = layout;
        var cubeIndexBuffer = IndexBuffer.Create(cubeIndices);*/
        //_cube = VertexArray.Create();
        //_cube.AddVertexBuffer(cubeVertexBuffer);
        //_cube.SetIndexBuffer(cubeIndexBuffer);
        _cameraController = new OrthographicCameraController(true);
    }

    public override void OnUpdate()
    {
	    RenderCommand.Clear();
	    Renderer.BeginScene(_cameraController);
		_cameraController.OnUpdate();   
	    //_cubeTransform = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45)) *Matrix4.CreateRotationY(MathHelper.DegreesToRadians(45));
        
       // _cubeTransform = _cubeTransform * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(100) * Time.DeltaTime) *
         //                Matrix4.CreateRotationY(MathHelper.DegreesToRadians(100) * Time.DeltaTime);
        //Renderer.Submit(_cube, _triangleShader, _cubeTransform);
        Renderer.Submit(_square, _flatColorShader, _cubeTransform, shader => shader.UploadUniformFloat4("u_Color", new BeeEngine.Mathematics.Vector4(color.X, color.Y, color.Z, color.W)));
        //_forestTexture.Bind();
        //Renderer.Submit(_square, _textureShader, _cubeTransform);
        _archerTexture.Bind();
        Renderer.Submit(_square, _textureShader, _cubeTransform * Matrix4.CreateScale(.5f));
        //Renderer.Submit(_triangle, _triangleShader, Matrix4.Identity);
    }

    public override void OnGUIRendering()
    {
	    ImGui.Begin("Settings");
	    ImGui.ColorEdit4("Color picker", ref color);
	    ImGui.End();
    }

    public override void OnEvent(ref EventDispatcher e)
    {
	    _cameraController.OnEvent(ref e);
    }

    private Vector4 color = new Vector4(Color.Blue.R, Color.Blue.G, Color.Blue.B, Color.Blue.A);
    public override void OnDetach()
    {
        _triangleShader.Dispose();
        _triangle.Dispose();
    }
}
