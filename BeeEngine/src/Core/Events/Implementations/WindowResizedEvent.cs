namespace BeeEngine.Events;

public class WindowResizedEvent: Event
{
    public readonly int Width;
    public readonly int Height;

    public WindowResizedEvent(int width, int height)
    {
        Width = width;
        Height = height;
        Category = EventCategory.Application;
    }
}