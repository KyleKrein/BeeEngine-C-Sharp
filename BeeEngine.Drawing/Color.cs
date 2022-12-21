namespace BeeEngine.Drawing;

public readonly struct Color
{
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;
    public readonly byte A;

    public Color(byte r, byte g, byte b, byte a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
    public static Color Yellow { get; } = new Color(255, 255, 0, 255);
    public static Color Blue { get; } = new Color(0, 0, 255, 255);
    public static Color Gray { get; } = new Color(127, 127, 127, 255);
    public static Color Green { get; } = new Color(0, 255, 0, 255);
    public static Color Red { get; } = new Color(255, 0, 0, 255);
    public static Color Magenta { get; } = new Color(255, 0, 255, 255);
    public static Color Transparent { get; } = new Color(0, 0, 0, 0);
}