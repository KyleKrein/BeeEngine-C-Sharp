namespace BeeEngine.Events;

public class KeyUpEvent: Event
{
    public readonly Key Key;

    public KeyUpEvent(Key key)
    {
        Key = key;
        Category = EventCategory.Input | EventCategory.Keyboard;
    }
}