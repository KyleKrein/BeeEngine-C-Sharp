namespace BeeEngine.Events;

public class MouseDownEvent: Event
{
    public readonly MouseButton Button;
    public readonly float X;
    public readonly float Y;

    public MouseDownEvent(MouseButton button, float x, float y)
    {
        Button = button;
        X = x;
        Y = y;
        Category = EventCategory.Mouse |
                   EventCategory.Input |
                   EventCategory.MouseButton;
    }
}