namespace BeeEngine.Events;

public sealed class WindowClosedEvent: Event
{
    public WindowClosedEvent()
    {
        Category = EventCategory.Application;
    }
}