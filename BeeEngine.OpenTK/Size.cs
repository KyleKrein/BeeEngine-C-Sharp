using OpenTK.Mathematics;

namespace BeeEngine.Drawing;

public readonly struct Size
{
    public readonly int Width;
    public readonly int Height;

    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public Size()
    {
        Width = 0;
        Height = 0;
    }
    public static implicit operator Vector2i(Size size)
    {
        return new Vector2i(size.Width, size.Height);
    }
}