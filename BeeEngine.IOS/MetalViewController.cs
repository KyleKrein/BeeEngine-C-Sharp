using System.Drawing;
using CoreAnimation;
using Metal;
using MetalKit;
using ObjCRuntime;

namespace BeeEngine.IOS;

public class MetalView : MTKView
{
    public MetalView(NativeHandle handle)
    {
        
    }
    public override void Draw()
    {
        Delegate.Draw(this);
    }
}
public class MetalViewController: UIViewController, IMTKViewDelegate
{
    private IMTLDevice _device;
    internal CAMetalLayer _layer;
    private IMTLBuffer _vertexBuffer;
    private IMTLRenderPipelineState _pipelineState;
    private IMTLCommandQueue _commandQueue;
    private CADisplayLink timer;
    
    

    private readonly float[] triangleVerteces =
    {
        0.0f, 0.5f, 0.0f,
        -0.5f, 0.0f, 0.0f,
        0.5f, 0.0f, 0.0f
    };

    private NativeHandle _handle;
    public MetalViewController(NativeHandle handle)
    {
        _handle = handle;
    }
    public override void ViewDidLoad()
    {
        //INIT
        base.ViewDidLoad();
        _device = MTLDevice.SystemDefault!;
        _layer = new CAMetalLayer();        // 1
        _layer.Device = _device;           // 2
        _layer.PixelFormat = MTLPixelFormat.BGRA8Unorm; // 3
        _layer.FramebufferOnly = true;    // 4
        
        var view = new MetalView(_handle);
        view.Frame = View.Frame;
        view.AutosizesSubviews = true;
        view.AutoResizeDrawable = true;
        view.Layer.Frame = View.Layer.Frame;
        view.Delegate = this;
        view.Device = _device;
        _layer.Frame = View.Layer.Frame;  // 5
        view.Layer.AddSublayer(_layer);
        View = view;

        //TRIANGLE TEST
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
        //CONTINUE INIT

        _commandQueue = _device.CreateCommandQueue()!;
        
        NSRunLoop.Main.Perform(GameLoop);
        timer = new CADisplayLink();
        timer.Init();
        
    }

    private void GameLoop()
    {
        //Render();
    }

    private void Render(MTKView view)
    {
        var drawable = _layer.NextDrawable();
        var renderPassDescriptor = view.CurrentRenderPassDescriptor;
        renderPassDescriptor.ColorAttachments[0].Texture = drawable.Texture;
        renderPassDescriptor.ColorAttachments[0].LoadAction = MTLLoadAction.Clear;
        renderPassDescriptor.ColorAttachments[0].ClearColor = new MTLClearColor(0.7, 0.2, 0.7, 1);

        var commandBuffer = _commandQueue.CommandBuffer();

        var renderEncoder = commandBuffer.CreateRenderCommandEncoder(renderPassDescriptor);
        renderEncoder.SetRenderPipelineState(_pipelineState);
        
        
        
        renderEncoder.SetVertexBuffer(_vertexBuffer, 0,0);
        renderEncoder.DrawPrimitives(MTLPrimitiveType.Triangle, 0, 3, 1);
        renderEncoder.EndEncoding();
        
        commandBuffer.PresentDrawable(drawable);
        commandBuffer.Commit();
    }

    public void DrawableSizeWillChange(MTKView view, CGSize size)
    {
        view.CurrentRenderPassDescriptor.RenderTargetWidth = (nuint) size.Width;
        view.CurrentRenderPassDescriptor.RenderTargetHeight = (nuint) size.Height;
        //_layer.DrawableSize = size;
    }

    public void Draw(MTKView view)
    {
        Render(view);
    }
}