using BeeEngine.Drawing;

namespace BeeEngine.UI;

public static class Application
{
    private struct WindowStruct
    {
        public readonly Window Window;
        public readonly Renderer Renderer;
        public readonly UIThread UiThread;

        public WindowStruct(Window window)
        {
            Window = window;
            Renderer = new Renderer();
            Window.Renderer = Renderer;
            UiThread = new UIThread(Window);
        }
    }

    private static List<WindowStruct> _windows = new List<WindowStruct>();

    public static void Run<T>() where T : Window
    {
        Window window = Activator.CreateInstance<Window>();
        var windowStruct = new WindowStruct(window);
        _windows.Add(windowStruct);
        windowStruct.Renderer.InitWindow(window.Text, window.Location, window.Size, true);
        windowStruct.UiThread.Update();
    }

    public static void Run(Window window)
    {
        var windowStruct = new WindowStruct(window);
        _windows.Add(windowStruct);
        windowStruct.Renderer.InitWindow(window.Text, window.Location, window.Size, true);
        windowStruct.UiThread.Update();
    }
}