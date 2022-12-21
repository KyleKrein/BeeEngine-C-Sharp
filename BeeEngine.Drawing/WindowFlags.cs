namespace BeeEngine.Drawing;

[Flags]
public enum WindowFlags: uint
{
    Fullscreen = 1,
    Opengl = 2,
    Shown = 4,
    Hidden = 8,
    Borderless = 16, // 0x00000010
    Resizable = 32, // 0x00000020
    Minimized = 64, // 0x00000040
    Maximized = 128, // 0x00000080
    MouseGrabbed = 256, // 0x00000100
    InputFocus = 512, // 0x00000200
    MouseFocus = 1024, // 0x00000400
    FullscreenDesktop = 4097, // 0x00001001
    Foreign = 2048, // 0x00000800
    AllowHighdpi = 8192, // 0x00002000
    MouseCapture = 16384, // 0x00004000
    AlwaysOnTop = 32768, // 0x00008000
    SkipTaskbar = 65536, // 0x00010000
    Utility = 131072, // 0x00020000
    Tooltip = 262144, // 0x00040000
    PopupMenu = 524288, // 0x00080000
    KeyboardGrabbed = 1048576, // 0x00100000
    Vulkan = 268435456, // 0x10000000
    Metal = 33554432, // 0x02000000
    InputGrabbed = MouseGrabbed, // 0x00000100
}
