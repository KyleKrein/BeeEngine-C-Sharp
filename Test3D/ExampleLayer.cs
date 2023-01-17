using BeeEngine;
using BeeEngine.Events;
using BeeEngine.Gui;
using BeeEngine.Mathematics;

namespace Test3D;

public class ExampleLayer: Layer
{
    private PerspectiveCameraController _cameraController;
    private FpsCounter _fpsCounter = new FpsCounter();
    private Texture2D _forest;
    private VertexArray _cube;
    private Shader _triangleShader;
    private Matrix4 _cubeTransform = Matrix4.Identity;
    public override void OnAttach()
    {
        _cameraController = new PerspectiveCameraController();
        _cameraController.Disable();
        _cameraController.CameraPosition = new Vector3(0, 0, 10);
        _forest = Texture2D.CreateFromFile(@"Assets\Textures\forest.png");
        
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
        
        _cube = VertexArray.Create();
        VertexBuffer vertexBuffer = VertexBuffer.Create(cubeVertices);
        IndexBuffer indexBuffer = IndexBuffer.Create(cubeIndices);
        
        var layout = new BufferLayout()
        {
            {ShaderDataType.Float3, "a_Position"},
            {ShaderDataType.Float4, "a_Color"}
        };
        
        vertexBuffer.Layout = layout;
        _cube.AddVertexBuffer(vertexBuffer);
        _cube.SetIndexBuffer(indexBuffer);
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
    }

    public override void OnDetach()
    {
        _forest.Dispose();
    }

    public override void OnEvent(ref EventDispatcher e)
    {
        e.Dispatch<MouseDownEvent>(OnMouseDown);
        e.Dispatch<MouseUpEvent>(OnMouseUp);
        _cameraController.OnEvent(ref e);
    }

    private bool OnMouseUp(MouseUpEvent e)
    {
        if (e.Button == MouseButton.Left)
        {
            _cameraController.Disable();
            Cursor.SetCursorState(CursorState.Normal);
        }

        return false;
    }

    private bool OnMouseDown(MouseDownEvent e)
    {
        if (e.Button == MouseButton.Left)
        {
            _cameraController.Enable();
            Cursor.SetCursorState(CursorState.Disabled);
        }

        return false;
    }

    private float rotation = 0;
    public override void OnUpdate()
    { 
        _fpsCounter.Update();
        //using var t = new Timer(); 
        /*Renderer2D.ResetStatistics();
        
        _cameraController.OnUpdate();
        Renderer2D.BeginScene(_cameraController);

        for (int i = 0; i < 24*5; i++)
        {
            for (int j = 0; j < 30*5; j++)
            {
                //Renderer2D.DrawRectangle(j, i, 1, 1, Color.Yellow);
                Renderer2D.DrawImage(j,i,0,1,1,_forest, Color.White, rotation*Time.DeltaTime, 1);
            }
        }
        Renderer2D.EndScene();*/
        _cameraController.OnUpdate();
        Renderer.BeginScene(_cameraController);
        RenderCommand.Clear();
        Renderer.Submit(_cube, _triangleShader, _cubeTransform);
        Renderer.EndScene();
    }

    public override void OnGUIRendering()
    {
        _fpsCounter.Render();
    }
}