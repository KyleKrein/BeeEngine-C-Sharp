namespace BeeEngine.OpenTK.Events;

public sealed class MouseClick: Event
{
    public readonly MouseButton Button;
    public readonly int X;
    public readonly int Y;

    public MouseClick(MouseButton button, int x, int y)
    {
        Button = button;
        X = x;
        Y = y;
    }
}