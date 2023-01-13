namespace BeeEngine.Events;

public class KeyDownEvent: Event
{
    public readonly Key Key;

    public KeyDownEvent(Key key)
    {
        Key = key;
        Category = EventCategory.Input | EventCategory.Keyboard;
    }
}