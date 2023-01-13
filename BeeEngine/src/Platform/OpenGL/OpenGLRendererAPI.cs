using BeeEngine.Mathematics;
using BeeEngine;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.Platform.OpenGL;

public class OpenGLRendererAPI: RendererAPI
{
    private int _osScale = 1;
    public override void SetClearColor(Color color)
    {
        GL.ClearColor(color.R, color.G, color.B, color.A);
    }

    public override void Clear()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public override void DrawIndexed(VertexArray vertexArray)
    {
        //GL.DrawElements(BeginMode.Triangles, vertexArray.IndexBuffer.Count, DrawElementsType.UnsignedInt, 0);
        GL.DrawElements(PrimitiveType.Triangles, vertexArray.IndexBuffer.Count, DrawElementsType.UnsignedInt, (int) 0);
    }

    public override void DrawIndexed(VertexArray vertexArray, int indexCount)
    {
        GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, (int) 0);
    }

    public override void Init()
    {
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.Enable(EnableCap.DepthTest);
        if (Application.PlatformOS == OS.Mac)
        {
            _osScale = 2;
        }
    }

    public override void SetViewPort(int x, int y, int width, int height)
    {
        GL.Viewport(x,y, width* _osScale, height* _osScale);
    }
}