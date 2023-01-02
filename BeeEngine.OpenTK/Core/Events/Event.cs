namespace BeeEngine.OpenTK.Events;

public enum EventType
{
    None = 0,
    WindowClose,
    WindowResize,
    WindowFocus,
    WindowLostFocus,
    WindowMoved,
    KeyPressed,
    KeyReleased,
    MouseButtonPressed, 
    MouseButtonReleased, 
    MouseMoved, 
    MouseScrolled
}



public abstract class Event
{
    public EventCategory Category { get; protected init; }
    public bool IsHandled { get; internal set; } = false;
}

public readonly ref struct EventDispatcher
{
    private readonly Event _event;
    private readonly Type _type;
    private readonly bool _shouldBeHandled;
    private static readonly List<Type> NonHandleableTypes = new List<Type>();
    public EventDispatcher(Event @event)
    {
        _event = @event;
        _type = @event.GetType();
        _shouldBeHandled = !NonHandleableTypes.Contains(_type);
    }

    public bool IsHandled => _event.IsHandled;

    public bool Dispatch<T>(Func<T, bool> func) where T : Event
    {
        if (typeof(T) != _type)
            return false;
        bool handled = func.Invoke((T)_event);
        if (handled && _shouldBeHandled)
            _event.IsHandled = true;
        return true;
    }

    public static void DontHandle<T>()
    {
        NonHandleableTypes.Add(typeof(T));
    }
} 