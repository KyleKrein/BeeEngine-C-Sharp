using OpenTK.Windowing.GraphicsLibraryFramework;

namespace BeeEngine.OpenTK.Events;

public class KeyUpEvent: Event
{
    public readonly Key Key;

    public KeyUpEvent(Key key)
    {
        Key = key;
        Category = EventCategory.Input | EventCategory.Keyboard;
    }
}