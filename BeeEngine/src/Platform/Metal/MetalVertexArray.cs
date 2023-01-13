namespace BeeEngine.Platform.Metal;

public class MetalVertexArray: VertexArray
{
    public override BufferLayout Layout { get; }
    public override void Bind()
    {
        foreach (var vertexBuffer in VertexBuffers)
        {
            vertexBuffer.Bind();
        }
        IndexBuffer.Bind();
    }

    public override void Unbind()
    {
        
    }

    public override void AddVertexBuffer(VertexBuffer buffer)
    {
        VertexBuffers.Add(buffer);
    }

    public override void SetIndexBuffer(IndexBuffer buffer)
    {
        IndexBuffer = buffer;
    }

    protected override void Dispose(bool disposing)
    {
        
    }
}