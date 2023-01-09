using OpenTK.Mathematics;

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

    public static implicit operator Vector2i(Point point)
    {
        return new Vector2i(point.X, point.Y);
    }
}