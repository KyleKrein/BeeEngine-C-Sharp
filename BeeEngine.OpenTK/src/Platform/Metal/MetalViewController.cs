#if IOS
using BeeEngine.Mathematics;
using CoreAnimation;
using CoreGraphics;
using Metal;
using MetalKit;
using ObjCRuntime;
using UIKit;

namespace BeeEngine.Platform.Metal;

internal class MetalViewController: UIViewController, IMTKViewDelegate
{
    //private CADisplayLink timer;
    private IOSWindow _window;

    private NativeHandle _handle;
    public MetalViewController(IOSWindow window)
    {
        _window = window;
    }
    private IMTLBuffer _vertexBuffer;
    private IMTLRenderPipelineState _pipelineState;
    private CADisplayLink timer;
    
    

    private readonly float[] triangleVerteces =
    {
        0.0f, 0.5f, 0.0f,
        -0.5f, 0.0f, 0.0f,
        0.5f, 0.0f, 0.0f
    };
    public override void ViewDidLoad()
    {
        //INIT
        
        base.ViewDidLoad();
        _window.Context.Init();
        /*
        Metal.Device = MTLDevice.SystemDefault!;
        Metal.Layer = new CAMetalLayer();        // 1
        Metal.Layer.Device = Metal.Device;           // 2
        Metal.Layer.PixelFormat = MTLPixelFormat.BGRA8Unorm; // 3
        Metal.Layer.FramebufferOnly = true;    // 4
        
        var view = new MetalView(_handle);
        Metal.View = view;
        view.Frame = View.Frame;
        
        view.SampleCount = 1;
        view.DepthStencilPixelFormat = MTLPixelFormat.Depth32Float_Stencil8;
        view.ColorPixelFormat = MTLPixelFormat.BGRA8Unorm;
        view.PreferredFramesPerSecond = 60;
        
        view.AutosizesSubviews = true;
        view.AutoResizeDrawable = true;
        view.Layer.Frame = View.Layer.Frame;
        view.Delegate = this;
        view.Device = Metal.Device;
        Metal.Layer.Frame = View.Layer.Frame;  // 5
        view.Layer.AddSublayer(Metal.Layer);
        View = view;

        //TRIANGLE TEST
        _vertexBuffer = Metal.Device.CreateBuffer(triangleVerteces, MTLResourceOptions.StorageModeShared)!;

        var defaultLibrary = Metal.Device.CreateDefaultLibrary();
        var fragmentProgram = defaultLibrary.CreateFunction("quad_fragment");
        var vertexProgram = defaultLibrary.CreateFunction("quad_vertex");
        ((MetalRenderer2DAPI)Renderer2DAPI.Instance).InitMetal();
        
        var pipelineStateDescriptor = new MTLRenderPipelineDescriptor
        {
            VertexFunction = vertexProgram,
            FragmentFunction = fragmentProgram
        };
        pipelineStateDescriptor.ColorAttachments[0].PixelFormat = MTLPixelFormat.BGRA8Unorm;

        
        _pipelineState = Metal.Device.CreateRenderPipelineState(pipelineStateDescriptor, out var error);
        if (_pipelineState == null)
        {
            throw new Exception(error.ToString());
        }
        //CONTINUE INIT
        Metal.depthState = Metal.Device.CreateDepthStencilState(new MTLDepthStencilDescriptor
        {
            DepthCompareFunction = MTLCompareFunction.Less,
            DepthWriteEnabled = true
        });
        Metal.CommandQueue = Metal.Device.CreateCommandQueue()!;
        Metal.ClearColor((Vector4) Color.CornflowerBlue);*/
    }

    public void DrawableSizeWillChange(MTKView view, CGSize size)
    {
        _window.DrawableSizeWillChange(view, size);
    }

    public void Draw(MTKView view)
    {
        _window.Draw(view);
    }
}
#endif