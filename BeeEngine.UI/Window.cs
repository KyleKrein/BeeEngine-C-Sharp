using BeeEngine.Drawing;
using SDL2;

namespace BeeEngine.UI;

public class Window: Control
{
    internal Renderer Renderer { get; set; }
    public Window(): this(null, "Title", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 1280, 720)
    {
        
    }

    internal void Init()
    {
        Renderer.InitWindow(Text, Location, Size, true, WindowFlags.Shown, WindowFlags.Opengl);
    }
    public Window(Control? parent, string? text) : base(parent, text)
    {
        if(parent is not null)
            throw new InvalidOperationException("Window can't be a child of another Control");
    }
    public Window(Control? parent, string? text, int x, int y, int width, int height): base(parent, text, x, y, width, height)
    {
        if(parent is not null)
            throw new InvalidOperationException("Window can't be a child of another Control");
    }

    public event EventHandler<Graphics> Paint; 
    internal override bool Update(Graphics g)
    {
        if (Renderer.IsClosing)
            return false;
        /*if(Invalid)
        {*/
            Renderer.PrepareForDrawingFrame(BackgroundColor);
            OnPaint(g);
            Paint?.Invoke(this, g);
            UpdateChildren(g);
            Invalid = false;
        //}
        Renderer.Render();
        return true;
    }


    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        Renderer.Dispose();
    }
}