using SDL2;

namespace BeeEngine.Drawing;

public struct RectangleF
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    public RectangleF()
    {
        X = 0;
        Y = 0;
        Width = 1;
        Height = 1;
    }

    public RectangleF(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
    public static implicit operator SDL.SDL_FRect(RectangleF rectangle)
    {
        return new SDL.SDL_FRect()
        {
            x = rectangle.X,
            y = rectangle.Y,
            w = rectangle.Width,
            h = rectangle.Height
        };
    }
}