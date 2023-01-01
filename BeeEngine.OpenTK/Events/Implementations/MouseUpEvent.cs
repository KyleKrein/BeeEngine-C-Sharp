namespace BeeEngine.OpenTK.Events;

public class MouseUpEvent: Event
{
    public readonly MouseButton Button;
    public readonly float X;
    public readonly float Y;

    public MouseUpEvent(MouseButton button, float x, float y)
    {
        Button = button;
        X = x;
        Y = y;
        Category = EventCategory.Mouse |
                   EventCategory.Input |
                   EventCategory.MouseButton;
    }
}
public struct MouseUpEventStruct
{
    public readonly MouseButton Button;
    public readonly float X;
    public readonly float Y;
    public EventCategory Category { get; private set; }

    public MouseUpEventStruct(MouseButton button, float x, float y)
    {
        Button = button;
        X = x;
        Y = y;
        Category = EventCategory.Mouse |
                   EventCategory.Input |
                   EventCategory.MouseButton;
    }
}