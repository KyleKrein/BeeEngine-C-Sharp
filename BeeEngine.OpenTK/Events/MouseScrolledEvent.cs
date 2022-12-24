namespace BeeEngine.OpenTK.Events;

public readonly struct MouseScrolledEvent: IEvent
{
    public EventCategory Category { get; init; }
    public readonly int Delta;

    public MouseScrolledEvent(int delta)
    {
        Delta = delta;
        Category = EventCategory.Input |
                   EventCategory.Mouse;
    }
}