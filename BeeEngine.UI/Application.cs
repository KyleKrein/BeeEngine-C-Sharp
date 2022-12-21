using BeeEngine.Drawing;

namespace BeeEngine.UI;

public static class Application
{
    private struct WindowStruct
    {
        public readonly Window Window;
        public readonly UIThread UiThread;

        public WindowStruct(Window window)
        {
            Window = window;
            UiThread = new UIThread(Window);
        }
    }

    private static List<WindowStruct> _windows = new List<WindowStruct>();

    public static void Run<T>() where T : Window
    {
        if (!Renderer.IsClosing)
        {
            throw new InvalidOperationException("Can't have more than 1 window opened at the same time");
        }
        Window window = Activator.CreateInstance<Window>();
        var windowStruct = new WindowStruct(window);
        _windows.Add(windowStruct);
        Renderer.InitWindow(window.Text, window.Location, window.Size, true);
        windowStruct.UiThread.Update();
    }

    public static void Run(Window window)
    {
        if (!Renderer.IsClosing)
        {
            throw new InvalidOperationException("Can't have more than 1 window opened at the same time");
        }
        var windowStruct = new WindowStruct(window);
        _windows.Add(windowStruct);
        Renderer.InitWindow(window.Text, window.Location, window.Size, true);
        windowStruct.UiThread.Update();
    }
}