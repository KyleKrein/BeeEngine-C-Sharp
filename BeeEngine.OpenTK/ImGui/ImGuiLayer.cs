using BeeEngine.OpenTK.Events;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace BeeEngine.OpenTK.Gui;

public class ImGuiLayer : Layer
{
    private GameWindow _window;
    private ImGuiController _controller;

    public ImGuiLayer()
    {
        _window = Game.Instance.GetWindow();
    }

    public override void OnAttach()
    {
        _controller = new ImGuiController(_window.ClientSize.X, _window.ClientSize.Y);
    }

    public override void OnDetach()
    {
        _controller.Dispose();
    }

    public override void OnEvent(EventDispatcher dispatcher)
    {
        dispatcher.Dispatch<MouseScrolledEvent>(OnMouseScrolled);
        dispatcher.Dispatch<KeyTypedEvent>(OnKeyTyped);
        dispatcher.Dispatch<WindowResizedEvent>(OnWindowResized);
    }
    private bool OnMouseScrolled(MouseScrolledEvent e)
    {
        _controller.MouseScroll(new Vector2(e.OffsetHorizontal, e.Offset));
        return false;
    }

    private Vector2 _mouseWheelOffset = Vector2.Zero;
    private bool OnWindowResized(WindowResizedEvent e)
    {
        _controller.WindowResized(e.Width, e.Height);
        return false;
    }
    private bool OnKeyTyped(KeyTypedEvent e)
    {
        _controller.PressChar(e.KeyChar);
        return false;
    }
    public override void OnUpdate()
    {
        _controller.Render();
        _controller.Update(_window, Time.DeltaTime);
    }
}