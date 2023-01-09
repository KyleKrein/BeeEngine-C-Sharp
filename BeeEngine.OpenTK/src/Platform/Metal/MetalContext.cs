using BeeEngine.Mathematics;

#if IOS
using CoreAnimation;
using Metal;
#endif


namespace BeeEngine.Platform.Metal;
#if IOS
internal class MetalContext: Context
{
    private IMTLDevice _device;
    internal CAMetalLayer _layer;
    private IMTLCommandQueue _commandQueue;
    private MetalViewController _metalViewController;
    public MetalViewController _ViewDelegate;

    public MetalContext()
    {
    }

    public override int SwapInterval { get; set; }
    public override void Init()
    {
        _metalViewController = (MetalViewController?) MetalAppDelegate.Instance.Window.RootViewController;
        Log.AssertAndThrow(_metalViewController is not null, "_metalViewController is null!");
        _device = MTLDevice.SystemDefault!;
        Log.AssertAndThrow(_device is not null, "Device is null!");
        Metal.Device = _device;
        _layer = new CAMetalLayer();        // 1
        _layer.Device = _device;           // 2
        _layer.PixelFormat = MTLPixelFormat.BGRA8Unorm; // 3
        _layer.FramebufferOnly = true;    // 4
        
        var view = new MetalView(0);
        view.Frame = _metalViewController.View.Frame;
        view.SampleCount = 1;
        view.DepthStencilPixelFormat = MTLPixelFormat.Depth32Float_Stencil8;
        view.ColorPixelFormat = MTLPixelFormat.BGRA8Unorm;
        view.PreferredFramesPerSecond = 60;
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
        
        Metal.depthState = _device.CreateDepthStencilState(new MTLDepthStencilDescriptor
        {
            DepthCompareFunction = MTLCompareFunction.Less,
            DepthWriteEnabled = true
        });
        Platform.Metal.Metal.View = view;
        Platform.Metal.Metal.Layer = _layer;
        Platform.Metal.Metal.CommandQueue = _commandQueue;
        Metal.ClearColor((Vector4) Color.CornflowerBlue);
        ((MetalRenderer2DAPI) Renderer2DAPI.Instance).InitMetal();
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
}
#endif