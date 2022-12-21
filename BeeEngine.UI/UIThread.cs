using BeeEngine.Drawing;

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
        _window.LoadEvent();
        Graphics g = new Graphics();
        while (flag)
        {
            //if(_window.Invalid)
                flag = _window.Update(g);
            Thread.Sleep(1);
        }
        _window.Dispose();
    }
}