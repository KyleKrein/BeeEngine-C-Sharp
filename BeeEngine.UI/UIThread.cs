namespace BeeEngine.UI;

internal sealed class UIThread
{
    private readonly Thread UI;
    private readonly Window _window;

    public UIThread(Window window)
    {
        _window = window;
        UI = Thread.CurrentThread;
    }

    public void Update()
    {
        bool flag = true;
        while (flag)
        {
            flag = _window.Update(_window.Renderer.GetGraphics());
            Thread.Sleep(1);
        }
        _window.Dispose();
    }
}