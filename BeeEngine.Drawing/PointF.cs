using SDL2;

namespace BeeEngine.Drawing;

public struct PointF
{
    public float X { get; set; }
    public float Y { get; set; }

    public PointF(float x, float y)
    {
        X = x;
        Y = y;
    }

    public PointF()
    {
        X = 0;
        Y = 0;
    }

    public static implicit operator SDL.SDL_FPoint(PointF point)
    {
        return new SDL.SDL_FPoint()
        {
            x = point.X,
            y = point.Y
        };
    }
}