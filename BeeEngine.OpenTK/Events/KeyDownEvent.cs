using OpenTK.Windowing.GraphicsLibraryFramework;

namespace BeeEngine.OpenTK.Events;

public readonly struct KeyDownEvent: IEvent
{
    public EventCategory Category { get; init; }
    public readonly Key Key;

    public KeyDownEvent(Key key)
    {
        Key = key;
        Category = EventCategory.Input | EventCategory.Keyboard;
    }
}