namespace BeeEngine.Events;

public class MouseScrolledEvent: Event
{
    public readonly float Offset;
    public readonly float OffsetHorizontal;

    public MouseScrolledEvent(float offsetVertical, float offsetHorizontal)
    {
        Category = EventCategory.Input |
                   EventCategory.Mouse;
        Offset = offsetVertical;
        OffsetHorizontal = offsetHorizontal;

    }
}