using BeeEngine.Mathematics;
using BeeEngine;
using BeeEngine.SmartPointers;

namespace BeeEngine._2D;

public class Sprite
{
    private readonly SharedRef<Texture2D> _texture;
    public Vector2[] TexCoords { get; } = new Vector2[4];

    public SharedRef<Texture2D> Texture => _texture.Share();

    private Sprite(Texture2D texture, Vector2 min, Vector2 max)
    {
        TexCoords[0] = new Vector2(min.X, min.Y);
        TexCoords[1] = new Vector2(max.X, min.Y);
        TexCoords[2] = new Vector2(max.X, max.Y);
        TexCoords[3] = new Vector2(min.X, max.Y);
        _texture = new SharedRef<Texture2D>(texture);
    }
    private Sprite(Texture2D texture)
    {
        TexCoords[0] = new Vector2(0, 0);
        TexCoords[1] = new Vector2(1, 0);
        TexCoords[2] = new Vector2(1, 1);
        TexCoords[3] = new Vector2(0, 1);
        _texture = new SharedRef<Texture2D>(texture);
    }
    public static Sprite AsSubTexture(Texture2D texture, Vector2 min, Vector2 max)
    {
        return new Sprite(texture, min, max);
    }

    public static Sprite CreateFromCoords(Texture2D texture, Vector2 position, Vector2 size)
    {
        Vector2 min = new Vector2(position.X*size.X/texture.Width, position.Y*size.Y/texture.Height);
        Vector2 max = new Vector2((position.X + 1)*size.X/texture.Width, (position.Y + 1)*size.Y/texture.Height);
        return AsSubTexture(texture, min, max);
    }
    public static Sprite FromFile(string filepath)
    {
        return new Sprite(Texture2D.CreateFromFile(filepath));
    }
}