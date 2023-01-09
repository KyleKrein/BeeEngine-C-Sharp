#if IOS
using Metal;

namespace BeeEngine.Platform.Metal;

public class MetalVertexBuffer: VertexBuffer
{
    private IMTLBuffer _buffer;
    public MetalVertexBuffer(float[] vertices)
    {
        _buffer = Metal.CreateBuffer(vertices);
    }

    public MetalVertexBuffer(int size)
    {
        _buffer = Metal.CreateBuffer((nuint) size);
    }

    public override void Bind()
    {
        Metal.BindBuffer(_buffer, ShaderType.Vertex);
    }

    public override void Unbind()
    {
        Metal.BindBuffer(null, ShaderType.Vertex);
    }

    protected override void Dispose(bool disposing)
    {
        _buffer.Dispose();
    }

    public override void SetData(nint data, int size)
    {
        _buffer?.Dispose();
        _buffer = Metal.CreateBuffer(data, (nuint) size);
    }
}
#endif