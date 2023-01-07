using BeeEngine.Platform.Metal;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Metal;
using MetalKit;
using ObjCRuntime;
using UIKit;

namespace BeeEngine;

internal class MetalAppDelegate: UIApplicationDelegate
{
    public override UIWindow? Window { get; set; }
    public static bool IsNotReady { get; private set; } = true;
    public static MetalContext Context { get; set; }

    public static IOSWindow EngineWindow;
    public static MetalAppDelegate Instance;
    public override void WillTerminate(UIApplication application)
    {
        EngineWindow.ShutDown();
        base.WillTerminate(application);
    }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // create a new window instance based on the screen size
        Window = new UIWindow(UIScreen.MainScreen.Bounds);
        Instance = this;
        Window.RootViewController = new MetalViewController();
        Window.MakeKeyAndVisible();
        EngineWindow.StartGameLoop();
        IsNotReady = false;
        return true;
    }
}

internal class MetalViewDelegate : NSObject, IMTKViewDelegate
{
    private IOSWindow Window;

    public MetalViewDelegate(IOSWindow window)
    {
        Window = window;
    }

    public void DrawableSizeWillChange(MTKView view, CGSize size)
    {
        Window.DrawableSizeWillChange(view, size);
    }

    public void Draw(MTKView view)
    {
        Window.Draw(view);
    }
}
internal class IOSWindow: Window, IMTKViewDelegate
{
    public override int Width
    {
        get => _width;
        set
        {
            Log.Error("Changing Width of window on iOS is unsupported");
            return;
        }
    }

    public override int Height
    {
        get => _height;
        set
        {
            Log.Error("Changing Height of window on iOS is unsupported");
            return;
        }
    }

    public override string Title
    {
        get => _title;
        set => _title = value;
    }

    public override VSync VSync { get; set; }

    public Thread GameLoop;

    public void StartGameLoop()
    {
        Start();
        //GameLoop = new Thread(Start);
        //GameLoop.Start();
    }

    private void Start()
    {
        ((MetalContext)Context).Init(out _device, out _layer, out _pipelineState, out _commandQueue, out _view, out var controller);
        TestInit();
        /*while (isActive)
        {
            Draw(_view);
        } */  
    }

    private void TestInit()
    {
        //TRIANGLE TEST
        float[] triangleVerteces =
        {
            0.0f, 0.5f, 0.0f,
            -0.5f, 0.0f, 0.0f,
            0.5f, 0.0f, 0.0f
        };
        _vertexBuffer = _device.CreateBuffer(triangleVerteces, MTLResourceOptions.StorageModeShared)!;

        var defaultLibrary = _device.CreateDefaultLibrary();
        var fragmentProgram = defaultLibrary.CreateFunction("basic_fragment");
        var vertexProgram = defaultLibrary.CreateFunction("basic_vertex");
        
        var pipelineStateDescriptor = new MTLRenderPipelineDescriptor
        {
            VertexFunction = vertexProgram,
            FragmentFunction = fragmentProgram
        };
        pipelineStateDescriptor.ColorAttachments[0].PixelFormat = MTLPixelFormat.BGRA8Unorm;

        
        _pipelineState = _device.CreateRenderPipelineState(pipelineStateDescriptor, out var error);
        if (_pipelineState == null)
        {
            throw new Exception(error.ToString());
        }

    }

    private IMTLDevice _device;
    internal CAMetalLayer _layer;
    private IMTLBuffer _vertexBuffer;
    private IMTLRenderPipelineState _pipelineState;
    private IMTLCommandQueue _commandQueue;

    private MetalView _view;
    //private MetalViewController _metalViewController;
    
    //TEMP
    private readonly float[] triangleVerteces =
    {
        0.0f, 0.5f, 0.0f,
        -0.5f, 0.0f, 0.0f,
        0.5f, 0.0f, 0.0f
    };
    

    public IOSWindow(WindowProps initSettings) : base(initSettings)
    {
        MetalAppDelegate.EngineWindow = this;
        Context = new MetalContext(this);
        MetalAppDelegate.Context = (MetalContext) Context;
    }

    public override void Init()
    {
        //MetalAppDelegate.Instance.Window.RootViewController = controller;
        //MetalAppDelegate.Instance.Window.MakeKeyAndVisible();

    }

    private Action _updateLoop;
    private Action _renderLoop;
    private bool isActive = true;
    private int _width;
    private int _height;
    private string _title;

    public override void Run(Action updateLoop, Action renderLoop)
    {
        _updateLoop = updateLoop;
        _renderLoop = renderLoop;
        UIApplication.Main(null, null, typeof(MetalAppDelegate));
        while (isActive)
        {
            //Draw(_view);
        }
    }

    public override void RunMultiThreaded(Action updateLoop, Action renderLoop)
    {
        _updateLoop = updateLoop;
        _renderLoop = renderLoop;
        UIApplication.Main(null, null, typeof(MetalAppDelegate));
        while (isActive)
        {
            //Draw(_view);
        }
    }

    public override void ReleaseUnmanagedResources()
    {
        
    }

    public NativeHandle Handle { get; }
    public void DrawableSizeWillChange(MTKView view, CGSize size)
    {
        _width = (int) size.Width;
        _height = (int) size.Height;
    }

    public void Draw(MTKView view)
    {
        _updateLoop.Invoke();
        var drawable = _layer.NextDrawable();
        var renderPassDescriptor = view.CurrentRenderPassDescriptor;
        renderPassDescriptor.ColorAttachments[0].Texture = drawable.Texture;
        renderPassDescriptor.ColorAttachments[0].LoadAction = MTLLoadAction.Clear;
        renderPassDescriptor.ColorAttachments[0].ClearColor = new MTLClearColor(0.7, 0.2, 0.7, 1);

        var commandBuffer = _commandQueue.CommandBuffer();

        var renderEncoder = commandBuffer.CreateRenderCommandEncoder(renderPassDescriptor);
        renderEncoder.SetRenderPipelineState(_pipelineState);

        if (_vertexBuffer != null)
        {
            renderEncoder.SetVertexBuffer(_vertexBuffer, 0,0);
            renderEncoder.DrawPrimitives(MTLPrimitiveType.Triangle, 0, 3, 1);
        }
        
        _renderLoop.Invoke();
        renderEncoder.EndEncoding();
        
        commandBuffer.PresentDrawable(drawable);
        commandBuffer.Commit();
    }

    public void ShutDown()
    {
        isActive = false;
    }
}