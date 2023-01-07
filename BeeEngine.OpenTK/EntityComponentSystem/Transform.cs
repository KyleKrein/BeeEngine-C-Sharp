using BeeEngine.Mathematics;

namespace BeeEngine.EntityComponentSystem;

public sealed class Transform: Component
{
    private Matrix4 _transform;
    private Vector3 _position;
    private Vector3 _scale;
    private Quaternion _rotation;

    private Transform(Matrix4 matrix4)
    {
        _transform = matrix4;
        _position = _transform.ExtractTranslation();
        _scale = _transform.ExtractScale();
        _rotation = _transform.ExtractRotation();
    }

    public ref float X => ref _position.X;
    public ref float Y => ref _position.Y;
    public ref float Z => ref _position.Z;

    public Vector3 Position
    {
        get => _position;
    }

    public static implicit operator Matrix4(Transform transform)
    {
        return transform._transform;
    }

    public static implicit operator Transform(Matrix4 matrix4)
    {
        return new Transform(matrix4);
    }
}