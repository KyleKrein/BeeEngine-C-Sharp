#if IOS
using Metal;

namespace BeeEngine.Platform.Metal;

public class MetalIndexBuffer: IndexBuffer
{
    private IMTLBuffer _buffer;
    public MetalIndexBuffer(uint[] indices)
    {
        _buffer = Metal.CreateBuffer(indices);
    }

    public override void Bind()
    {
        Metal.BindBuffer(_buffer, ShaderType.Fragment);
    }

    public override void Unbind()
    {
        
    }

    protected override void Dispose(bool disposing)
    {
        _buffer.Dispose();
    }
}
#endif