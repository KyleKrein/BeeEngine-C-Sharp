using BeeEngine.Mathematics;
using BeeEngine;
using BeeEngine._2D;
using BeeEngine.SmartPointers;
using UnmanageUtility;

namespace BeeEngine;


struct Renderer2DData
{
    public const int MaxRectangles = 10000;
    public const int MaxVertices = MaxRectangles * 4;
    public const int MaxIndices = MaxRectangles * 6;
    public int MaxTextureSlots;

    public Shader TextureShader = null;
    public VertexArray VertexArray = null;
    public Texture2D BlankTexture = null;


    public VertexBuffer RectVertexBuffer = null;
    public int RectIndexCount = 0;
    public UnmanagedArray<RectVertex> RectVerticesBuffer;
    //public RectVertex[] RectVerticesBuffer = null;
    public unsafe RectVertex* CurrentVertex;

    public Texture2D[] TextureSlots;
    public int TextureSlotIndex = 1; // 0 = blank texture

    public Vector4[] RectVertexPositions = new Vector4[4];

    public Renderer2DData()
    {
        MaxTextureSlots = 16;//TODO: Get rendering capabilities
        TextureSlots = new Texture2D[MaxTextureSlots];
    }


    

    public Renderer2D.Statistics Statistics;
}

struct RectVertex
{
    public Vector3 Position;
    public Vector4 Color;
    public Vector2 TexCoord;
    public float TextureIndex;
    public float TilingFactor;
}
public class OpenGLRenderer2DAPI : Renderer2DAPI
{
    private Renderer2DData _data;
    
