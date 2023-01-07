using BeeEngine;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.Platform.OpenGL;

public class OpenGLVertexArray: VertexArray
{
    public override BufferLayout Layout { get; }
    private int _rendererID;

    public OpenGLVertexArray()
    {
        if (Application.PlatformOS == OS.Mac)
        {
            GL.GenVertexArrays(1, out _rendererID);//On Mac it crashes on CreateVertexArrays
        }
        else
        {
            GL.CreateVertexArrays(1, out _rendererID);
        }
    }
    public override void Bind()
    {
        DebugTimer.Start("OpenGLVertexArray.Bind()");
        GL.BindVertexArray(_rendererID);
        DebugTimer.End("OpenGLVertexArray.Bind()");
    }

    public override void Unbind()
    {
        GL.BindVertexArray(0);
    }

    public override void AddVertexBuffer(VertexBuffer buffer)
    {
        DebugTimer.Start();
        Log.Assert(buffer.Layout.BufferElements.Count > 0, "Vertex buffer has no layout!");
        
        GL.BindVertexArray(_rendererID);
        buffer.Bind();
        
        uint index = 0;
        var layout = buffer.Layout;
        foreach (var element in layout)
        {
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, 
                element.GetComponentCount(), 
                ShaderDataTypeToOpenGLType(element.Type),
                element.Normalized, 
                layout.Stride,
                element.Offset);
            index++;
        }
        VertexBuffers.Add(buffer);
        DebugTimer.End();
    }

    private VertexAttribPointerType ShaderDataTypeToOpenGLType(ShaderDataType type)
    {
        switch (type)
        {
            case ShaderDataType.Float:
                return VertexAttribPointerType.Float;
            case ShaderDataType.Float2:
                return VertexAttribPointerType.Float;
            case ShaderDataType.Float3:
                return VertexAttribPointerType.Float;
            case ShaderDataType.Float4:
                return VertexAttribPointerType.Float;;
            case ShaderDataType.Mat3:
                return VertexAttribPointerType.Float;
            case ShaderDataType.Mat4:
                return VertexAttribPointerType.Float;
            case ShaderDataType.Int:
                return VertexAttribPointerType.Int;
            case ShaderDataType.Int2:
                return VertexAttribPointerType.Int;
            case ShaderDataType.Int3:
                return VertexAttribPointerType.Int;
            case ShaderDataType.Int4:
                return VertexAttribPointerType.Int;
            case ShaderDataType.Bool:
                return VertexAttribPointerType.Byte;
        }
        
        Log.Error("Unknown data type!");
        return 0;
    }
    public override void SetIndexBuffer(IndexBuffer buffer)
    {
        DebugTimer.Start();
        GL.BindVertexArray(_rendererID);
        buffer.Bind();
        IndexBuffer = buffer;
        DebugTimer.End();
    }

    protected override void Dispose(bool disposing)
    {
        GL.DeleteVertexArrays(1, ref _rendererID);
    }
}