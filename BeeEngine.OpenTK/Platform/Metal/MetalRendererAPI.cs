using BeeEngine.Mathematics;
using MetalKit;

namespace BeeEngine.Platform.Metal;

public class MetalRendererAPI: RendererAPI
{
    public override void SetClearColor(Color color)
    {
        Metal.ClearColor((Vector4)color);
    }

    public override void Clear()
    {
        
    }

    public override void DrawIndexed(VertexArray vertexArray)
    {
        
        
    }

    public override void DrawIndexed(VertexArray vertexArray, int indexCount)
    {
        Metal.DrawIndexedTriangles((nuint) indexCount);
    }

    public override void Init()
    {
        
    }

    public override void SetViewPort(int x, int y, int width, int height)
    {
        
    }
}