    static readonly Vector2[] textureCoords = new []
    {
        new Vector2(0.0f, 0.0f), 
        new Vector2(1.0f, 0.0f), 
        new Vector2(1.0f, 1.0f), 
        new Vector2(0.0f, 1.0f)
    }; 
    const int rectVertexCount = 4;
    public unsafe override void Init()
    {
        DebugTimer.Start();
        _data = new Renderer2DData();
        _data.TextureShader = Shader.Create("StandartBeeEngine2DShader",
            @"#version 330 core
			
layout(location = 0) in vec3 a_Position;
layout(location = 1) in vec4 a_Color;
layout(location = 2) in vec2 a_TexCoord;
layout(location = 3) in float a_TextureIndex;
layout(location = 4) in float a_TilingFactor;

uniform mat4 u_ViewProjection;

out vec2 v_TexCoord;
out vec4 v_Color;
out float v_TextureIndex;
out float v_TilingFactor;
void main()
{
    v_Color = a_Color;
	v_TexCoord = a_TexCoord;
	v_TextureIndex = a_TextureIndex;
	v_TilingFactor = a_TilingFactor;
	gl_Position =  u_ViewProjection * vec4(a_Position, 1.0);	
}",
            @"#version 330 core
			
layout(location = 0) out vec4 color;
in vec2 v_TexCoord;
in vec4 v_Color;
in float v_TextureIndex;
in float v_TilingFactor;

uniform sampler2D u_Textures[16];
void main()
{
	color = texture(u_Textures[int(v_TextureIndex)], v_TexCoord * v_TilingFactor) * v_Color;
}");
        int[] samplers = new int[_data.MaxTextureSlots];
        for (int i = 0; i < _data.MaxTextureSlots; i++)
        {
            samplers[i] = i;
        }
        
        _data.TextureShader.Bind();
        _data.TextureShader.UploadUniformIntArray("u_Textures", samplers, _data.MaxTextureSlots);
        
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
            {ShaderDataType.Float2, "a_TexCoord"},
            {ShaderDataType.Float, "a_TextureIndex"},
            {ShaderDataType.Float, "a_TilingFactor"}
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
        _data.TextureSlots[0] = _data.BlankTexture;

        _data.RectVertexPositions[0] = new Vector4(-0.5f, -0.5f, 0.0f, 1.0f);
        _data.RectVertexPositions[1] = new Vector4( 0.5f, -0.5f, 0.0f, 1.0f);
        _data.RectVertexPositions[2] = new Vector4( 0.5f,  0.5f, 0.0f, 1.0f);
        _data.RectVertexPositions[3] = new Vector4(-0.5f,  0.5f, 0.0f, 1.0f);
        
        
        DebugTimer.End();
    }
    
    public unsafe override void BeginScene()
    {
        _data.CurrentVertex = (RectVertex*) _data.RectVerticesBuffer.Ptr;
        _data.RectIndexCount = 0;

        _data.TextureSlotIndex = 1;
        for (int i = 1; i < _data.MaxTextureSlots; i++)
        {
            _data.TextureSlots[i] = null;
        }
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

        for (int i = 0; i < _data.TextureSlotIndex; i++)
        {
            _data.TextureSlots[i].Bind(i);
        }
        RenderCommand.DrawIndexed(_data.VertexArray, _data.RectIndexCount);
        
        _data.Statistics.DrawCalls++;
        
        DebugTimer.End();
    }

    public override void ResetStatistics()
    {
        _data.Statistics.DrawCalls = 0;
        _data.Statistics.QuadCount = 0;
        _data.Statistics.SpriteCount = 0;
    }

    public override Renderer2D.Statistics GetStatistics()
    {
        return _data.Statistics;
    }

    private readonly byte[] _blankTextureData = new byte[] {255, 255, 255, 255};
    public unsafe override void DrawRectangle(ref Vector3 position, ref Vector2 size, ref Vector4 color)
    {
        DebugTimer.Start();

        const float blankTextureIndex = 0.0f;
        
        _data.CurrentVertex->Position = position;
        _data.CurrentVertex->Color = color;
        _data.CurrentVertex->TexCoord = new Vector2(0.0f, 0.0f);
        _data.CurrentVertex->TextureIndex = blankTextureIndex;
        _data.CurrentVertex->TilingFactor = 1.0f;
        _data.CurrentVertex++;
        
        _data.CurrentVertex->Position = new Vector3(position.X + size.X, position.Y, 0.0f);
        _data.CurrentVertex->Color = color;
        _data.CurrentVertex->TexCoord = new Vector2(1.0f, 0.0f);
        _data.CurrentVertex->TextureIndex = blankTextureIndex;
        _data.CurrentVertex->TilingFactor = 1.0f;
        _data.CurrentVertex++;
        
        _data.CurrentVertex->Position = new Vector3(position.X + size.X, position.Y + size.Y, 0.0f);
        _data.CurrentVertex->Color = color;
        _data.CurrentVertex->TexCoord = new Vector2(1.0f, 1.0f);
        _data.CurrentVertex->TextureIndex = blankTextureIndex;
        _data.CurrentVertex->TilingFactor = 1.0f;
        _data.CurrentVertex++;
        
        _data.CurrentVertex->Position = new Vector3(position.X, position.Y + size.Y, 0.0f);
        _data.CurrentVertex->Color = color;
        _data.CurrentVertex->TexCoord = new Vector2(0.0f, 1.0f);
        _data.CurrentVertex->TextureIndex = blankTextureIndex;
        _data.CurrentVertex->TilingFactor = 1.0f;
        _data.CurrentVertex++;

        _data.RectIndexCount += 6;
        
        /*
        UploadTransform(ref transform, _data.TextureShader);
        _data.TextureShader.UploadUniformFloat4("u_Color", (Vector4) color);
        _data.BlankTexture.Bind();
        _data.Rectangle.Bind();
        RenderCommand.DrawIndexed(_data.Rectangle);*/
        DebugTimer.End();
    }

    public unsafe override void DrawRotatedRectangle(ref Vector3 position, ref Vector2 size, ref Vector4 color, float rotationInRadians)
    {
        DebugTimer.Start();

        FlushAndReset();

        const float blankTextureIndex = 0.0f;
        const float TilingFactor = 1.0f;

        Matrix4 transform = Matrix4.CreateTranslation(position) 
                            * Matrix4.CreateRotationZ(rotationInRadians)
                            * Matrix4.CreateScale(new Vector3(size.X, size.Y, 1.0f));
        
        transform.Transpose();
        
        for (int i = 0; i < rectVertexCount; i++)
        {
            _data.CurrentVertex->Position = new Vector3(transform * _data.RectVertexPositions[i]);
            _data.CurrentVertex->Color = color;
            _data.CurrentVertex->TexCoord = textureCoords[i];
            _data.CurrentVertex->TextureIndex = blankTextureIndex;
            _data.CurrentVertex->TilingFactor = TilingFactor;
            _data.CurrentVertex++;
        }

        _data.RectIndexCount += 6;
        
        /*
        UploadTransform(ref transform, _data.TextureShader);
        _data.TextureShader.UploadUniformFloat4("u_Color", (Vector4) color);
        _data.BlankTexture.Bind();
        _data.Rectangle.Bind();
        RenderCommand.DrawIndexed(_data.Rectangle);*/
        _data.Statistics.QuadCount++;
        DebugTimer.End();
    }

    private void FlushAndReset()
    {
        if (_data.RectIndexCount >= Renderer2DData.MaxIndices)
        {
            EndScene();
            BeginScene();
        }
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

    public unsafe override void SetCameraTransform(Matrix4 cameraMatrix)
    {
        _data.TextureShader.Bind();
        _data.TextureShader.UploadUniformMatrix4("u_ViewProjection", &cameraMatrix);
    }

    public override unsafe void SetCameraTransform(SharedPointer<Matrix4> cameraMatrix)
    {
        _data.TextureShader.Bind();
        _data.TextureShader.UploadUniformMatrix4("u_ViewProjection", cameraMatrix.GetPtr());
        cameraMatrix.Release();
    }

    public override void SetColor(int r, int g, int b, int a)
    {
        
    }

    public override void SetTexture2D(Texture2D texture2D)
    {
        
    }

    public unsafe override void DrawTexture2D(ref Vector3 position, ref Vector2 size, Texture2D texture, ref Vector4 color,
        float textureScale)
    {
        DebugTimer.Start();

        float textureIndex = 0.0f;
        for (int i = 0; i < _data.TextureSlotIndex; i++)
        {
            if (_data.TextureSlots[i].Equals(texture))
            {
                textureIndex = i;
                break;
            }
        }
        if (textureIndex == 0.0f)
        {
            textureIndex = _data.TextureSlotIndex;
            _data.TextureSlots[_data.TextureSlotIndex] = texture;
            _data.TextureSlotIndex++;
        }
        _data.CurrentVertex->Position = position;
        _data.CurrentVertex->Color = color;
        _data.CurrentVertex->TexCoord = new Vector2(0.0f, 0.0f);
        _data.CurrentVertex->TextureIndex = textureIndex;
        _data.CurrentVertex->TilingFactor = textureScale;
        _data.CurrentVertex++;
        
        _data.CurrentVertex->Position = new Vector3(position.X + size.X, position.Y, 0.0f);
        _data.CurrentVertex->Color = color;
        _data.CurrentVertex->TexCoord = new Vector2(1.0f, 0.0f);
        _data.CurrentVertex->TextureIndex = textureIndex;
        _data.CurrentVertex->TilingFactor = textureScale;
        _data.CurrentVertex++;
        
        _data.CurrentVertex->Position = new Vector3(position.X + size.X, position.Y + size.Y, 0.0f);
        _data.CurrentVertex->Color = color;
        _data.CurrentVertex->TexCoord = new Vector2(1.0f, 1.0f);
        _data.CurrentVertex->TextureIndex = textureIndex;
        _data.CurrentVertex->TilingFactor = textureScale;
        _data.CurrentVertex++;
        
        _data.CurrentVertex->Position = new Vector3(position.X, position.Y + size.Y, 0.0f);
        _data.CurrentVertex->Color = color;
        _data.CurrentVertex->TexCoord = new Vector2(0.0f, 1.0f);
        _data.CurrentVertex->TextureIndex = textureIndex;
        _data.CurrentVertex->TilingFactor = textureScale;
        _data.CurrentVertex++;

        _data.RectIndexCount += 6;
        
        DebugTimer.End();
    }

    public unsafe override void DrawRotatedTexture2D(ref Vector3 position, ref Vector2 size, Texture2D texture, ref Vector4 color,
        float textureScale, float rotationInRadians)
    {
        DebugTimer.Start();
        
        FlushAndReset();
        float textureIndex = 0.0f;
        for (int i = 0; i < _data.TextureSlotIndex; i++)
        {
            if (_data.TextureSlots[i].Equals(texture))
            {
                textureIndex = i;
                break;
            }
        }
        if (textureIndex == 0.0f)
        {
            if(_data.TextureSlotIndex >= _data.MaxTextureSlots)
                FlushAndReset();
            textureIndex = _data.TextureSlotIndex;
            _data.TextureSlots[_data.TextureSlotIndex] = texture;
            _data.TextureSlotIndex++;
        }
        
        Matrix4 transform = Matrix4.CreateTranslation(position) 
                            * Matrix4.CreateRotationZ(rotationInRadians)
                            * Matrix4.CreateScale(new Vector3(size.X, size.Y, 1.0f));
        transform.Transpose();

        for (int i = 0; i < rectVertexCount; i++)
        {
            _data.CurrentVertex->Position = new Vector3(transform * _data.RectVertexPositions[i]);
            _data.CurrentVertex->Color = color;
            _data.CurrentVertex->TexCoord = textureCoords[i];
            _data.CurrentVertex->TextureIndex = textureIndex;
            _data.CurrentVertex->TilingFactor = textureScale;
            _data.CurrentVertex++;
        }

        _data.RectIndexCount += 6;
        
        _data.Statistics.QuadCount++;
        _data.Statistics.SpriteCount++;
        
        DebugTimer.End();
    }

    public override void DrawRotatedTexture2D(ref Vector3 position, ref Vector2 size, Sprite sprite, ref Vector4 color, float textureScale,
        float rotationInRadians)
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