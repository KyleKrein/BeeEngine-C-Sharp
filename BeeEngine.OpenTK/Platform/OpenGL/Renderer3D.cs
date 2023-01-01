using System.Drawing;
using BeeEngine.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.OpenTK;

internal class Renderer3D
{
    private int _vertexBufferObject;

    public void Init()
    {
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
    }

    public void CopyVerticesToBuffer(float[] vertices, BufferUsageHint hint)
    {
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, hint);
    }
}