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
        DebugTimer.Start();
        _rendererApi.DrawIndexed(vertexArray);
        DebugTimer.End();
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

    public static void Init()
    {
        _rendererApi.Init();
    }

    public static void SetViewPort(int x, int y, int width, int height)
    {
        _rendererApi.SetViewPort(x, y, width, height);
    }
}