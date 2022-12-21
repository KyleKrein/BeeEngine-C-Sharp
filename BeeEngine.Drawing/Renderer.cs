using SDL2;

namespace BeeEngine.Drawing;

internal class Renderer: IDisposable
{
    private IntPtr _renderer;
    private IntPtr _window;
    private Graphics? _graphics;
    private readonly object locker = new object();

    public void Init(string Title, int Width, int Height)
    {
        // Initilizes SDL.
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
        {
            throw new Exception($"There was an issue initializing SDL. {SDL.SDL_GetError()}");
        }

        // Create a new window given a title, size, and passes it a flag indicating it should be shown.
        _window = SDL.SDL_CreateWindow(
            Title,
            SDL.SDL_WINDOWPOS_UNDEFINED, 
            SDL.SDL_WINDOWPOS_UNDEFINED, 
            Width, 
            Height, 
            SDL.SDL_WindowFlags.SDL_WINDOW_HIDDEN | SDL.SDL_WindowFlags.SDL_WINDOW_VULKAN);

        if (_window == IntPtr.Zero)
        {
            throw new Exception($"There was an issue creating the window. {SDL.SDL_GetError()}");
        }

        // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
        _renderer = SDL.SDL_CreateRenderer(
            IntPtr.Zero, 
            -1,
            SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

        if (_renderer == IntPtr.Zero)
        {
            throw new Exception($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
        }
    }

    public void InitWindow(string text, Point location, Size size, bool useVsync, params WindowFlags[] flags)
    {
        // Initilizes SDL.
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
        {
            throw new Exception($"There was an issue initializing SDL. {SDL.SDL_GetError()}");
        }

        SDL.SDL_WindowFlags windowFlag = 0;
        foreach (var flag in flags)
        {
            windowFlag = windowFlag | (SDL.SDL_WindowFlags) flag;
        }
        // Create a new window given a title, size, and passes it a flag indicating it should be shown.
        _window = SDL.SDL_CreateWindow(
            text,
            location.X, 
            location.Y, 
            size.Width, 
            size.Height, 
            windowFlag
            );

        if (_window == IntPtr.Zero)
        {
            throw new Exception($"There was an issue creating the window. {SDL.SDL_GetError()}");
        }

        // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
        _renderer = SDL.SDL_CreateRenderer(
            _window, 
            -1,
            SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
            (useVsync ? SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC : 0));

        if (_renderer == IntPtr.Zero)
        {
            throw new Exception($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
        }
    }
    
    bool PollEvents()
    {
        // Check to see if there are any events and continue to do so until the queue is empty.
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
        {
            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    IsClosing = true;
                    return false;
            }
        }

        return true;
    }

    public bool IsClosing { get; private set; } = false;

    internal bool PrepareForDrawingFrame(Color BackgroundColor)
    {
        if (!PollEvents())
        {
            return false;
        }
        ResetRenderTarget();
        // Sets the color that the screen will be cleared with.
        if (SDL.SDL_SetRenderDrawColor(_renderer, BackgroundColor.R, BackgroundColor.G, 
                BackgroundColor.B, BackgroundColor.A) < 0)
        {
            throw new Exception($"There was an issue with setting the render draw color. {SDL.SDL_GetError()}");
        }

        // Clears the current render surface.
        if (SDL.SDL_RenderClear(_renderer) < 0)
        {
            throw new Exception($"There was an issue with clearing the render surface. {SDL.SDL_GetError()}");
        }
        return true;
    }

    public void SetRenderTarget(IntPtr target)
    {
        SDL.SDL_SetRenderTarget(_renderer, target);
    }

    public void ResetRenderTarget()
    {
        SDL.SDL_SetRenderTarget(_renderer, IntPtr.Zero);
    }
    internal void Render()
    {
        // Switches out the currently presented render surface with the one we just did work on.
        SDL.SDL_RenderPresent(_renderer);
    }

    public void Dispose()
    {
        SDL.SDL_DestroyRenderer(_renderer);
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_Quit();
    }

    public Graphics GetGraphics()
    {
        if (_graphics is null)
        {
            lock (locker)
            {
                if (_graphics is null)
                {
                    _graphics = new Graphics(this);
                }
            }
        }

        return _graphics;
    }

    public void InitWithoutWindow(WindowFlags[] flags)
    {
        // Initilizes SDL.
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
        {
            throw new Exception($"There was an issue initializing SDL. {SDL.SDL_GetError()}");
        }
        SDL.SDL_WindowFlags windowFlag = 0;
        foreach (var flag in flags)
        {
            windowFlag = windowFlag | (SDL.SDL_WindowFlags) flag;
        }

        if (windowFlag == 0)
        {
            windowFlag = windowFlag | SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL;
        }

        // Create a new window given a title, size, and passes it a flag indicating it should be shown.
        _window = SDL.SDL_CreateWindow(
            "Title",
            SDL.SDL_WINDOWPOS_UNDEFINED, 
            SDL.SDL_WINDOWPOS_UNDEFINED, 
            1, 
            1, 
            SDL.SDL_WindowFlags.SDL_WINDOW_HIDDEN | windowFlag);

        if (_window == IntPtr.Zero)
        {
            throw new Exception($"There was an issue creating the window. {SDL.SDL_GetError()}");
        }

        // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
        _renderer = SDL.SDL_CreateRenderer(
            _window, 
            -1,
            SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

        if (_renderer == IntPtr.Zero)
        {
            throw new Exception($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
        }
    }

    public IntPtr GetRenderer()
    {
        return _renderer;
    }

    public void DrawLine(int x1, int y1, int x2, int y2, Color color)
    {
        SDL.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDL.SDL_RenderDrawLine(_renderer, x1, y1, x2, y2);
    }

    public void DrawPoint(int x, int y, Color color)
    {
        SDL.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDL.SDL_RenderDrawPoint(_renderer, x, y);
    }

    public void DrawPointF(float x, float y, Color color)
    {
        SDL.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDL.SDL_RenderDrawPointF(_renderer, x, y);
    }

    public void DrawPoints(SDL.SDL_Point[] sdlPoints, Color color)
    {
        SDL.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDL.SDL_RenderDrawPoints(_renderer, sdlPoints, sdlPoints.Length);
    }

    public void DrawPointsF(SDL.SDL_FPoint[] sdlPoints, Color color)
    {
        SDL.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDL.SDL_RenderDrawPointsF(_renderer, sdlPoints, sdlPoints.Length);
    }

    public void DrawRect(SDL.SDL_Rect rect, Color color)
    {
        SDL.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDL.SDL_RenderDrawRect(_renderer, ref rect);
    }

    public void DrawRectF(SDL.SDL_FRect rect, Color color)
    {
        SDL.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDL.SDL_RenderDrawRectF(_renderer, ref rect);
    }

    public void DrawRects(SDL.SDL_Rect[] sdlRects, Color color)
    {
        SDL.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDL.SDL_RenderDrawRects(_renderer, sdlRects, sdlRects.Length);
    }

    public void DrawRectsF(SDL.SDL_FRect[] sdlRects, Color color)
    {
        SDL.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDL.SDL_RenderDrawRectsF(_renderer, sdlRects, sdlRects.Length);
    }

    public void DrawTexture(nint texture, SDL.SDL_Rect textureRect, SDL.SDL_Rect targetRect)
    {
        SDL.SDL_RenderCopy(_renderer, texture, ref textureRect, ref targetRect);
    }

    public void Clear(Color color)
    {
        SDL.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);

        // Clears the current render surface.
        SDL.SDL_RenderClear(_renderer);
    }
}