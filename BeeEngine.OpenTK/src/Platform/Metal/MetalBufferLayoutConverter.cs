#if IOS
using Metal;
#endif


namespace BeeEngine.Platform.Metal;
#if IOS


internal static class MetalBufferLayoutConverter
{
    public static MTLVertexDescriptor ToMetal(this BufferLayout bufferLayout, nuint bufferIndex = 0)
    {
        MTLVertexDescriptor descriptor = new MTLVertexDescriptor();
        int i = 0;
        nuint offset = 0;
        foreach (var element in bufferLayout)
        {
            descriptor.Attributes[i].Format = ConvertFormatToMetal(element.Type);
            descriptor.Attributes[i].BufferIndex = bufferIndex;
            descriptor.Attributes[i].Offset = offset;
            offset += (nuint) element.Offset;
        }

        descriptor.Layouts[0].Stride = (nuint) bufferLayout.Stride;
        descriptor.Layouts[0].StepRate = 1;
        descriptor.Layouts[0].StepFunction = MTLVertexStepFunction.PerVertex;

        return descriptor;
    }

    private static MTLVertexFormat ConvertFormatToMetal(ShaderDataType type)
    {
        switch (type)
        {
            case ShaderDataType.None:
                return MTLVertexFormat.Invalid;
            case ShaderDataType.Float:
                return MTLVertexFormat.Float;
            case ShaderDataType.Float2:
                return MTLVertexFormat.Float2;
            case ShaderDataType.Float3:
                return MTLVertexFormat.Float3;
            case ShaderDataType.Float4:
                return MTLVertexFormat.Float4;
            case ShaderDataType.Mat3:
                return MTLVertexFormat.Invalid;
            case ShaderDataType.Mat4:
                return MTLVertexFormat.Invalid;
            case ShaderDataType.Int:
                return MTLVertexFormat.Int;
            case ShaderDataType.Int2:
                return MTLVertexFormat.Int2;
            case ShaderDataType.Int3:
                return MTLVertexFormat.Int3;
            case ShaderDataType.Int4:
                return MTLVertexFormat.Int4;
            case ShaderDataType.Bool:
                return MTLVertexFormat.Invalid;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
#endif