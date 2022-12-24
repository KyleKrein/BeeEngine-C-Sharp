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



public interface IEvent
{
    public EventCategory Category { get; init; }
}