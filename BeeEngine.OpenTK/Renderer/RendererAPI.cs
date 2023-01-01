using BeeEngine.Mathematics;

namespace BeeEngine.OpenTK.Renderer;

public abstract class RendererAPI
{
    public static API API
    {
        get => Renderer.API;
        set => Renderer.API = value;
    }
    public abstract void SetClearColor(Color color);
    public abstract void Clear();
    public abstract void DrawIndexed(VertexArray vertexArray);
}