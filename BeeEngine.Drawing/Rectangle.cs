using SDL2;

namespace BeeEngine.Drawing;

public struct Rectangle
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Rectangle()
    {
        X = 0;
        Y = 0;
        Width = 1;
        Height = 1;
    }

    public Rectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public static implicit operator SDL.SDL_Rect(Rectangle rectangle)
    {
        return new SDL.SDL_Rect()
        {
            x = rectangle.X,
            y = rectangle.Y,
            w = rectangle.Width,
            h = rectangle.Height
        };
    }
}