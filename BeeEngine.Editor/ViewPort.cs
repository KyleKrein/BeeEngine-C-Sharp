using System.Numerics;
using BeeEngine.Events;
using BeeEngine.SmartPointers;
using ImGuiNET;

namespace BeeEngine.Editor;

public sealed class ViewPort
{
    private FrameBuffer _frameBuffer;
    private uint _width, _height;
    private SharedPointer<FrameBufferPreferences> _preferences;
    private OrthographicCameraController _orthographicCameraController;
    public ViewPort(int width, int height, bool ResizeOnWindow = true)
    {
        _width = (uint) width;
        _height = (uint) height;
        var frameBufferPreferences = new FrameBufferPreferences(_width, _height);
        _preferences = new SharedPointer<FrameBufferPreferences>(ref frameBufferPreferences);
        _frameBuffer = FrameBuffer.Create(_preferences.Share());
        _orthographicCameraController = new OrthographicCameraController();
    }

    public void OnEvent(ref EventDispatcher e)
    {
        if(e.Category.HasFlag(EventCategory.Application))
            return;
        _orthographicCameraController.OnEvent(ref e);
    }
    public required Action Func;
    public void Update()
    {
        _frameBuffer.Bind();
        _orthographicCameraController.OnUpdate();
        RenderCommand.Clear();
        Renderer2D.BeginScene(_orthographicCameraController);
        Func.Invoke();
        Renderer2D.EndScene();
        _frameBuffer.Unbind();
    }

    public void Render()
    {
        _frameBuffer.Bind();
        ImGui.Begin("ViewPort");
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
        var size = ImGui.GetContentRegionAvail();
        if (_width != size.X || _height != size.Y)
        {
            _width = (uint) size.X;
            _height = (uint) size.Y;
            _preferences.GetRef().Width = _width;
            _preferences.GetRef().Height = _height;
            _frameBuffer.Invalidate();
            var sizeChanged = new WindowResizedEvent((int) _width, (int) _height);
            EventDispatcher e = new EventDispatcher(sizeChanged);
            _orthographicCameraController.OnEvent(ref e);
        }
        ImGui.Image(_frameBuffer.ColorAttachment, new Vector2(_width, _height), new Vector2(0, 1), new Vector2(1, 0));
        ImGui.PopStyleVar();
        ImGui.End();
        _frameBuffer.Unbind();
    }
}