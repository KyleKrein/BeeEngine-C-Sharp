using SDL2;

namespace BeeEngine.Drawing;

public sealed class Graphics: IDisposable
{
    private Texture? _target = null;
    private Graphics(Texture texture)
    {
        _target = texture;
        SDL.SDL_SetTextureBlendMode(texture._texture, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND); //Делает прозрачной картинку, но надо почитать подробнее
        Renderer.SetRenderTarget(texture._texture);
    }

    internal Graphics()
    {
        
    }

    public static Graphics FromTexture(Texture texture)
    {
        return new Graphics(texture);
    }
    

    /*public void DrawLine(Vector2 start, Vector2 end, ColorRGB color)
    {
        DrawLine((int) start.X, (int) start.Y, (int) end.X, (int) end.Y, color);
    }*/
    public void DrawLine(int x1, int y1, int x2, int y2, Color color)
    {
        //CheckIfRendererExists();
        Renderer.DrawLine(x1, y1, x2, y2, color);
    }
    public void DrawPoint(Point point, Color color)
    {
        DrawPoint(point.X, point.Y, color);
    }
    public void DrawPoint(int x, int y, Color color)
    {
        //CheckIfRendererExists();
        Renderer.DrawPoint(x, y, color);
    }
    public void DrawPoint(PointF point, Color color)
    {
        DrawPoint(point.X, point.Y, color);
    }
    /*public void DrawPoint(Vector2 position, ColorRGB color)
    {
        DrawPoint(position.X, position.Y, color);
    }*/
    public void DrawPoint(float x, float y, Color color)
    {
        //CheckIfRendererExists();
        Renderer.DrawPointF(x, y, color);
    }

    public void DrawPoints(Color color, Point[] points)
    {
        //CheckIfRendererExists();
        SDL.SDL_Point[] sdlPoints = new SDL.SDL_Point[points.Length];
        for (int i = 0; i < sdlPoints.Length; i++)
        {
            sdlPoints[i] = points[i];
        }

        Renderer.DrawPoints(sdlPoints, color);
    }
    public void DrawPoints(Color color, PointF[] points)
    {
        //CheckIfRendererExists();
        SDL.SDL_FPoint[] sdlPoints = new SDL.SDL_FPoint[points.Length];
        for (int i = 0; i < sdlPoints.Length; i++)
        {
            sdlPoints[i] = points[i];
        }
        Renderer.DrawPointsF(sdlPoints, color);
    }
    public void DrawPoints(Color color, SDL.SDL_Point[] points)
    {
        //CheckIfRendererExists();
        Renderer.DrawPoints(points, color);
    }
    public void DrawPoints(Color color, SDL.SDL_FPoint[] points)
    {
        //CheckIfRendererExists();
        Renderer.DrawPointsF(points, color);
    }

    public void DrawRectangle(Rectangle rectangle, Color color)
    {
        SDL.SDL_Rect rect = rectangle;
        //CheckIfRendererExists();
        Renderer.DrawRect(rect, color);
    }
    public void DrawRectangle(SDL.SDL_Rect rectangle, Color color)
    {
        //CheckIfRendererExists();
        Renderer.DrawRect(rectangle, color);
    }
    public void DrawRectangle(RectangleF rectangle, Color color)
    {
        SDL.SDL_FRect rect = rectangle;
        //CheckIfRendererExists();
        Renderer.DrawRectF(rect, color);
    }
    public void DrawRectangle(SDL.SDL_FRect rectangle, Color color)
    {
        //CheckIfRendererExists();
        Renderer.DrawRectF(rectangle, color);
    }

    public void DrawRectangles(Rectangle[] rectangles, Color color)
    {
        //CheckIfRendererExists();
        
        SDL.SDL_Rect[] sdlRects = new SDL.SDL_Rect[rectangles.Length];
        for (int i = 0; i < sdlRects.Length; i++)
        {
            sdlRects[i] = rectangles[i];
        }

        Renderer.DrawRects(sdlRects, color);
    }
    public void DrawRectangles(RectangleF[] rectangles, Color color)
    {
        //CheckIfRendererExists();
        
        SDL.SDL_FRect[] sdlRects = new SDL.SDL_FRect[rectangles.Length];
        for (int i = 0; i < sdlRects.Length; i++)
        {
            sdlRects[i] = rectangles[i];
        }
        Renderer.DrawRectsF(sdlRects, color);
    }
    public void DrawRectangles(SDL.SDL_Rect[] rectangles, Color color)
    {
        //CheckIfRendererExists();
        Renderer.DrawRects(rectangles, color);
    }
    public void DrawRectangles(SDL.SDL_FRect[] rectangles, Color color)
    {
        Renderer.DrawRectsF(rectangles, color);
    }

    public void DrawTexture(Texture texture, int x, int y)
    {
        var targetRect = new SDL.SDL_Rect() { x = x, y = y, h = texture.Height, w = texture.Width};
        Renderer.DrawTexture(texture, texture.TextureRect, targetRect);
    }
    public void DrawTexture(Texture texture, int x, int y, int width, int height)
    {
        var targetRect = new SDL.SDL_Rect() { x = x, y = y, h = height, w = width};
        Renderer.DrawTexture(texture, texture.TextureRect, targetRect);
    }

    private void ReleaseUnmanagedResources()
    {
        if (_target is not null)
        {
            _target = null;
            Renderer.ResetRenderTarget();
        }
            
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Graphics()
    {
        ReleaseUnmanagedResources();
    }

    public void Clear(Color color)
    {
        Renderer.Clear(color);
    }
}