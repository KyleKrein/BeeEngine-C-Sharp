using System.Numerics;

#if IOS
using CoreAnimation;
using Metal;
using MetalKit;
#endif

using Vector4 = BeeEngine.Mathematics.Vector4;

namespace BeeEngine.Platform.Metal;
internal static class Metal
{
#if IOS
    public static IMTLDevice Device;
    public static MTKView View;
    public static CAMetalLayer Layer;
    public static IMTLCommandQueue CommandQueue;
    private static MTLRenderPassDescriptor _renderPassDescriptor;
    private static IMTLRenderCommandEncoder renderCommandEncoder;
    private static IMTLCommandBuffer _commandBuffer;
    private static ICAMetalDrawable _currentDrawable;
    private static IMTLBuffer bindedVertexBuffer;
    private static IMTLBuffer bindedIndexBuffer;
    
    public static void BindBuffer(IMTLBuffer buffer, ShaderType type)
    {
        switch (type)
        {
            case ShaderType.Vertex:
                bindedVertexBuffer = buffer;
                break;
            case ShaderType.Fragment:
                bindedIndexBuffer = buffer;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public static void ClearColor(Vector4 color)
    {
        if (View is null)
        {
            return;
        }
        _renderPassDescriptor = View.CurrentRenderPassDescriptor;
        _renderPassDescriptor.ColorAttachments[0].LoadAction = MTLLoadAction.Clear;
        _renderPassDescriptor.ColorAttachments[0].ClearColor = 
            new MTLClearColor(color.X, color.Y, color.Z, color.W);
    }

    public static void Clear()
    {
        
    }

    public static IMTLRenderPipelineState CreateShader(string source, string vertexName, string fragmentName, MTLVertexDescriptor vertexDescriptor, out MTLRenderPipelineDescriptor descriptor)
    {
        var library = Device.CreateLibrary(source, new MTLCompileOptions(){}, out var error);
        //Log.Assert(library != null, "Could not create shader from source: {0}", error);
        //Log.Info("Library: {0}", library);
        var fragmentProgram = library.CreateFunction(fragmentName);
        //Log.Info("Fragment: {0}", fragmentProgram);
        var vertexProgram = library.CreateFunction(vertexName);
        //Log.Info("Vertex: {0}", vertexProgram);
        
        var pipelineStateDescriptor = new MTLRenderPipelineDescriptor
        {
            VertexFunction = vertexProgram,
            FragmentFunction = fragmentProgram,
            SampleCount = View.SampleCount,
            //VertexDescriptor = vertexDescriptor,
            DepthAttachmentPixelFormat = View.DepthStencilPixelFormat,
            StencilAttachmentPixelFormat = View.DepthStencilPixelFormat
        };
        pipelineStateDescriptor.ColorAttachments[0].PixelFormat = MTLPixelFormat.BGRA8Unorm;
        //Log.Info("Pipeline state descriptor: {0}", pipelineStateDescriptor);;
        
        var pipelineState = Device.CreateRenderPipelineState(pipelineStateDescriptor, out error);
        Log.Assert(pipelineState != null, "Can't build pipeline state for shader: {0}", error);
        descriptor = pipelineStateDescriptor;
        return pipelineState;
    }

    public static void PrepareFrame()
    {
        _currentDrawable = Layer.NextDrawable();
        _renderPassDescriptor.ColorAttachments[0].Texture = _currentDrawable.Texture;

        _commandBuffer = CommandQueue.CommandBuffer();

        renderCommandEncoder = _commandBuffer.CreateRenderCommandEncoder(_renderPassDescriptor);
        renderCommandEncoder.SetDepthStencilState(depthState);
    }

    public static IMTLDepthStencilState depthState;

    public static void DrawTriangles(nuint vertexCount, nuint instanceCount)
    {
        renderCommandEncoder.SetVertexBuffer(bindedVertexBuffer, 0,0);
        if(bindedIndexBuffer is not null)
            renderCommandEncoder.SetFragmentBuffer(bindedIndexBuffer, 0,1);
        renderCommandEncoder.DrawPrimitives(MTLPrimitiveType.Triangle, 0, vertexCount, instanceCount);
    }
    public static void DrawIndexedTriangles(nuint indexCount)
    {
        renderCommandEncoder.SetVertexBuffer(bindedVertexBuffer, 0,0);
        renderCommandEncoder.DrawIndexedPrimitives(MTLPrimitiveType.Triangle, indexCount, MTLIndexType.UInt32, bindedIndexBuffer, 0);
    }

    public static void Flush()
    {
        renderCommandEncoder.EndEncoding();
        _commandBuffer.PresentDrawable(_currentDrawable);
        _commandBuffer.Commit();
    }

    public static IMTLBuffer CreateBuffer(nuint size)
    {
        return Device.CreateBuffer(size, MTLResourceOptions.CpuCacheModeDefault);
    }

    public static IMTLBuffer CreateBuffer<T>(T[] data) where T : struct
    {
        Log.Assert(data is not null, "Index buffer data is null!");
        return Device.CreateBuffer(data, MTLResourceOptions.StorageModeShared);
    }
    public static IMTLBuffer CreateBuffer(nint ptr, nuint length)
    {
        return Device.CreateBuffer(ptr,length, MTLResourceOptions.CpuCacheModeDefault);
    }

    /*public static void CopyDataToBuffer(nint data, nint size, IMTLBuffer buffer)
    {
        
    }*/
    public static void BindShader(ref IMTLRenderPipelineState renderPipelineState, MTLRenderPipelineDescriptor pipelineStateDescriptor)
    {
        if (renderPipelineState is null)
        {
            Log.Error("renderPipelineState in shader is NULL!!!");
            return;
            //renderPipelineState = Device.CreateRenderPipelineState(pipelineStateDescriptor, out var error);
            //Log.Assert(renderPipelineState == null, "Pipeline state is still null!: {0}", error);
            //return;
        }
        if (Device is null)
        {
            Log.Error("Device is NULL!!!");
            return;
        }
        if (renderCommandEncoder is null)
        {
            Log.Error("renderCommandEncoder is NULL!!!");
            return;
        }
        renderCommandEncoder.SetRenderPipelineState(renderPipelineState);
    }
#endif
}