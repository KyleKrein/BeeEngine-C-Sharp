/*using System.Numerics;
using System.Runtime.InteropServices;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;

namespace BeeEngine.OpenTK.Gui;

internal static class ImGuiInit
{
    public static void ImplGlfwInitForOpenGL(IntPtr window, bool installCallbacks)
    {
        var io = ImGui.GetIO();
    }

    public static void ImplOpenGL3_Init(string version)
    {
        
    }
    
    // GLFW data
enum GlfwClientApi
{
    GlfwClientApi_Unknown,
    GlfwClientApi_OpenGL,
    GlfwClientApi_Vulkan
};

struct ImGui_ImplGlfw_Data
{
    public unsafe void*             Window;
    GlfwClientApi           ClientApi;
    public double                  Time;
    public unsafe void*             MouseWindow;
    public IntPtr[]             MouseCursors = new IntPtr[11];
    public Vector2                  LastValidMousePos;
    public bool                    InstalledCallbacks;

    // Chain GLFW callbacks: our callbacks will call the user's previously installed callbacks, if any.
    public GLFWCallbacks.WindowFocusCallback      PrevUserCallbackWindowFocus;
    public GLFWCallbacks.CursorPosCallback        PrevUserCallbackCursorPos;
    public GLFWCallbacks.CursorEnterCallback      PrevUserCallbackCursorEnter;
    public GLFWCallbacks.MouseButtonCallback      PrevUserCallbackMousebutton;
    public GLFWCallbacks.ScrollCallback           PrevUserCallbackScroll;
    public GLFWCallbacks.KeyCallback              PrevUserCallbackKey;
    public GLFWCallbacks.CharCallback             PrevUserCallbackChar;
    public GLFWCallbacks.MonitorCallback          PrevUserCallbackMonitor;

    public ImGui_ImplGlfw_Data()   {
        unsafe
        {
            memset(&this, 0, (ulong) sizeof(ImGui_ImplGlfw_Data));
        }
    }
};
/// <summary>
/// .NET wrapper to native call of 'memset'. Requires Microsoft Visual C++ Runtime installed
/// </summary>
[DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
public static extern unsafe IntPtr memset(void* dest, int value, ulong count);
// Backend data stored in io.BackendPlatformUserData to allow support for multiple Dear ImGui contexts
// It is STRONGLY preferred that you use docking branch with multi-viewports (== single Dear ImGui context + multiple windows) instead of multiple Dear ImGui contexts.
// FIXME: multi-context support is not well tested and probably dysfunctional in this backend.
// - Because glfwPollEvents() process all windows and some events may be called outside of it, you will need to register your own callbacks
//   (passing install_callbacks=false in ImGui_ImplGlfw_InitXXX functions), set the current dear imgui context and then call our callbacks.
// - Otherwise we may need to store a GLFWWindow* -> ImGuiContext* map and handle this in the backend, adding a little bit of extra complexity to it.
// FIXME: some shared resources (mouse cursor shape, gamepad) are mishandled when using multi-context.
static unsafe ImGui_ImplGlfw_Data* ImGui_ImplGlfw_GetBackendData()
{
    return ImGuiNative.igGetCurrentContext() != IntPtr.Zero ? (ImGui_ImplGlfw_Data*)ImGuiNET.ImGuiNative.igGetIO()->BackendPlatformUserData: null;
}

// Functions
static unsafe nint ImGui_ImplGlfw_GetClipboardText(void* user_data)
{
    return GLFW.GetClipboardString((global::OpenTK.Windowing.GraphicsLibraryFramework.Window*) user_data);
}

static unsafe nint ImGui_ImplGlfw_SetClipboardText(void* user_data, string text)
{
    GLFW.SetClipboardString((global::OpenTK.Windowing.GraphicsLibraryFramework.Window*) user_data, text);
}

static ImGuiKey ImGui_ImplGlfw_KeyToImGuiKey(Keys key)
{
    switch (key)
    {
        case Keys.Unknown:
            return ImGuiKey.None;
        case Keys.Space:
            return ImGuiKey.None;
        case Keys.Apostrophe:
            return ImGuiKey.None;
        case Keys.Comma:
            return ImGuiKey.None;
        case Keys.Minus:
            return ImGuiKey.None;
        case Keys.Period:
            return ImGuiKey.Period;

        case Keys.Slash:
            return ImGuiKey.Slash;

        case Keys.D0:
            return ImGuiKey._0;

        case Keys.D1:
            return ImGuiKey._1;

        case Keys.D2:
            return ImGuiKey._2;

        case Keys.D3:
            return ImGuiKey._3;

        case Keys.D4:
            return ImGuiKey._4;

        case Keys.D5:
            return ImGuiKey._5;

        case Keys.D6:
            return ImGuiKey._6;

        case Keys.D7:
            return ImGuiKey._7;

        case Keys.D8:
            return ImGuiKey._8;

        case Keys.D9:
            return ImGuiKey._9;

        case Keys.Semicolon:
            return ImGuiKey.Semicolon;

        case Keys.Equal:
            return ImGuiKey.Equal;

        case Keys.A:
            return ImGuiKey.A;

        case Keys.B:
            return ImGuiKey.B;

        case Keys.C:
            return ImGuiKey.C;

        case Keys.D:
            return ImGuiKey.D;

        case Keys.E:
            return ImGuiKey.E;

        case Keys.F:
            return ImGuiKey.F;

        case Keys.G:
            return ImGuiKey.G;

        case Keys.H:
            return ImGuiKey.H;

        case Keys.I:
            return ImGuiKey.I;

        case Keys.J:
            return ImGuiKey.J;

        case Keys.K:
            return ImGuiKey.K;

        case Keys.L:
            return ImGuiKey.L;

        case Keys.M:
            return ImGuiKey.M;

        case Keys.N:
            return ImGuiKey.N;

        case Keys.O:
            return ImGuiKey.O;

        case Keys.P:
            return ImGuiKey.O;

        case Keys.Q:
            return ImGuiKey.Q;

        case Keys.R:
            return ImGuiKey.R;

        case Keys.S:
            return ImGuiKey.S;

        case Keys.T:
            return ImGuiKey.T;

        case Keys.U:
            return ImGuiKey.U;

        case Keys.V:
            return ImGuiKey.V;

        case Keys.W:
            return ImGuiKey.W;

        case Keys.X:
            return ImGuiKey.X;

        case Keys.Y:
            return ImGuiKey.Y;

        case Keys.Z:
            return ImGuiKey.Z;

        case Keys.LeftBracket:
            return ImGuiKey.LeftBracket;

        case Keys.Backslash:
            return ImGuiKey.Backslash;

        case Keys.RightBracket:
            return ImGuiKey.RightBracket;

        case Keys.GraveAccent:
            return ImGuiKey.GraveAccent;

        case Keys.Escape:
            return ImGuiKey.Escape;

        case Keys.Enter:
            return ImGuiKey.Enter;

        case Keys.Tab:
            return ImGuiKey.Tab;

        case Keys.Backspace:
            return ImGuiKey.Backspace;

        case Keys.Insert:
            return ImGuiKey.Insert;

        case Keys.Delete:
            return ImGuiKey.Delete;

        case Keys.Right:
            return ImGuiKey.RightArrow;

        case Keys.Left:
            return ImGuiKey.LeftArrow;

        case Keys.Down:
            return ImGuiKey.DownArrow;

        case Keys.Up:
            return ImGuiKey.UpArrow;

        case Keys.PageUp:
            return ImGuiKey.PageUp;

        case Keys.PageDown:
            return ImGuiKey.PageDown;

        case Keys.Home:
            return ImGuiKey.Home;

        case Keys.End:
            return ImGuiKey.End;

        case Keys.CapsLock:
            return ImGuiKey.CapsLock;

        case Keys.ScrollLock:
            return ImGuiKey.ScrollLock;

        case Keys.NumLock:
            return ImGuiKey.NumLock;

        case Keys.PrintScreen:
            return ImGuiKey.PrintScreen;

        case Keys.Pause:
            return ImGuiKey.Pause;

        case Keys.F1:
            return ImGuiKey.F1;

        case Keys.F2:
            return ImGuiKey.F2;

        case Keys.F3:
            return ImGuiKey.F3;

        case Keys.F4:
            return ImGuiKey.F4;

        case Keys.F5:
            return ImGuiKey.F5;

        case Keys.F6:
            return ImGuiKey.F6;

        case Keys.F7:
            return ImGuiKey.F7;

        case Keys.F8:
            return ImGuiKey.F8;

        case Keys.F9:
            return ImGuiKey.F9;

        case Keys.F10:
            return ImGuiKey.F10;

        case Keys.F11:
            return ImGuiKey.F11;

        case Keys.F12:
            return ImGuiKey.F12;

        case Keys.KeyPad0:
            return ImGuiKey.Keypad0;

        case Keys.KeyPad1:
            return ImGuiKey.Keypad1;

        case Keys.KeyPad2:
            return ImGuiKey.Keypad2;

        case Keys.KeyPad3:
            return ImGuiKey.Keypad3;

        case Keys.KeyPad4:
            return ImGuiKey.Keypad4;

        case Keys.KeyPad5:
            return ImGuiKey.Keypad5;

        case Keys.KeyPad6:
            return ImGuiKey.Keypad6;

        case Keys.KeyPad7:
            return ImGuiKey.Keypad7;

        case Keys.KeyPad8:
            return ImGuiKey.Keypad8;

        case Keys.KeyPad9:
            return ImGuiKey.Keypad9;

        case Keys.KeyPadDecimal:
            return ImGuiKey.KeypadDecimal;

        case Keys.KeyPadDivide:
            return ImGuiKey.KeypadDivide;

        case Keys.KeyPadMultiply:
            return ImGuiKey.KeypadMultiply;

        case Keys.KeyPadSubtract:
            return ImGuiKey.KeypadSubtract;

        case Keys.KeyPadAdd:
            return ImGuiKey.KeypadAdd;

        case Keys.KeyPadEnter:
            return ImGuiKey.KeypadEnter;

        case Keys.KeyPadEqual:
            return ImGuiKey.KeypadEqual;

        case Keys.LeftShift:
            return ImGuiKey.LeftShift;

        case Keys.LeftControl:
            return ImGuiKey.LeftCtrl;

        case Keys.LeftAlt:
            return ImGuiKey.LeftAlt;

        case Keys.LeftSuper:
            return ImGuiKey.LeftSuper;

        case Keys.RightShift:
            return ImGuiKey.RightShift;

        case Keys.RightControl:
            return ImGuiKey.RightCtrl;

        case Keys.RightAlt:
            return ImGuiKey.RightAlt;

        case Keys.RightSuper:
            return ImGuiKey.RightSuper;

        case Keys.Menu:
            return ImGuiKey.Menu;

        default:
            return ImGuiKey.None;
    }
}

static KeyModifiers ImGui_ImplGlfw_KeyToModifier(Keys key)
{
    if (key == Keys.LeftControl || key == Keys.RightControl)
        return KeyModifiers.Control;
    if (key == Keys.LeftShift || key == Keys.RightShift)
        return KeyModifiers.Shift;
    if (key == Keys.LeftAlt || key == Keys.RightAlt)
        return KeyModifiers.Alt;
    if (key == Keys.LeftSuper || key == Keys.RightSuper)
        return KeyModifiers.Super;
    return 0;
}

static void ImGui_ImplGlfw_UpdateKeyModifiers(KeyModifiers mods)
{
    ImGuiIOPtr io = ImGui.GetIO();
    io.AddKeyEvent(ImGuiKey.ImGuiMod_Ctrl, (mods & KeyModifiers.Control) != 0);
    io.AddKeyEvent(ImGuiKey.ImGuiMod_Shift, (mods & KeyModifiers.Shift) != 0);
    io.AddKeyEvent(ImGuiKey.ImGuiMod_Alt, (mods & KeyModifiers.Alt) != 0);
    io.AddKeyEvent(ImGuiKey.ImGuiMod_Super, (mods & KeyModifiers.Super) != 0);
}

static unsafe void ImGui_ImplGlfw_MouseButtonCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window, MouseButton button, InputAction action, KeyModifiers mods)
{
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();
    if (bd->PrevUserCallbackMousebutton != null && window == bd->Window)
        bd->PrevUserCallbackMousebutton(window, button, action, mods);

    ImGui_ImplGlfw_UpdateKeyModifiers(mods);

    var io = ImGui.GetIO();
    if (button is >= 0 and < (MouseButton) 8)
        io.AddMouseButtonEvent((int) button, action == InputAction.Press);
}

static unsafe void ImGui_ImplGlfw_ScrollCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window, double xoffset, double yoffset)
{
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();
    if (bd->PrevUserCallbackScroll != null && window == bd->Window)
        bd->PrevUserCallbackScroll(window, xoffset, yoffset);

    var io = ImGui.GetIO();
    io.AddMouseWheelEvent((float)xoffset, (float)yoffset);
}

/*static int ImGui_ImplGlfw_TranslateUntranslatedKey(int key, int scancode)
{
#if GLFW_HAS_GETKEYNAME && !defined(__EMSCRIPTEN__)
    // GLFW 3.1+ attempts to "untranslate" keys, which goes the opposite of what every other framework does, making using lettered shortcuts difficult.
    // (It had reasons to do so: namely GLFW is/was more likely to be used for WASD-type game controls rather than lettered shortcuts, but IHMO the 3.1 change could have been done differently)
    // See https://github.com/glfw/glfw/issues/1502 for details.
    // Adding a workaround to undo this (so our keys are translated->untranslated->translated, likely a lossy process).
    // This won't cover edge cases but this is at least going to cover common cases.
    if (key >= GLFW_KEY_KP_0 && key <= GLFW_KEY_KP_EQUAL)
        return key;
    GLFWerrorfun prev_error_callback = glfwSetErrorCallback(nullptr);
    const char* key_name = glfwGetKeyName(key, scancode);
    glfwSetErrorCallback(prev_error_callback);
#if (GLFW_VERSION_COMBINED >= 3300) // Eat errors (see #5908)
    (void)glfwGetError(NULL);
#endif
    if (key_name && key_name[0] != 0 && key_name[1] == 0)
    {
        const char char_names[] = "`-=[]\\,;\'./";
        const int char_keys[] = { GLFW_KEY_GRAVE_ACCENT, GLFW_KEY_MINUS, GLFW_KEY_EQUAL, GLFW_KEY_LEFT_BRACKET, GLFW_KEY_RIGHT_BRACKET, GLFW_KEY_BACKSLASH, GLFW_KEY_COMMA, GLFW_KEY_SEMICOLON, GLFW_KEY_APOSTROPHE, GLFW_KEY_PERIOD, GLFW_KEY_SLASH, 0 };
        IM_ASSERT(IM_ARRAYSIZE(char_names) == IM_ARRAYSIZE(char_keys));
        if (key_name[0] >= '0' && key_name[0] <= '9')               { key = GLFW_KEY_0 + (key_name[0] - '0'); }
        else if (key_name[0] >= 'A' && key_name[0] <= 'Z')          { key = GLFW_KEY_A + (key_name[0] - 'A'); }
        else if (key_name[0] >= 'a' && key_name[0] <= 'z')          { key = GLFW_KEY_A + (key_name[0] - 'a'); }
        else if (const char* p = strchr(char_names, key_name[0]))   { key = char_keys[p - char_names]; }
    }
    // if (action == GLFW_PRESS) printf("key %d scancode %d name '%s'\n", key, scancode, key_name);
#else
    IM_UNUSED(scancode);
#endif
    return key;
}#1#

static unsafe void ImGui_ImplGlfw_KeyCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window, Keys keycode, int scancode, InputAction action, KeyModifiers mods)
{
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();
    if (bd->PrevUserCallbackKey != null && window == bd->Window)
        bd->PrevUserCallbackKey(window, keycode, scancode, action, mods);

    if (action != InputAction.Press && action != InputAction.Release)
        return;
    KeyModifiers keycode_to_mod = ImGui_ImplGlfw_KeyToModifier(keycode);
    // Workaround: X11 does not include current pressed/released modifier key in 'mods' flags. https://github.com/glfw/glfw/issues/1630
    if (keycode_to_mod != 0)
        mods = (action == InputAction.Press) ? (mods | keycode_to_mod) : (mods & ~keycode_to_mod);
    ImGui_ImplGlfw_UpdateKeyModifiers(mods);

    //keycode = ImGui_ImplGlfw_TranslateUntranslatedKey(keycode, scancode);
    //var zh = ImGui_ImplGlfw_KeyToImGuiKey(keycode);
    var io = ImGui.GetIO();
    ImGuiKey imgui_key = ImGui_ImplGlfw_KeyToImGuiKey(keycode);
    io.AddKeyEvent(imgui_key, (action == InputAction.Press));
    io.SetKeyEventNativeData(imgui_key, (int) keycode, scancode); // To support legacy indexing (<1.87 user code)
}

static unsafe void ImGui_ImplGlfw_WindowFocusCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window, bool focused)
{
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();
    if (bd->PrevUserCallbackWindowFocus != null && window == bd->Window)
        bd->PrevUserCallbackWindowFocus(window, focused);

    var io = ImGui.GetIO();
    io.AddFocusEvent(focused);
}

static unsafe void ImGui_ImplGlfw_CursorPosCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window, double x, double y)
{
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();
    if (bd->PrevUserCallbackCursorPos != null && window == bd->Window)
        bd->PrevUserCallbackCursorPos(window, x, y);
    if (GLFW.GetInputMode(window, CursorStateAttribute.Cursor) == CursorModeValue.CursorDisabled)
        return;

    var io = ImGui.GetIO();
    io.AddMousePosEvent((float)x, (float)y);
    bd->LastValidMousePos = new Vector2((float)x, (float)y);
}

// Workaround: X11 seems to send spurious Leave/Enter events which would make us lose our position,
// so we back it up and restore on Leave/Enter (see https://github.com/ocornut/imgui/issues/4984)
static unsafe void ImGui_ImplGlfw_CursorEnterCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window, bool entered)
{
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();
    if (bd->PrevUserCallbackCursorEnter != null && window == bd->Window)
        bd->PrevUserCallbackCursorEnter(window, entered);
    if (GLFW.GetInputMode(window, CursorStateAttribute.Cursor) == CursorModeValue.CursorDisabled)
        return;

    var io = ImGui.GetIO();
    if (entered)
    {
        bd->MouseWindow = window;
        io.AddMousePosEvent(bd->LastValidMousePos.X, bd->LastValidMousePos.Y);
    }
    else if (!entered && bd->MouseWindow == window)
    {
        bd->LastValidMousePos = io.MousePos;
        bd->MouseWindow = null;
        io.AddMousePosEvent(-float.MaxValue, -float.MaxValue);
    }
}

static unsafe void ImGui_ImplGlfw_CharCallback(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window, uint c)
{
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();
    if (bd->PrevUserCallbackChar != null && window == bd->Window)
        bd->PrevUserCallbackChar(window, c);

    var io = ImGui.GetIO();
    io.AddInputCharacter(c);
}

static unsafe void ImGui_ImplGlfw_MonitorCallback(Monitor* monitor, ConnectedState state)
{
	// Unused in 'master' branch but 'docking' branch will use this, so we declare it ahead of it so if you have to install callbacks you can install this one too.
}

static unsafe void ImGui_ImplGlfw_InstallCallbacks(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window)
{
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();

    GLFW.SetWindowFocusCallback(window, ImGui_ImplGlfw_WindowFocusCallback);
    GLFW.SetCursorEnterCallback(window, ImGui_ImplGlfw_CursorEnterCallback);
    GLFW.SetCursorPosCallback(window, ImGui_ImplGlfw_CursorPosCallback);
    GLFW.SetMouseButtonCallback(window, ImGui_ImplGlfw_MouseButtonCallback);
    GLFW.SetScrollCallback(window, ImGui_ImplGlfw_ScrollCallback);
    GLFW.SetKeyCallback(window, ImGui_ImplGlfw_KeyCallback);
    GLFW.SetCharCallback(window, ImGui_ImplGlfw_CharCallback);
    GLFW.SetMonitorCallback(ImGui_ImplGlfw_MonitorCallback);
    bd->InstalledCallbacks = true;
}

static unsafe void ImGui_ImplGlfw_RestoreCallbacks(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window)
{
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();

    GLFW.SetWindowFocusCallback(window, bd->PrevUserCallbackWindowFocus);
    GLFW.SetCursorEnterCallback(window, bd->PrevUserCallbackCursorEnter);
    GLFW.SetCursorPosCallback(window, bd->PrevUserCallbackCursorPos);
    GLFW.SetMouseButtonCallback(window, bd->PrevUserCallbackMousebutton);
    GLFW.SetScrollCallback(window, bd->PrevUserCallbackScroll);
    GLFW.SetKeyCallback(window,bd->PrevUserCallbackKey);
    GLFW.SetCharCallback(window, bd->PrevUserCallbackChar);
    GLFW.SetMonitorCallback(bd->PrevUserCallbackMonitor);
    bd->InstalledCallbacks = false;
}

static unsafe bool ImGui_ImplGlfw_Init(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window, bool install_callbacks, GlfwClientApi client_api)
{
    var io = ImGui.GetIO();

    // Setup backend capabilities flags
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();
    io.BackendPlatformUserData = (nint) bd;
    io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;         // We can honor GetMouseCursor() values (optional)
    io.BackendFlags |= ImGuiBackendFlags.HasSetMousePos;          // We can honor io.WantSetMousePos requests (optional, rarely used)

    bd->Window = window;
    bd->Time = 0.0;

    io.SetClipboardTextFn = ImGui_ImplGlfw_SetClipboardText;
    io.GetClipboardTextFn = ImGui_ImplGlfw_GetClipboardText;
    io.ClipboardUserData = bd->Window;


    // Create mouse cursors
    // (By design, on X11 cursors are user configurable and some cursors may be missing. When a cursor doesn't exist,
    // GLFW will emit an error which will often be printed by the app, so we temporarily disable error reporting.
    // Missing cursors will return nullptr and our _UpdateMouseCursor() function will use the Arrow cursor instead.)
    GLFWerrorfun prev_error_callback = glfwSetErrorCallback(nullptr);
    bd->MouseCursors[ImGuiMouseCursor_Arrow] = glfwCreateStandardCursor(GLFW_ARROW_CURSOR);
    bd->MouseCursors[ImGuiMouseCursor_TextInput] = glfwCreateStandardCursor(GLFW_IBEAM_CURSOR);
    bd->MouseCursors[ImGuiMouseCursor_ResizeNS] = glfwCreateStandardCursor(GLFW_VRESIZE_CURSOR);
    bd->MouseCursors[ImGuiMouseCursor_ResizeEW] = glfwCreateStandardCursor(GLFW_HRESIZE_CURSOR);
    bd->MouseCursors[ImGuiMouseCursor_Hand] = glfwCreateStandardCursor(GLFW_HAND_CURSOR);
#if GLFW_HAS_NEW_CURSORS
    bd->MouseCursors[ImGuiMouseCursor_ResizeAll] = glfwCreateStandardCursor(GLFW_RESIZE_ALL_CURSOR);
    bd->MouseCursors[ImGuiMouseCursor_ResizeNESW] = glfwCreateStandardCursor(GLFW_RESIZE_NESW_CURSOR);
    bd->MouseCursors[ImGuiMouseCursor_ResizeNWSE] = glfwCreateStandardCursor(GLFW_RESIZE_NWSE_CURSOR);
    bd->MouseCursors[ImGuiMouseCursor_NotAllowed] = glfwCreateStandardCursor(GLFW_NOT_ALLOWED_CURSOR);
#else
    bd->MouseCursors[ImGuiMouseCursor_ResizeAll] = glfwCreateStandardCursor(GLFW_ARROW_CURSOR);
    bd->MouseCursors[ImGuiMouseCursor_ResizeNESW] = glfwCreateStandardCursor(GLFW_ARROW_CURSOR);
    bd->MouseCursors[ImGuiMouseCursor_ResizeNWSE] = glfwCreateStandardCursor(GLFW_ARROW_CURSOR);
    bd->MouseCursors[ImGuiMouseCursor_NotAllowed] = glfwCreateStandardCursor(GLFW_ARROW_CURSOR);
#endif
    glfwSetErrorCallback(prev_error_callback);
#if (GLFW_VERSION_COMBINED >= 3300) // Eat errors (see #5785)
    (void)glfwGetError(NULL);
#endif

    // Chain GLFW callbacks: our callbacks will call the user's previously installed callbacks, if any.
    if (install_callbacks)
        ImGui_ImplGlfw_InstallCallbacks(window);

    bd->ClientApi = client_api;
    return true;
}

static unsafe bool ImGui_ImplGlfw_InitForOpenGL(global::OpenTK.Windowing.GraphicsLibraryFramework.Window* window, bool install_callbacks)
{
    return ImGui_ImplGlfw_Init(window, install_callbacks, GlfwClientApi.GlfwClientApi_OpenGL);
}

static unsafe void ImGui_ImplGlfw_Shutdown()
{
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();
    var io = ImGui.GetIO();

    if (bd->InstalledCallbacks)
        ImGui_ImplGlfw_RestoreCallbacks((global::OpenTK.Windowing.GraphicsLibraryFramework.Window*) bd->Window);

    for (ImGuiMouseCursor cursor_n = 0; cursor_n < (ImGuiMouseCursor) 11; cursor_n++)
        GLFW.DestroyCursor((Cursor*) bd->MouseCursors[(int) cursor_n]);
}

static unsafe void ImGui_ImplGlfw_UpdateMouseData()
{
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();
    var io = ImGui.GetIO();

    if (GLFW.GetInputMode((global::OpenTK.Windowing.GraphicsLibraryFramework.Window*) bd->Window, CursorStateAttribute.Cursor)== CursorModeValue.CursorDisabled)
    {
        io.AddMousePosEvent(-float.MaxValue, -float.MaxValue);
        return;
    }
    bool is_app_focused =  GLFW.GetWindowAttrib((global::OpenTK.Windowing.GraphicsLibraryFramework.Window*) bd->Window, WindowAttributeGetBool.Focused);
    GLFW.GetWindowAttrib((global::OpenTK.Windowing.GraphicsLibraryFramework.Window*) bd->Window, WindowAttributeGetBool.Focused);
    if (is_app_focused)
    {
        // (Optional) Set OS mouse position from Dear ImGui if requested (rarely used, only when ImGuiConfigFlags_NavEnableSetMousePos is enabled by user)
        if (io.WantSetMousePos)
            GLFW.SetCursorPos((global::OpenTK.Windowing.GraphicsLibraryFramework.Window*) bd->Window, (double)io.MousePos.X, (double)io.MousePos.Y);

        // (Optional) Fallback to provide mouse position when focused (ImGui_ImplGlfw_CursorPosCallback already provides this when hovered or captured)
        if (is_app_focused && bd->MouseWindow == null)
        {
            double mouse_x, mouse_y;
            GLFW.GetCursorPos((global::OpenTK.Windowing.GraphicsLibraryFramework.Window*) bd->Window, out mouse_x, out mouse_y);
            io.AddMousePosEvent((float)mouse_x, (float)mouse_y);
            bd->LastValidMousePos = new Vector2((float)mouse_x, (float)mouse_y);
        }
    }
}

static void ImGui_ImplGlfw_UpdateMouseCursor()
{
    ImGuiIO& io = ImGui::GetIO();
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();
    if ((io.ConfigFlags & ImGuiConfigFlags_NoMouseCursorChange) || glfwGetInputMode(bd->Window, GLFW_CURSOR) == GLFW_CURSOR_DISABLED)
        return;

    ImGuiMouseCursor imgui_cursor = ImGui::GetMouseCursor();
    if (imgui_cursor == ImGuiMouseCursor_None || io.MouseDrawCursor)
    {
        // Hide OS mouse cursor if imgui is drawing it or if it wants no cursor
        glfwSetInputMode(bd->Window, GLFW_CURSOR, GLFW_CURSOR_HIDDEN);
    }
    else
    {
        // Show OS mouse cursor
        // FIXME-PLATFORM: Unfocused windows seems to fail changing the mouse cursor with GLFW 3.2, but 3.3 works here.
        glfwSetCursor(bd->Window, bd->MouseCursors[imgui_cursor] ? bd->MouseCursors[imgui_cursor] : bd->MouseCursors[ImGuiMouseCursor_Arrow]);
        glfwSetInputMode(bd->Window, GLFW_CURSOR, GLFW_CURSOR_NORMAL);
    }
}

// Update gamepad inputs
//static inline float Saturate(float v) { return v < 0.0f ? 0.0f : v  > 1.0f ? 1.0f : v; }
/*static void ImGui_ImplGlfw_UpdateGamepads()
{
    ImGuiIO& io = ImGui::GetIO();
    if ((io.ConfigFlags & ImGuiConfigFlags_NavEnableGamepad) == 0) // FIXME: Technically feeding gamepad shouldn't depend on this now that they are regular inputs.
        return;

    io.BackendFlags &= ~ImGuiBackendFlags_HasGamepad;
#if GLFW_HAS_GAMEPAD_API
    GLFWgamepadstate gamepad;
    if (!glfwGetGamepadState(GLFW_JOYSTICK_1, &gamepad))
        return;
    #define MAP_BUTTON(KEY_NO, BUTTON_NO, _UNUSED)          do { io.AddKeyEvent(KEY_NO, gamepad.buttons[BUTTON_NO] != 0); } while (0)
    #define MAP_ANALOG(KEY_NO, AXIS_NO, _UNUSED, V0, V1)    do { float v = gamepad.axes[AXIS_NO]; v = (v - V0) / (V1 - V0); io.AddKeyAnalogEvent(KEY_NO, v > 0.10f, Saturate(v)); } while (0)
#else
    int axes_count = 0, buttons_count = 0;
    const float* axes = glfwGetJoystickAxes(GLFW_JOYSTICK_1, &axes_count);
    const unsigned char* buttons = glfwGetJoystickButtons(GLFW_JOYSTICK_1, &buttons_count);
    if (axes_count == 0 || buttons_count == 0)
        return;
    #define MAP_BUTTON(KEY_NO, _UNUSED, BUTTON_NO)          do { io.AddKeyEvent(KEY_NO, (buttons_count > BUTTON_NO && buttons[BUTTON_NO] == GLFW_PRESS)); } while (0)
    #define MAP_ANALOG(KEY_NO, _UNUSED, AXIS_NO, V0, V1)    do { float v = (axes_count > AXIS_NO) ? axes[AXIS_NO] : V0; v = (v - V0) / (V1 - V0); io.AddKeyAnalogEvent(KEY_NO, v > 0.10f, Saturate(v)); } while (0)
#endif
    io.BackendFlags |= ImGuiBackendFlags_HasGamepad;
    MAP_BUTTON(ImGuiKey_GamepadStart,       GLFW_GAMEPAD_BUTTON_START,          7);
    MAP_BUTTON(ImGuiKey_GamepadBack,        GLFW_GAMEPAD_BUTTON_BACK,           6);
    MAP_BUTTON(ImGuiKey_GamepadFaceLeft,    GLFW_GAMEPAD_BUTTON_X,              2);     // Xbox X, PS Square
    MAP_BUTTON(ImGuiKey_GamepadFaceRight,   GLFW_GAMEPAD_BUTTON_B,              1);     // Xbox B, PS Circle
    MAP_BUTTON(ImGuiKey_GamepadFaceUp,      GLFW_GAMEPAD_BUTTON_Y,              3);     // Xbox Y, PS Triangle
    MAP_BUTTON(ImGuiKey_GamepadFaceDown,    GLFW_GAMEPAD_BUTTON_A,              0);     // Xbox A, PS Cross
    MAP_BUTTON(ImGuiKey_GamepadDpadLeft,    GLFW_GAMEPAD_BUTTON_DPAD_LEFT,      13);
    MAP_BUTTON(ImGuiKey_GamepadDpadRight,   GLFW_GAMEPAD_BUTTON_DPAD_RIGHT,     11);
    MAP_BUTTON(ImGuiKey_GamepadDpadUp,      GLFW_GAMEPAD_BUTTON_DPAD_UP,        10);
    MAP_BUTTON(ImGuiKey_GamepadDpadDown,    GLFW_GAMEPAD_BUTTON_DPAD_DOWN,      12);
    MAP_BUTTON(ImGuiKey_GamepadL1,          GLFW_GAMEPAD_BUTTON_LEFT_BUMPER,    4);
    MAP_BUTTON(ImGuiKey_GamepadR1,          GLFW_GAMEPAD_BUTTON_RIGHT_BUMPER,   5);
    MAP_ANALOG(ImGuiKey_GamepadL2,          GLFW_GAMEPAD_AXIS_LEFT_TRIGGER,     4,      -0.75f,  +1.0f);
    MAP_ANALOG(ImGuiKey_GamepadR2,          GLFW_GAMEPAD_AXIS_RIGHT_TRIGGER,    5,      -0.75f,  +1.0f);
    MAP_BUTTON(ImGuiKey_GamepadL3,          GLFW_GAMEPAD_BUTTON_LEFT_THUMB,     8);
    MAP_BUTTON(ImGuiKey_GamepadR3,          GLFW_GAMEPAD_BUTTON_RIGHT_THUMB,    9);
    MAP_ANALOG(ImGuiKey_GamepadLStickLeft,  GLFW_GAMEPAD_AXIS_LEFT_X,           0,      -0.25f,  -1.0f);
    MAP_ANALOG(ImGuiKey_GamepadLStickRight, GLFW_GAMEPAD_AXIS_LEFT_X,           0,      +0.25f,  +1.0f);
    MAP_ANALOG(ImGuiKey_GamepadLStickUp,    GLFW_GAMEPAD_AXIS_LEFT_Y,           1,      -0.25f,  -1.0f);
    MAP_ANALOG(ImGuiKey_GamepadLStickDown,  GLFW_GAMEPAD_AXIS_LEFT_Y,           1,      +0.25f,  +1.0f);
    MAP_ANALOG(ImGuiKey_GamepadRStickLeft,  GLFW_GAMEPAD_AXIS_RIGHT_X,          2,      -0.25f,  -1.0f);
    MAP_ANALOG(ImGuiKey_GamepadRStickRight, GLFW_GAMEPAD_AXIS_RIGHT_X,          2,      +0.25f,  +1.0f);
    MAP_ANALOG(ImGuiKey_GamepadRStickUp,    GLFW_GAMEPAD_AXIS_RIGHT_Y,          3,      -0.25f,  -1.0f);
    MAP_ANALOG(ImGuiKey_GamepadRStickDown,  GLFW_GAMEPAD_AXIS_RIGHT_Y,          3,      +0.25f,  +1.0f);
    #undef MAP_BUTTON
    #undef MAP_ANALOG
}#1#

void ImGui_ImplGlfw_NewFrame()
{
    ImGuiIO& io = ImGui::GetIO();
    ImGui_ImplGlfw_Data* bd = ImGui_ImplGlfw_GetBackendData();
    IM_ASSERT(bd != nullptr && "Did you call ImGui_ImplGlfw_InitForXXX()?");

    // Setup display size (every frame to accommodate for window resizing)
    int w, h;
    int display_w, display_h;
    glfwGetWindowSize(bd->Window, &w, &h);
    glfwGetFramebufferSize(bd->Window, &display_w, &display_h);
    io.DisplaySize = ImVec2((float)w, (float)h);
    if (w > 0 && h > 0)
        io.DisplayFramebufferScale = ImVec2((float)display_w / (float)w, (float)display_h / (float)h);

    // Setup time step
    double current_time = glfwGetTime();
    io.DeltaTime = bd->Time > 0.0 ? (float)(current_time - bd->Time) : (float)(1.0f / 60.0f);
    bd->Time = current_time;

    ImGui_ImplGlfw_UpdateMouseData();
    ImGui_ImplGlfw_UpdateMouseCursor();

    // Update game controllers (if enabled and available)
    ImGui_ImplGlfw_UpdateGamepads();
}
}*/