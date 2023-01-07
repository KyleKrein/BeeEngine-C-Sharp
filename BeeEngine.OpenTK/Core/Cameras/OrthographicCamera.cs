using BeeEngine.Mathematics;
using BeeEngine.OpenTK.Profiling;
using BeeEngine.SmartPointers;

namespace BeeEngine;

public class OrthographicCamera: IDisposable
{
    private float _rotation;
    private Vector3 _position;
    private SharedPointer<Matrix4> _viewProjectionMatrix;
    //private Matrix4 _viewProjectionMatrix;
    private Matrix4 _projectionMatrix;
    private Matrix4 _viewMatrix;

    public ref Matrix4 ProjectionMatrix => ref _projectionMatrix;

    public ref Matrix4 ViewMatrix => ref _viewMatrix;

    public Matrix4 ViewProjectionMatrix => _viewProjectionMatrix.Get();

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
        _viewProjectionMatrix = new SharedPointer<Matrix4>();
        _viewProjectionMatrix.Get() = _viewMatrix * _projectionMatrix;
    }

    internal SharedPointer<Matrix4> GetViewProjectionMatrix()
    {
        return _viewProjectionMatrix.Share();
    }
    [ProfileMethod]
    private unsafe void RecalculateViewMatrix()
    {
        Matrix4 translation = Matrix4.CreateTranslation(_position);
        Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_rotation));
        Matrix4 transform = translation * rotation;
        _viewMatrix = transform.Inverted();
        //MUST BE THIS:
        //_viewProjectionMatrix = _projectionMatrix * _viewMatrix;
        _viewProjectionMatrix.Get() = _viewMatrix * _projectionMatrix;
    }
    [ProfileMethod]
    public void SetProjectionMatrix(float left, float right, float bottom, float top)
    {
        _projectionMatrix = Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, -1, 1);
        RecalculateViewMatrix();
    }

    private void ReleaseUnmanagedResources()
    {
        _viewProjectionMatrix.Release();
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~OrthographicCamera()
    {
        ReleaseUnmanagedResources();
    }
}