using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BeeEngine.Mathematics;
using UnmanageUtility;

namespace BeeEngine.OpenTK.Renderer;

struct Renderer2DData
{
    public const int MaxRectangles = 10000;
    public const int MaxVertices = MaxRectangles * 4;
    public const int MaxIndices = MaxRectangles * 6;

    public Shader TextureShader = null;
    public VertexArray VertexArray = null;
    public Texture2D BlankTexture = null;


    public VertexBuffer RectVertexBuffer = null;
    public int RectIndexCount = 0;
    public UnmanagedArray<RectVertex> RectVerticesBuffer;
    //public RectVertex[] RectVerticesBuffer = null;
    public unsafe RectVertex* CurrentVertex;

    public Renderer2DData()
    {
    }
}

struct RectVertex
{
    public Vector3 Position;
    public Vector4 Color;
    public Vector2 TexCoord;
}
public class OpenGLRenderer2DAPI : Renderer2DAPI
{
    private Renderer2DData _data;
    
    public unsafe override void Init()
    {
        DebugTimer.Start();
        _data = new Renderer2DData();
        _data.TextureShader = Shader.Create("StandartBeeEngine2DShader",
            @"#version 330 core
			
layout(location = 0) in vec3 a_Position;
layout(location = 1) in vec4 a_Color;
layout(location = 2) in vec2 a_TexCoord;

uniform mat4 u_ViewProjection;

out vec2 v_TexCoord;
out vec4 v_Color;
void main()
{
    v_Color = a_Color;
	v_TexCoord = a_TexCoord;
	gl_Position =  u_ViewProjection * vec4(a_Position, 1.0);	
}",
            @"#version 330 core
			
layout(location = 0) out vec4 color;
in vec2 v_TexCoord;
in vec4 v_Color;

uniform sampler2D u_Texture;
uniform float u_TextureScale;
void main()
{
	//color = texture(u_Texture, v_TexCoord * u_TextureScale) * u_Color;
	color = v_Color;
}");
        _data.TextureShader.Bind();
        _data.TextureShader.UploadUniformInt("u_Texture", 0);
        
        /*float[] rectangleVertices =
        {
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, //bottom left
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f, //bottom right
            0.5f,  0.5f, 0.0f, 1.0f, 1.0f,// top right
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f,// top left
        };
        uint[] rectangleIndices =
        {
            0, 1, 2, 2, 3, 0
        };*/
        

        _data.VertexArray = VertexArray.Create();
        _data.RectVertexBuffer = VertexBuffer.Create(Renderer2DData.MaxVertices * sizeof(RectVertex));
        
        BufferLayout squareLayout = new BufferLayout()
        {
            {ShaderDataType.Float3, "a_Position"},
            {ShaderDataType.Float4, "a_Color"},
            {ShaderDataType.Float2, "a_TexCoord"}
        };
        _data.RectVertexBuffer.Layout = squareLayout;
        _data.VertexArray.AddVertexBuffer(_data.RectVertexBuffer);

        _data.RectVerticesBuffer = new UnmanagedArray<RectVertex>(Renderer2DData.MaxVertices);

        uint[] rectangleIndices = new uint[Renderer2DData.MaxIndices];
        uint offset = 0;
        for (int i = 0; i < Renderer2DData.MaxIndices; i+=6)
        {
            rectangleIndices[i + 0] = offset + 0;
            rectangleIndices[i + 1] = offset + 1;
            rectangleIndices[i + 2] = offset + 2;
            
            rectangleIndices[i + 3] = offset + 2;
            rectangleIndices[i + 4] = offset + 3;
            rectangleIndices[i + 5] = offset + 0;
            offset += 4;
        }
        
        var rectangleIndexBuffer = IndexBuffer.Create(rectangleIndices);
        
        
        _data.VertexArray.SetIndexBuffer(rectangleIndexBuffer);
        _data.BlankTexture = Texture2D.Create(1, 1);
        _data.BlankTexture.SetData(_blankTextureData, 4);
        DebugTimer.End();
    }
    
    public unsafe override void BeginScene()
    {
        _data.CurrentVertex = (RectVertex*) _data.RectVerticesBuffer.Ptr;
        _data.RectIndexCount = 0;
    }

    public unsafe override void EndScene()
    {
        DebugTimer.Start();
        int size = (int)_data.CurrentVertex - (int)_data.RectVerticesBuffer.Ptr;
        _data.RectVertexBuffer.SetData(_data.RectVerticesBuffer.Ptr, size);
        Flush();
        DebugTimer.End();
    }

    public override void Flush()
    {
        DebugTimer.Start();
        
        RenderCommand.DrawIndexed(_data.VertexArray, _data.RectIndexCount);
        
        DebugTimer.End();
    }

    private readonly byte[] _blankTextureData = new byte[] {255, 255, 255, 255};
    public unsafe override void DrawRectangle(ref Vector3 position, ref Vector2 size, ref Vector4 color)
    {
        //DebugTimer.Start();

        _data.CurrentVertex->Position = position;
        _data.CurrentVertex->Color = color;
        _data.CurrentVertex->TexCoord = new Vector2(0.0f, 0.0f);
        _data.CurrentVertex++;
        
        _data.CurrentVertex->Position = new Vector3(position.X + size.X, position.Y, 0.0f);
        _data.CurrentVertex->Color = color;
        _data.CurrentVertex->TexCoord = new Vector2(1.0f, 0.0f);
        _data.CurrentVertex++;
        
        _data.CurrentVertex->Position = new Vector3(position.X + size.X, position.Y + size.Y, 0.0f);
        _data.CurrentVertex->Color = color;
        _data.CurrentVertex->TexCoord = new Vector2(1.0f, 1.0f);
        _data.CurrentVertex++;
        
        _data.CurrentVertex->Position = new Vector3(position.X, position.Y + size.Y, 0.0f);
        _data.CurrentVertex->Color = color;
        _data.CurrentVertex->TexCoord = new Vector2(0.0f, 1.0f);
        _data.CurrentVertex++;

        _data.RectIndexCount += 6;
        
        /*
        UploadTransform(ref transform, _data.TextureShader);
        _data.TextureShader.UploadUniformFloat4("u_Color", (Vector4) color);
        _data.BlankTexture.Bind();
        _data.Rectangle.Bind();
        RenderCommand.DrawIndexed(_data.Rectangle);*/
        //DebugTimer.End();
    }
    public override void DrawRectangle(ref Matrix4 transform, Color color)
    {
        DebugTimer.Start();
        //_textureShader.Bind();
        UploadTransform(ref transform, _data.TextureShader);
        _data.TextureShader.UploadUniformFloat4("u_Color", (Vector4) color);
        _data.BlankTexture.Bind();
        _data.VertexArray.Bind();
        RenderCommand.DrawIndexed(_data.VertexArray);
        DebugTimer.End();
    }

    private void UploadTransform(ref Matrix4 transform, Shader shader)
    {
        shader.UploadUniformMatrix4("u_Transform", ref transform);
    }

    public override void SetCameraTransform(Matrix4 cameraMatrix)
    {
        _data.TextureShader.Bind();
        _data.TextureShader.UploadUniformMatrix4("u_ViewProjection", ref cameraMatrix);
    }

    public override void SetColor(int r, int g, int b, int a)
    {
        
    }

    public override void SetTexture2D(Texture2D texture2D)
    {
        
    }
    public override void DrawTexture2D(ref Matrix4 transform, Texture2D texture, Vector4 color, float textureScale)
    {
        DebugTimer.Start();
        _data.TextureShader.UploadUniformFloat4("u_Color", color);
        _data.TextureShader.UploadUniformFloat("u_TextureScale", textureScale);
        texture.Bind();
        //_textureShader.Bind();
        UploadTransform(ref transform, _data.TextureShader);
        _data.VertexArray.Bind();
        RenderCommand.DrawIndexed(_data.VertexArray);
        DebugTimer.End();
    }

    
}