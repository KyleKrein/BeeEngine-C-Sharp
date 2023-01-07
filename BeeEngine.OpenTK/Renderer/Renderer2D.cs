using BeeEngine.Drawing;
using BeeEngine.Mathematics;
using BeeEngine;

namespace BeeEngine;

public static class Renderer2D
{
    public struct Statistics
    {
        public int DrawCalls;
        public int QuadCount;
        public int SpriteCount;
        public int TotalVertexCount => QuadCount * 4;
        
    }
    public static void BeginScene(OrthographicCamera camera)
    {
        DebugTimer.Start();
        RenderCommand.Clear();
        RenderCommand2D.SetCamera(camera);
        RenderCommand2D.BeginScene();
        DebugTimer.End();
    }
    public static void EndScene()
    {
        DebugTimer.Start();

        RenderCommand2D.EndScene();
        
        DebugTimer.End();
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

    public static void DrawRectangle(ref RectangleProperties properties)
    {
        DrawRectangle(properties.X, properties.Y, properties.Z, properties.Width, properties.Height, properties.Color, properties.Rotation);
    }
    public static void DrawRectangle(Point position, Size size, Color color)
    {
        DrawRectangle(position.X, position.Y, 0, size.Width, size.Height, color, 0f);
    }

    public static void DrawRectangle(float x, float y, float width, float height, Color color)
    {
        DrawRectangle(x, y, 0, width, height, color, 0f);
    }
    public static void DrawRectangle(float x, float y,float z ,float width, float height, Color color, float rotation)
    {
        RenderCommand2D.DrawRectangle(x, y, z, width, height, color, rotation);
    }

    public static void DrawImage(float x, float y, float width, float height, Texture2D texture)
    {
        DrawImage(x, y, 0, width, height, texture);
    }
    public static void DrawImage(float x, float y, float z, float width, float height, Texture2D texture)
    {
        DrawImage(x, y, z, width, height, texture, Color.White, 0f, 1f);
    }

    public static void DrawImage(ref RectangleProperties properties, Texture2D texture, float textureMultiplier = 1f)
    {
        DrawImage(properties.X, properties.Y, properties.Z, properties.Width, properties.Height, texture, properties.Color, properties.Rotation, textureMultiplier);
    }
    public static void DrawImage(float x, float y, float width, float height, Texture2D texture, Color color)
    {
        DrawImage(x, y, 0, width, height, texture, color, 0f, 1f);
    }
    public static void DrawImage(float x, float y, float z, float width, float height, Texture2D texture, Color color, float rotation, float textureMultiplier)
    {
        RenderCommand2D.DrawTexture2D(x, y, z, width, height, texture, color, textureMultiplier, MathHelper.DegreesToRadians(rotation));
    }

    public static void ResetStatistics()
    {
        RenderCommand2D.ResetStatistics();
    }

    public static Statistics GetStatistics()
    {
        return RenderCommand2D.GetStatistics();
    }
    
}