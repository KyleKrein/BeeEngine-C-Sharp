using BeeEngine.Mathematics;
using Metal;

namespace BeeEngine.Platform.Metal;

public class MetalShader: Shader
{
    private MTLRenderPipelineDescriptor _renderPipelineDescriptor;
    private IMTLRenderPipelineState _renderPipelineState;
    public MetalShader(string name, string filepath)
    {
        Name = name;
        var file = File.ReadAllText(filepath);
        GetNamesFromSource(out string vertexName, out string fragmentName);
        _renderPipelineState = Metal.CreateShader(file, vertexName, fragmentName, new MTLVertexDescriptor(), out _renderPipelineDescriptor);
    }

    private void GetNamesFromSource(out string vertexName, out string fragmentName)
    {
        vertexName = "func_vertex";
        fragmentName = "func_fragment";
    }

    public MetalShader(string source, string vertexName, string fragmentName, MTLVertexDescriptor vertexDescriptor)
    {
        _renderPipelineState = Metal.CreateShader(source, vertexName, fragmentName, vertexDescriptor, out _renderPipelineDescriptor);
    }

    public override void Bind()
    {
        Metal.BindShader(ref _renderPipelineState, _renderPipelineDescriptor);
    }

    public override void Unbind()
    {
        
    }

    public override void UploadUniformMatrix4(string name, ref Matrix4 matrix4)
    {
        
    }

    public override unsafe void UploadUniformMatrix4(string name, Matrix4* matrix4)
    {
        
    }

    public override void UploadUniformFloat(string name, float value)
    {
        
    }

    public override void UploadUniformFloat2(string name, Vector2 vector)
    {
        
    }

    public override void UploadUniformFloat3(string name, Vector3 vector)
    {
        
    }

    public override void UploadUniformFloat4(string name, Vector4 vector)
    {
        
    }

    public override void UploadUniformInt(string name, int value)
    {
        
    }

    public override void UploadUniformIntArray(string name, int[] values, int count)
    {
        
    }

    public override void UploadUniformInt2(string name, Vector2i vector)
    {
        
    }

    public override void UploadUniformInt3(string name, Vector3i vector)
    {
        
    }

    public override void UploadUniformInt4(string name, Vector4i vector)
    {
        
    }

    protected override void Dispose(bool disposing)
    {
        
    }
}