using BeeEngine.Mathematics;

namespace BeeEngine.OpenTK.Renderer;

public class OpenGLRenderer2DAPI : Renderer2DAPI
{
    private Shader _flatColorShader;
    private Shader _textureShader;
    private VertexArray _rectangle;
    public override void Init()
    {
        _flatColorShader = Renderer.Shaders.Load(@"Assets/Shaders/FlatColor.glsl");
        _textureShader = Renderer.Shaders.Load(@"Assets/Shaders/Texture.glsl");
        _textureShader.Bind();
        _textureShader.UploadUniformInt("u_Texture", 0);
        
        float[] rectangleVertices =
        {
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, //bottom left
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f, //bottom right
            0.5f,  0.5f, 0.0f, 1.0f, 1.0f,// top right
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f,// top left
        };
        uint[] rectangleIndices =
        {
            0, 1, 2, 2, 3, 0
        };

        _rectangle = VertexArray.Create();
        var rectangleVertexBuffer = VertexBuffer.Create(rectangleVertices, 0);
        var rectangleIndexBuffer = IndexBuffer.Create(rectangleIndices);
        BufferLayout squareLayout = new BufferLayout()
        {
            {ShaderDataType.Float3, "a_Position"},
            {ShaderDataType.Float2, "a_TexCoord"}
        };
        rectangleVertexBuffer.Layout = squareLayout;
        _rectangle.AddVertexBuffer(rectangleVertexBuffer);
        _rectangle.SetIndexBuffer(rectangleIndexBuffer);
    }

    public override void DrawRectangle(ref Matrix4 transform, Color color)
    {
        _flatColorShader.Bind();
        UploadTransform(ref transform, _flatColorShader);
        _flatColorShader.UploadUniformFloat4("u_Color", (Vector4) color);
        _rectangle.Bind();
        RenderCommand.DrawIndexed(_rectangle);
    }

    private void UploadTransform(ref Matrix4 transform, Shader shader)
    {
        shader.UploadUniformMatrix4("u_Transform", ref transform);
    }

    public override void SetCameraTransform(Matrix4 cameraMatrix)
    {
        _flatColorShader.Bind();
        _flatColorShader.UploadUniformMatrix4("u_ViewProjection", ref cameraMatrix);
        _textureShader.Bind();
        _textureShader.UploadUniformMatrix4("u_ViewProjection", ref cameraMatrix);
    }

    public override void SetColor(int r, int g, int b, int a)
    {
        
    }

    public override void SetTexture2D(Texture2D texture2D)
    {
        
    }

    public override void DrawTexture2D(ref Matrix4 transform, Texture2D texture)
    {
        texture.Bind();
        _textureShader.Bind();
        UploadTransform(ref transform, _textureShader);
        _rectangle.Bind();
        RenderCommand.DrawIndexed(_rectangle);
    }
}