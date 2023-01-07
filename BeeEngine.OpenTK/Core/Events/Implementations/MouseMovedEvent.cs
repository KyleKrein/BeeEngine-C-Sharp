namespace BeeEngine.Events;

public class MouseMovedEvent: Event
{
    public readonly float X;
    public readonly float Y;
    public readonly float DeltaX;
    public readonly float DeltaY;

    public MouseMovedEvent(float x, float y, float deltaX, float deltaY)
    {
        X = x;
        Y = y;
        DeltaX = deltaX;
        DeltaY = deltaY;
        Category = EventCategory.Input | EventCategory.Mouse;
    }
}