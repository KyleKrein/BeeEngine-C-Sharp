using BeeEngine.Mathematics;

namespace BeeEngine;

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
    public abstract void DrawIndexed(VertexArray vertexArray, int indexCount);

    public abstract void Init();

    public abstract void SetViewPort(int x, int y, int width, int height);
}