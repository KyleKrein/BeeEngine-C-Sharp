using OpenTK.Windowing.GraphicsLibraryFramework;

namespace BeeEngine.OpenTK.Events;

public class KeyTypedEvent: Event
{
    public readonly int Key;
    public readonly char KeyChar;

    public KeyTypedEvent(int key)
    {
        Key = key;
        KeyChar = (char) key;
        Category = EventCategory.Input | EventCategory.Keyboard;
    }
}