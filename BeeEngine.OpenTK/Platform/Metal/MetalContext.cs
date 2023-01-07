using CoreAnimation;
using Metal;

namespace BeeEngine.Platform.Metal;

internal class MetalContext: Context
{
    private IMTLDevice _device;
    internal CAMetalLayer _layer;
    private IMTLBuffer _vertexBuffer;
    private IMTLRenderPipelineState _pipelineState;
    private IMTLCommandQueue _commandQueue;
    private MetalViewController _metalViewController;
    private IOSWindow _window;
    public MetalViewDelegate _ViewDelegate;

    public MetalContext(IOSWindow iosWindow)
    {
        _window = iosWindow;
        _ViewDelegate = new MetalViewDelegate(iosWindow);
    }

    public override int SwapInterval { get; set; }
    public override void Init()
    {
        //Init(out _, out _, out _, out _, out _, out _);
    }

    public override void SwapBuffers()
    {
        
    }

    public override void MakeCurrent()
    {
        
    }

    public override void MakeNonCurrent()
    {
        
    }

    public override void Destroy()
    {
        
    }

    public void Init(out IMTLDevice device, out CAMetalLayer layer, out IMTLRenderPipelineState pipelineState, out IMTLCommandQueue commandQueue, out MetalView view, out MetalViewController controller)
    {
        _metalViewController = (MetalViewController?) MetalAppDelegate.Instance.Window.RootViewController;
        _device = MTLDevice.SystemDefault!;
        _layer = new CAMetalLayer();        // 1
        _layer.Device = _device;           // 2
        _layer.PixelFormat = MTLPixelFormat.BGRA8Unorm; // 3
        _layer.FramebufferOnly = true;    // 4
        
        view = new MetalView(0);
        view.Frame = _metalViewController.View.Frame;
        view.AutosizesSubviews = true;
        view.AutoResizeDrawable = true;
        view.Layer.Frame = _metalViewController.View.Layer.Frame;
        view.Delegate = _ViewDelegate;
        view.Device = _device;
        _layer.Frame = _metalViewController.View.Layer.Frame;  // 5
        view.Layer.AddSublayer(_layer);
        _metalViewController.View = view;
        
        
        //CONTINUE INIT

        _commandQueue = _device.CreateCommandQueue()!;

        device = _device;
        layer = _layer;
        pipelineState = _pipelineState;
        commandQueue = _commandQueue;
        controller = _metalViewController;
    }
}