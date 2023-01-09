using OpenTK.Mathematics;

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

    public static implicit operator Vector2d(PointF point)
    {
        return new Vector2d(point.X, point.Y);
    }
}