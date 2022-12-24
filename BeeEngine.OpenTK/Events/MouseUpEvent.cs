namespace BeeEngine.OpenTK.Events;

public readonly struct MouseUpEvent: IEvent
{
    public readonly MouseButton Button;
    public readonly int X;
    public readonly int Y;

    public MouseUpEvent(MouseButton button, int x, int y)
    {
        Button = button;
        X = x;
        Y = y;
        Category = EventCategory.Mouse |
                   EventCategory.Input |
                   EventCategory.MouseButton;
    }

    public EventCategory Category { get; init; }
}