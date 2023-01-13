using BeeEngine.Events;
using BeeEngine.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace BeeEngine;

internal enum WindowHandlerType
{
    GLFW,
    SDL
}
internal abstract class WindowHandler: IDisposable
{
    public static WindowHandlerType Type { get; private set; }
    public abstract int Width { get; set; }
    public abstract int Height { get; set; }
    public abstract VSync VSync { get; set; }
    public abstract string Title { get; set; }

    public static WindowHandler Create(WindowHandlerType type, ref WindowProps preferences, EventQueue eventQueue)
    {
        Type = type;
        switch (type)
        {
            case WindowHandlerType.GLFW:
                return new GLFWWindowHandler(ref preferences, eventQueue);
            case WindowHandlerType.SDL:
                throw new PlatformNotSupportedException("SDL is still not supported :(");
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public abstract void ProcessEvents();

    public abstract void SwapBuffers();
    public abstract void MakeContextCurrent();
    public abstract void MakeContextNonCurrent();

    protected abstract void Dispose(bool disposing);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~WindowHandler()
    {
        Dispose(false);
    }

    public abstract bool IsRunning();
}

internal unsafe sealed class GLFWWindowHandler: WindowHandler
{
    public override int Width
    {
        get => _width;
        set
        {
            GLFW.SetWindowSize(_windowHandle,value, _height);
            _width = value;
        }
    }

    public override int Height
    {
        get => _height;
        set
        {
            GLFW.SetWindowSize(_windowHandle,_width, value);
            _height = value;
        }
    }

    public override string Title
    {
        get => _title;
        set
        {
            GLFW.SetWindowTitle(_windowHandle, value);
            _title = value;
        }
    }

    public override VSync VSync
    {
        get => _vSync;
        set
        {
            switch (value)
            {
                case VSync.On:
                    GLFW.SwapInterval(1);
                    break;
                case VSync.Off:
                    GLFW.SwapInterval(0);
                    break;
            }
            _vSync = value;
        }
    }
    
    private  Vector2 mouseCoords = Vector2.Zero;

    private global::OpenTK.Windowing.GraphicsLibraryFramework.Window* _windowHandle;
    private EventQueue _events;
    private VSync _vSync;
    private int _width;
    private int _height;
    private string _title;
    private bool _isRunning = false;

    public GLFWWindowHandler(ref WindowProps preferences, EventQueue events)
    {
        _events = events;
        GLFW.SetErrorCallback((error, description) => Log.Error("GLFW Error {0}: {1}", error, description));
        if (!GLFW.Init())
        {
            Log.Error("GLFW Init failed!");
        }
        
        GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlApi);
        GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core );
        if (Application.PlatformOS == OS.Mac)
        {
            GLFW.WindowHint(WindowHintBool.OpenGLForwardCompat, true);
            GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
            GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 1);
        }
        else
        {
            GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
            GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 5);
        }
        _windowHandle = GLFW.CreateWindow(preferences.Width, preferences.Height, preferences.Title, null, null);
        GLFW.MakeContextCurrent(_windowHandle);
        GL.LoadBindings(new GLFWBindingsContext());
        //GL = Silk.NET.OpenGL.GL.GetApi(s => GLFW.GetProcAddress(s));
        //GLFW.ShowWindow(_windowHandle);
        _isRunning = true;
        switch (preferences.VSync)
        {
            case VSync.On:
                GLFW.SwapInterval(1);
                break;
            case VSync.Off:
                GLFW.SwapInterval(0);
                break;
        }
        GLFW.SetWindowTitle(_windowHandle, preferences.Title);
        GLFW.SetWindowSize(_windowHandle, preferences.Width, preferences.Height);
        _title = preferences.Title;
        _width = preferences.Width;
        _height = preferences.Height;
        GLFW.SetCharCallback(_windowHandle, CharCallback);
        //TODO: GLFW.SetDropCallback(_windowHandle, )
        GLFW.SetErrorCallback((error, description) => Log.Error("GLFW Error {0}: {1}", error, description));
        //TODO: GLFW.SetJoystickCallback()
        GLFW.SetKeyCallback(_windowHandle, KeyCallback);
        GLFW.SetScrollCallback(_windowHandle, ScrollCallback);
        GLFW.SetCursorPosCallback(_windowHandle, MouseMovedCallback);
        GLFW.SetFramebufferSizeCallback(_windowHandle, FrameBufferResizeCallback);
        GLFW.SetMouseButtonCallback(_windowHandle, MouseButtonCallback);
        GLFW.SetWindowCloseCallback(_windowHandle, WindowCloseCallback);
        GLFW.SetWindowSizeCallback(_windowHandle, WindowResizeCallback);
        GLFW.SetWindowPosCallback(_windowHandle, WindowPosChangedCallback);
        GLFW.SetWindowRefreshCallback(_windowHandle, RefreshCallback);
    }

    private void CharCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window1, uint codepoint)
    {
        _events.AddEvent(new KeyTypedEvent((int) codepoint));
    }

    private void RefreshCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window1)
    {
        
    }

    private void WindowPosChangedCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window1, int x, int y)
    {
        
    }

    private void WindowResizeCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window1, int width, int height)
    {
        
    }

    private void WindowCloseCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window1)
    {
        _isRunning = false;
        _events.AddEvent(new WindowClosedEvent());
    }

    private void MouseButtonCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window1, global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton mouseButton, InputAction action, KeyModifiers mods)
    {
        switch (action)
        {
            case InputAction.Press:
                _events.AddEvent(new MouseDownEvent(ConvertGLFWMouseButton(mouseButton), mouseCoords.X, mouseCoords.Y));
                break;
            case InputAction.Release:
                _events.AddEvent(new MouseUpEvent(ConvertGLFWMouseButton(mouseButton), mouseCoords.X, mouseCoords.Y));
                break;
            case InputAction.Repeat:
                //TODO
                break;
        }
    }

    private MouseButton ConvertGLFWMouseButton(global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton button)
    {
        switch (button)
        {
            case global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left:
                return MouseButton.Left;
            case global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Right:
                return MouseButton.Right;
            case global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Middle:
                return MouseButton.Middle;
            case global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Button4:
                return MouseButton.Button4;
            case global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Button5:
                return MouseButton.Button5;
            case global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Button6:
                return MouseButton.Button6;
            case global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Button7:
                return MouseButton.Button7;
            case global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Button8:
                return MouseButton.Button8;
            default:
                throw new ArgumentOutOfRangeException(nameof(button), button, null);
        }
    }

    private void FrameBufferResizeCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window1, int width, int height)
    {
        
    }

    private void MouseMovedCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window1, double x, double y)
    {
        _events.AddEvent(new MouseMovedEvent((float) x, (float) y, (float) (x - mouseCoords.X), (float) (y - mouseCoords.Y)));
        mouseCoords = new Vector2((float) x, (float) y);
    }

    private void ScrollCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window1, double offsetx, double offsety)
    {
        _events.AddEvent(new MouseScrolledEvent((float) offsety, (float) offsetx));
    }

    private void KeyCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window1, Keys key, int scancode, InputAction action, KeyModifiers mods)
    {
        switch (action)
        {
            case InputAction.Press:
                _events.AddEvent(new KeyDownEvent(ConvertKeyCodes(key)));
                break;
            case InputAction.Release:
                _events.AddEvent(new KeyUpEvent(ConvertKeyCodes(key)));
                break;
            case InputAction.Repeat:
                //TODO
                break;
        }
        
    }

    private Key ConvertKeyCodes(Keys key)
    {
        return key switch
        {
            Keys.Unknown => Key.Unknown,
            Keys.Space => Key.Space,
            Keys.Apostrophe => Key.Apostrophe,
            Keys.Comma => Key.Comma,
            Keys.Minus => Key.Minus,
            Keys.Period => Key.Period,
            Keys.Slash => Key.Slash,
            Keys.D0 => Key.D0,
            Keys.D1 => Key.D1,
            Keys.D2 => Key.D2,
            Keys.D3 => Key.D3,
            Keys.D4 => Key.D4,
            Keys.D5 => Key.D5,
            Keys.D6 => Key.D6,
            Keys.D7 => Key.D7,
            Keys.D8 => Key.D8,
            Keys.D9 => Key.D9,
            Keys.Semicolon => Key.Semicolon,
            Keys.Equal => Key.Equal,
            Keys.A => Key.A,
            Keys.B => Key.B,
            Keys.C => Key.C,
            Keys.D => Key.D,
            Keys.E => Key.E,
            Keys.F => Key.F,
            Keys.G => Key.G,
            Keys.H => Key.H,
            Keys.I => Key.I,
            Keys.J => Key.J,
            Keys.K => Key.K,
            Keys.L => Key.L,
            Keys.M => Key.M,
            Keys.N => Key.N,
            Keys.O => Key.O,
            Keys.P => Key.P,
            Keys.Q => Key.Q,
            Keys.R => Key.R,
            Keys.S => Key.S,
            Keys.T => Key.T,
            Keys.U => Key.U,
            Keys.V => Key.V,
            Keys.W => Key.W,
            Keys.X => Key.X,
            Keys.Y => Key.Y,
            Keys.Z => Key.Z,
            Keys.LeftBracket => Key.LeftBracket,
            Keys.Backslash => Key.Backslash,
            Keys.RightBracket => Key.RightBracket,
            Keys.GraveAccent => Key.GraveAccent,
            Keys.Escape => Key.Escape,
            Keys.Enter => Key.Enter,
            Keys.Tab => Key.Tab,
            Keys.Backspace => Key.Backspace,
            Keys.Insert => Key.Insert,
            Keys.Delete => Key.Delete,
            Keys.Right => Key.Right,
            Keys.Left => Key.Left,
            Keys.Down => Key.Down,
            Keys.Up => Key.Up,
            Keys.PageUp => Key.PageUp,
            Keys.PageDown => Key.PageDown,
            Keys.Home => Key.Home,
            Keys.End => Key.End,
            Keys.CapsLock => Key.CapsLock,
            Keys.ScrollLock => Key.ScrollLock,
            Keys.NumLock => Key.NumLock,
            Keys.PrintScreen => Key.PrintScreen,
            Keys.Pause => Key.Pause,
            Keys.F1 => Key.F1,
            Keys.F2 => Key.F2,
            Keys.F3 => Key.F3,
            Keys.F4 => Key.F4,
            Keys.F5 => Key.F5,
            Keys.F6 => Key.F6,
            Keys.F7 => Key.F7,
            Keys.F8 => Key.F8,
            Keys.F9 => Key.F9,
            Keys.F10 => Key.F10,
            Keys.F11 => Key.F11,
            Keys.F12 => Key.F12,
            Keys.F13 => Key.F13,
            Keys.F14 => Key.F14,
            Keys.F15 => Key.F15,
            Keys.F16 => Key.F16,
            Keys.F17 => Key.F17,
            Keys.F18 => Key.F18,
            Keys.F19 => Key.F19,
            Keys.F20 => Key.F20,
            Keys.F21 => Key.F21,
            Keys.F22 => Key.F22,
            Keys.F23 => Key.F23,
            Keys.F24 => Key.F24,
            Keys.F25 => Key.F25,
            Keys.KeyPad0 => Key.KeyPad0,
            Keys.KeyPad1 => Key.KeyPad1,
            Keys.KeyPad2 => Key.KeyPad2,
            Keys.KeyPad3 => Key.KeyPad3,
            Keys.KeyPad4 => Key.KeyPad4,
            Keys.KeyPad5 => Key.KeyPad5,
            Keys.KeyPad6 => Key.KeyPad6,
            Keys.KeyPad7 => Key.KeyPad7,
            Keys.KeyPad8 => Key.KeyPad8,
            Keys.KeyPad9 => Key.KeyPad9,
            Keys.KeyPadDecimal => Key.KeyPadDecimal,
            Keys.KeyPadDivide => Key.KeyPadDivide,
            Keys.KeyPadMultiply => Key.KeyPadMultiply,
            Keys.KeyPadSubtract => Key.KeyPadSubtract,
            Keys.KeyPadAdd => Key.KeyPadAdd,
            Keys.KeyPadEnter => Key.KeyPadEnter,
            Keys.KeyPadEqual => Key.KeyPadEqual,
            Keys.LeftShift => Key.LeftShift,
            Keys.LeftControl => Key.LeftControl,
            Keys.LeftAlt => Key.LeftAlt,
            Keys.LeftSuper => Key.LeftSuper,
            Keys.RightShift => Key.RightShift,
            Keys.RightControl => Key.RightControl,
            Keys.RightAlt => Key.RightAlt,
            Keys.RightSuper => Key.RightSuper,
            _ => Key.Unknown
        };
    }

    public override void ProcessEvents()
    {
        GLFW.PollEvents();
    }

    public override void SwapBuffers()
    {
        GLFW.SwapBuffers(_windowHandle);
    }

    public override void MakeContextCurrent()
    {
        GLFW.MakeContextCurrent(_windowHandle);
    }

    public override void MakeContextNonCurrent()
    {
        GLFW.MakeContextCurrent(null);
    }

    protected override void Dispose(bool disposing)
    {
        GLFW.DestroyWindow(_windowHandle);
        GLFW.Terminate();
    }

    public override bool IsRunning()
    {
        return _isRunning;
    }
}