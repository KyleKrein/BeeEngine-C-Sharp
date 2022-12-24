using OpenTK.Windowing.GraphicsLibraryFramework;

namespace BeeEngine.OpenTK.Events;

public readonly struct KeyTypedEvent: IEvent
{
    public EventCategory Category { get; init; }
    public readonly Keys Key;

    public KeyTypedEvent(Keys key)
    {
        Key = key;
        Category = EventCategory.Input | EventCategory.Keyboard;
    }
}