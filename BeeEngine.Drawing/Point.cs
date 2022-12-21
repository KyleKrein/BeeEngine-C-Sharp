using SDL2;

namespace BeeEngine.Drawing;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point()
    {
        X = 0;
        Y = 0;
    }

    public static implicit operator SDL.SDL_Point(Point point)
    {
        return new SDL.SDL_Point()
        {
            x = point.X,
            y = point.Y
        };
    }
}