using System.Collections;
using System.Data;

namespace BeeEngine.OpenTK.Renderer;

public sealed class BufferLayout: IEnumerable<BufferElement>
{
    
    internal readonly List<BufferElement> BufferElements;
    internal bool IsReadOnly = false;
    public int Stride { get; private set; }

    /*public BufferLayout(params BufferElement[] elements)
    {
        BufferElements = new List<BufferElement>(elements);
    }
    public BufferLayout(IEnumerable<BufferElement> elements)
    {
        BufferElements = new List<BufferElement>(elements);
    }*/

    public BufferLayout()
    {
        BufferElements = new List<BufferElement>();
    }

    public void Add(BufferElement element)
    {
        if (IsReadOnly)
            throw new ReadOnlyException("Can't change BufferLayout after it was built");
        BufferElements.Add(element);
    }

    public IEnumerator<BufferElement> GetEnumerator()
    {
        return BufferElements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(ShaderDataType element, string position, bool normalized = false)
    {
        if (IsReadOnly)
            throw new ReadOnlyException("Can't change BufferLayout after it was built");
        Add(new BufferElement(element, position, normalized));
    }

    internal void Build()
    {
        IsReadOnly = true;

        CalculateOffsetsAndStride();
    }

    private void CalculateOffsetsAndStride()
    {
        int offset = 0;
        Stride = 0;
        foreach (var element in BufferElements)
        {
            element.Offset = offset;
            offset += element.Size;
            Stride += element.Size;
        }
    }
}

public sealed class BufferElement
{
    public readonly string Name;
    public readonly ShaderDataType Type;
    public readonly int Size;
    public bool Normalized { get; }
    public int Offset { get; internal set; }
    public BufferElement(ShaderDataType type, string name, bool normalized)
    {
        Name = name;
        Type = type;
        Size = ShaderDataTypeSize(type);
        Offset = 0;
        Normalized = normalized;
    }

    internal static int ShaderDataTypeSize(ShaderDataType type)
    {
        return type switch
        {
            ShaderDataType.None => 0,
            ShaderDataType.Float => 4,
            ShaderDataType.Float2 => 8,
            ShaderDataType.Float3 => 12,
            ShaderDataType.Float4 => 16,
            ShaderDataType.Mat3 => 4*3*3,
            ShaderDataType.Mat4 => 4*4*4,
            ShaderDataType.Int => 4,
            ShaderDataType.Int2 => 8,
            ShaderDataType.Int3 => 12,
            ShaderDataType.Int4 => 16,
            ShaderDataType.Bool => sizeof(bool),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public int GetComponentCount()
    {
        return Type switch
        {
            ShaderDataType.None => 0,
            ShaderDataType.Float => 1,
            ShaderDataType.Float2 => 2,
            ShaderDataType.Float3 => 3,
            ShaderDataType.Float4 => 4,
            ShaderDataType.Mat3 => 3 * 3,
            ShaderDataType.Mat4 => 4 * 4,
            ShaderDataType.Int => 1,
            ShaderDataType.Int2 => 2,
            ShaderDataType.Int3 => 3,
            ShaderDataType.Int4 => 4,
            ShaderDataType.Bool => 1,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public enum ShaderDataType
{
    None = 0,
    Float,
    Float2,
    Float3,
    Float4,
    Mat3,
    Mat4,
    Int,
    Int2,
    Int3,
    Int4,
    Bool
}