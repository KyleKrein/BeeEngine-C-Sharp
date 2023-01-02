using BeeEngine.Drawing;
using BeeEngine.OpenTK.Events;

namespace BeeEngine.OpenTK;

public static class Input
{
    private static readonly Dictionary<Key, bool> _keys;
    private static readonly Dictionary<MouseButton, bool> _mouseButtons;
    public static Point MousePosition { get; private set; }
    public static PointF MouseMovedDelta { get; private set; }
    public static float MouseOffset { get; private set; }
    public static float MouseOffsetHorizontal { get; private set; }

    static Input()
    {
        _keys = new Dictionary<Key, bool>();
        _mouseButtons = new Dictionary<MouseButton, bool>();
        MousePosition = default;
        MouseMovedDelta = default;
        MouseOffset = default;
        MouseOffsetHorizontal = default;
        foreach (var k in Enum.GetValues<Key>())
        {
            _keys.Add(k, false);
        }
        foreach (var k in Enum.GetValues<MouseButton>())
        {
            _mouseButtons.Add(k, false);
        }
    }
    public static bool KeyPressed(Key k)
    {
        return _keys[k];
    }

    public static bool MouseKeyPressed(MouseButton m)
    {
        return _mouseButtons[m];
    }

    internal static void OnEvent(Event @event)
    {
        if (!@event.Category.HasFlag(EventCategory.Input))
        {
            return;
        }
        switch (@event)
        {
            case KeyDownEvent e:
                OnKeyPressed(e);
                break;
            case KeyTypedEvent e:
                OnKeyTyped(e);
                break;
            case KeyUpEvent e:
                OnKeyReleased(e);
                break;
            case MouseDownEvent e:
                OnMousePressed(e);
                break;
            case MouseMovedEvent e:
                OnMouseMoved(e);
                break;
            case MouseScrolledEvent e:
                OnMouseScrolled(e);
                break;
            case MouseUpEvent e:
                OnMouseReleased(e);
                break;
        }
    }
    private static void OnMousePressed(MouseDownEvent e)
    {
        _mouseButtons[e.Button] = true;
    }
    private static void OnMouseReleased(MouseUpEvent e)
    {
        _mouseButtons[e.Button] = false;
    }
    private static void OnMouseMoved(MouseMovedEvent e)
    {
        MousePosition = new Point((int) e.X, (int) e.Y);
        MouseMovedDelta = new PointF(e.DeltaX, e.DeltaY);
    }
    private static void OnMouseScrolled(MouseScrolledEvent e)
    {
        MouseOffset = e.Offset;
        MouseOffsetHorizontal = e.OffsetHorizontal;
    }
    private static void OnKeyPressed(KeyDownEvent e)
    {
        _keys[e.Key] = true;
    }
    private static void OnKeyReleased(KeyUpEvent e)
    {
        _keys[e.Key] = false;
    }
    private static void OnKeyTyped(KeyTypedEvent e)
    {
        //?
    }
}