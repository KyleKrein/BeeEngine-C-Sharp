using BeeEngine.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.OpenTK.Renderer;

public class OrthographicCamera
{
    private float _rotation;
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
        _position = Vector3.Zero;
        _rotation = 0.0f;
        _projectionMatrix = Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, -1, 1);
        _viewMatrix = Matrix4.Identity;
        //MUST BE THIS: _viewProjectionMatrix = _projectionMatrix * _viewMatrix;
        _viewProjectionMatrix = _viewMatrix * _projectionMatrix;
    }

    private void RecalculateViewMatrix()
    {
        Matrix4 translation = Matrix4.CreateTranslation(_position);
        Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_rotation));
        Matrix4 transform = translation * rotation;
        _viewMatrix = transform.Inverted();
        //MUST BE THIS:
        //_viewProjectionMatrix = _projectionMatrix * _viewMatrix;
        _viewProjectionMatrix = _viewMatrix * _projectionMatrix;
    }

    public void SetProjectionMatrix(float left, float right, float bottom, float top)
    {
        _projectionMatrix = Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, -1, 1);
        RecalculateViewMatrix();
    }
}