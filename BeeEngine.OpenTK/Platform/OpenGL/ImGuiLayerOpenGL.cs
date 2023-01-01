using BeeEngine.OpenTK.Events;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace BeeEngine.OpenTK.Gui;

internal sealed class ImGuiLayerOpenGL: ImGuiLayer
{
    private NativeWindow _window;
    private ImGuiController _controller;

    public ImGuiLayerOpenGL()
    {
        _window = CrossPlatformWindow.Instance.GetWindow();
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

    public override void OnBegin()
    {
        _controller.Update(_window, Time.DeltaTime);
    }

    public override void OnGUIRendering()
    {
        ImGui.ShowDemoWindow();
    }

    public override void OnEnd()
    {
        _controller.Render();
    }
    
    public override void OnUpdate()
    {
        _controller.Update(_window, Time.DeltaTime);
    }
}