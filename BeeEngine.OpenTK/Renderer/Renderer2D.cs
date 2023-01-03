using BeeEngine.Drawing;
using BeeEngine.Mathematics;
using BeeEngine.OpenTK.Renderer;

namespace BeeEngine.OpenTK;

public static class Renderer2D
{
    public static void BeginScene(OrthographicCamera camera)
    {
        RenderCommand.Clear();
        RenderCommand2D.SetCamera(camera);
    }

    public static void Init()
    {
        RenderCommand2D.Init();
    }

    public static void Shutdown()
    {
        
    }


    public static void DrawRectangle(Point position, Size size, Color color)
    {
        DrawRectangle(position.X, position.Y, size.Width, size.Height, color);
    }

    public static void DrawRectangle(float x, float y, float width, float height, Color color)
    {
        RenderCommand2D.DrawRectangle(x, y, width, height, color);
    }

    public static void DrawImage(float x, float y, float width, float height, Texture2D texture)
    {
        RenderCommand2D.DrawTexture2D(x, y, width, height, texture);
    }
}