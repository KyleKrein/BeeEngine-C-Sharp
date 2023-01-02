using BeeEngine.Mathematics;
using BeeEngine.OpenTK.Renderer;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.OpenTK.Platform.OpenGL;

public class OpenGLRendererAPI: RendererAPI
{
    public override void SetClearColor(Color color)
    {
        GL.ClearColor(color.R, color.G, color.B, color.A);
    }

    public override void Clear()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
    }

    public override void DrawIndexed(VertexArray vertexArray)
    {
        //GL.DrawElements(BeginMode.Triangles, vertexArray.IndexBuffer.Count, DrawElementsType.UnsignedInt, 0);
        GL.DrawElements(PrimitiveType.Triangles, vertexArray.IndexBuffer.Count, DrawElementsType.UnsignedInt, (int) 0);
    }

    public override void Init()
    {
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    }
}