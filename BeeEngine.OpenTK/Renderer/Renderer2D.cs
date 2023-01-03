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
        DebugTimer.Start();
        RenderCommand2D.Init();
        DebugTimer.End();
    }

    public static void Shutdown()
    {
        
    }


    public static void DrawRectangle(Point position, Size size, Color color)
    {
        DrawRectangle(position.X, position.Y, 0, size.Width, size.Height, color);
    }

    public static void DrawRectangle(float x, float y, float width, float height, Color color)
    {
        DrawRectangle(x, y, 0, width, height, color);
    }
    public static void DrawRectangle(float x, float y,float z ,float width, float height, Color color)
    {
        RenderCommand2D.DrawRectangle(x, y, z, width, height, color);
    }

    public static void DrawImage(float x, float y, float width, float height, Texture2D texture)
    {
        DrawImage(x, y, 0, width, height, texture);
    }
    public static void DrawImage(float x, float y, float z, float width, float height, Texture2D texture)
    {
        DrawImage(x, y, z, width, height, texture, new Color(255, 255, 255, 255));
    }
    public static void DrawImage(float x, float y, float width, float height, Texture2D texture, Color color)
    {
        DrawImage(x, y, 0, width, height, texture, color);
    }
    public static void DrawImage(float x, float y, float z, float width, float height, Texture2D texture, Color color)
    {
        RenderCommand2D.DrawTexture2D(x, y, z, width, height, texture, color);
    }
}