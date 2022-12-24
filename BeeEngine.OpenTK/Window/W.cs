using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace BeeEngine.OpenTK;

public class W: NativeWindow
{
    public W(string title = "Window", int width = 1280, int height = 720) : base(new NativeWindowSettings()
    {
        Title = title,
        Size = new Vector2i(width, height)
    })
    {
        ProcessWindowEvents(false);
        IsEventDriven = true;
    }
}