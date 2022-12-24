namespace BeeEngine.OpenTK.Events;

[Flags]
public enum EventCategory
{
    None = 0,
    Application = 1,
    Input = 2,
    Keyboard = 4,
    Mouse = 8,
    MouseButton = 16
}