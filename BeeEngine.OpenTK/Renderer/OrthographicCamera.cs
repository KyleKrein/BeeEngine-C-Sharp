using BeeEngine.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.OpenTK.Renderer;

public class OrthographicCamera
{
    private float _rotation = 0;
    private Vector3 _position;
    private Matrix4 _viewProjectionMatrix;
    private Matrix4 _projectionMatrix;
    private Matrix4 _viewMatrix;

    public ref Matrix4 ProjectionMatrix => ref _projectionMatrix;

    public ref Matrix4 ViewMatrix => ref _viewMatrix;

    public ref Matrix4 ViewProjectionMatrix => ref _viewProjectionMatrix;

    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            RecalculateViewMatrix();
        }
    }

    public float Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            RecalculateViewMatrix();
        }
    }

    public OrthographicCamera(float left, float right, float bottom, float top)
    {
        ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, -1, 1);
        ViewMatrix = Matrix4.Identity;
        ViewProjectionMatrix = ProjectionMatrix * ViewMatrix;
    }

    private void RecalculateViewMatrix()
    {
        Matrix4 transform = Matrix4.CreateTranslation(Position) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation));
        ViewMatrix = transform.Inverted();
        ViewProjectionMatrix = ProjectionMatrix * ViewMatrix;
    }
}