namespace BeeEngine.OpenTK.Events;

public readonly struct WindowResizedEvent: IEvent
{
    public readonly int Width;
    public readonly int Height;

    public WindowResizedEvent(int width, int height)
    {
        Width = width;
        Height = height;
        Category = EventCategory.Application;
    }

    public EventCategory Category { get; init; }
}