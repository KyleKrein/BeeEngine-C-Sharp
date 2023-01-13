namespace BeeEngine;

public struct RendererID
{
    internal uint _id;

    public RendererID(int id): this((uint)id) {}

    public RendererID(uint id)
    {
        _id = id;
    }

    public RendererID() : this((uint) 0){}

    public static implicit operator uint(RendererID rendererId)
    {
        return rendererId._id;
    }
    public static implicit operator int(RendererID rendererId)
    {
        return (int) rendererId._id;
    }

    public static implicit operator RendererID(uint id)
    {
        return new RendererID(id);
    }
    public static implicit operator RendererID(int id)
    {
        return new RendererID(id);
    }
}