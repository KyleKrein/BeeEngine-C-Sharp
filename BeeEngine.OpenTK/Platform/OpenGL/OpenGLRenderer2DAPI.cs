using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BeeEngine.Mathematics;

namespace BeeEngine.OpenTK.Renderer;

public class OpenGLRenderer2DAPI : Renderer2DAPI
{
    private Shader _textureShader;
    private VertexArray _rectangle;
    private Texture2D _blankTexture;
    public override void Init()
    {
        _textureShader = Shader.Create("StandartBeeEngine2DShader",
            @"#version 330 core
			
layout(location = 0) in vec3 a_Position;
layout(location = 1) in vec2 a_TexCoord;

uniform mat4 u_ViewProjection;
uniform mat4 u_Transform;

out vec2 v_TexCoord;
void main()
{
	v_TexCoord = a_TexCoord;
	gl_Position =  u_ViewProjection * u_Transform * vec4(a_Position, 1.0);	
}",
            @"#version 330 core
			
layout(location = 0) out vec4 color;
in vec2 v_TexCoord;

uniform vec4 u_Color;
uniform sampler2D u_Texture;
uniform float u_TextureScale;
void main()
{
	color = texture(u_Texture, v_TexCoord * u_TextureScale) * u_Color;
}");
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
        _blankTexture = Texture2D.Create(1, 1);
        _blankTexture.SetData(_blankTextureData, 4);
    }

    private readonly byte[] _blankTextureData = new byte[] {255, 255, 255, 255};
    public override void DrawRectangle(ref Matrix4 transform, Color color)
    {
        //_textureShader.Bind();
        UploadTransform(ref transform, _textureShader);
        _textureShader.UploadUniformFloat4("u_Color", (Vector4) color);
        _blankTexture.Bind();
        _rectangle.Bind();
        RenderCommand.DrawIndexed(_rectangle);
    }

    private void UploadTransform(ref Matrix4 transform, Shader shader)
    {
        shader.UploadUniformMatrix4("u_Transform", ref transform);
    }

    public override void SetCameraTransform(Matrix4 cameraMatrix)
    {
        _textureShader.Bind();
        _textureShader.UploadUniformMatrix4("u_ViewProjection", ref cameraMatrix);
    }

    public override void SetColor(int r, int g, int b, int a)
    {
        
    }

    public override void SetTexture2D(Texture2D texture2D)
    {
        
    }
    public override void DrawTexture2D(ref Matrix4 transform, Texture2D texture, Vector4 color, float textureScale)
    {
        _textureShader.UploadUniformFloat4("u_Color", color);
        _textureShader.UploadUniformFloat("u_TextureScale", textureScale);
        texture.Bind();
        //_textureShader.Bind();
        UploadTransform(ref transform, _textureShader);
        _rectangle.Bind();
        RenderCommand.DrawIndexed(_rectangle);
    }
}