namespace BeeEngine.OpenTK.Events;

public readonly struct MouseMovedEvent: IEvent
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

    public EventCategory Category { get; init; }
}