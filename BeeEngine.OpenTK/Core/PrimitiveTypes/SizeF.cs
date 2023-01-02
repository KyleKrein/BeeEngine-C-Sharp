namespace BeeEngine.Drawing;

public readonly struct SizeF
{
    public readonly float Width;
    public readonly float Height;

    public SizeF(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public SizeF()
    {
        Width = 0;
        Height = 0;
    }
}