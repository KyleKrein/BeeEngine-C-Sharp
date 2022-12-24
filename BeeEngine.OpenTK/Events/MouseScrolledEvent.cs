namespace BeeEngine.OpenTK.Events;

public class MouseScrolledEvent: Event
{
    public readonly float DeltaX;
    public readonly float DeltaY;

    public MouseScrolledEvent(float deltaX, float deltaY)
    {
        DeltaY = deltaY;
        DeltaX = deltaX;
        Category = EventCategory.Input |
                   EventCategory.Mouse;
    }
}