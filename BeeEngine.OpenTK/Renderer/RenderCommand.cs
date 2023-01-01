using System.Runtime.CompilerServices;
using BeeEngine.Mathematics;
using BeeEngine.OpenTK.Platform.OpenGL;

namespace BeeEngine.OpenTK.Renderer;

public static class RenderCommand
{
    private static RendererAPI _rendererApi;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetClearColor(Color color)
    {
        _rendererApi.SetClearColor(color);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear()
    {
        _rendererApi.Clear();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DrawIndexed(VertexArray vertexArray)
    {
        _rendererApi.DrawIndexed(vertexArray);
    }

    static RenderCommand()
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                _rendererApi = new OpenGLRendererAPI();
                return;
        }
        Log.Error("Could not create renderer because of unknown API type");
        throw new InvalidOperationException();
    }
}