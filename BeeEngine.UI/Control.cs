using BeeEngine.Drawing;

namespace BeeEngine.UI;

public class Control: IDisposable
{
    public Color BackgroundColor { get; set; }
    public Color ForegroundColor { get; set; }
    private Texture _buffer;
    private bool isLayoutSuspended = false;
    public virtual string Text
    {
        get => _text is null?string.Empty:_text;
        set
        {
            if (value is null)
            {
                _text = string.Empty;
            }

            if (value == Text)
            {
                return;
            }

            _text = value;
        }
    }
    private string _text;
    public int Width
    {
        get => _size.Width;
        set => _size = new Size(value, _size.Height);
    }
    public int Height 
    {
        get => _size.Height;
        set => _size = new Size(_size.Width, value);
    }
    public Point Location { get; set; }
    private Size _size;

    public Size Size
    {
        get => _size;
        set => _size = value;
    }
    private Control? _parent;

    public Control()
    {
        
    }

    public Control(Control? parent, string? text)
    {
        _parent = parent;
        Text = text;
    }
    public Control(Control? parent, string? text, int x, int y, int width, int height): this(parent, text)
    {
        Size = new Size(width, height);
        Location = new Point(x, y);
    }
    
    protected virtual void OnPaint(Graphics g)
    {
        
    }

    protected void SuspendLayout()
    {
        isLayoutSuspended = true;
    }

    protected void ResumeLayout()
    {
        isLayoutSuspended = false;
    }

    private void ReleaseUnmanagedResources()
    {
        // TODO release unmanaged resources here
    }

    private void Refresh()
    {
        
    }

    public void Invalidate()
    {
        Invalid = true;
    }

    internal virtual bool Update(Graphics g)
    {
        if (Invalid)
        {
            Graphics bufferGraphics = Graphics.FromTexture(_buffer);
            bufferGraphics.Clear(BackgroundColor);
            OnPaint(bufferGraphics);
            UpdateChildren(bufferGraphics);
            bufferGraphics.Dispose();
            Invalid = false;
        }
        g.DrawTexture(_buffer, Location.X, Location.Y, Size.Width, Size.Height);
        return true;
    }

    internal virtual void UpdateChildren(Graphics g)
    {
        //TODO: Make Children Collection and make Update method
    }

    internal bool Invalid { get; set; } = true;

    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing)
        {
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Control()
    {
        Dispose(false);
    }
}