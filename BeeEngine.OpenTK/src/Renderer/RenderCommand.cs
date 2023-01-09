using System.Runtime.CompilerServices;
using BeeEngine.Mathematics;
using BeeEngine;
using BeeEngine.Platform.Metal;
using BeeEngine.Platform.OpenGL;

namespace BeeEngine;

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
    public static void DrawIndexed(VertexArray vertexArray, int indexCount)
    {
        DebugTimer.Start();
        _rendererApi.DrawIndexed(vertexArray, indexCount);
        DebugTimer.End();
    }

    static RenderCommand()
    {
        
    }

    public static void Init()
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                _rendererApi = new OpenGLRendererAPI();
                goto SUCCESS;
                #if IOS
            case API.Metal:
                _rendererApi = new MetalRendererAPI();
                goto SUCCESS;
                #endif
        }
        Log.Error("Could not create renderer because of unknown API type");
        throw new InvalidOperationException();
        SUCCESS:
        _rendererApi.Init();
    }

    public static void SetViewPort(int x, int y, int width, int height)
    {
        _rendererApi.SetViewPort(x, y, width, height);
    }
}