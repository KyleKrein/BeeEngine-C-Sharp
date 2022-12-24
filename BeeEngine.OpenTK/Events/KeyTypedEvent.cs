using OpenTK.Windowing.GraphicsLibraryFramework;

namespace BeeEngine.OpenTK.Events;

public class KeyTypedEvent: Event
{
    public readonly Keys Key;

    public KeyTypedEvent(Keys key)
    {
        Key = key;
        Category = EventCategory.Input | EventCategory.Keyboard;
    }
}