using BeeEngine.Mathematics;
using BeeEngine.OpenTK.Events;
using BeeEngine.OpenTK.Renderer;

namespace BeeEngine.OpenTK;

public class OrthographicCameraController
{
    private float _aspectRation;
    public OrthographicCamera Camera { get; }
    public bool Rotation { get; set; }
    private float _zoomLevel = 1.0f;
    private Vector3 _cameraPosition = Vector3.Zero;
    private float _cameraRotation = 0f;
    public float ZoomStep { get; set; } = .1f;
    public float MovementSpeed { get; set; } = 1;
    public float RotationSpeed { get; set; } = 90;
    public OrthographicCameraController(bool rotation = false): this(Application.Instance!.Width, Application.Instance.Height, rotation) { }
    public OrthographicCameraController(int width, int height, bool rotation = false)
    {
        _aspectRation = width / (float)height;
        Rotation = rotation;
        Camera = new OrthographicCamera(-_aspectRation * _zoomLevel, _aspectRation * _zoomLevel, -_zoomLevel,
            _zoomLevel);
    }

    public void OnUpdate()
    {
        if (Input.KeyPressed(Key.W))
        {
            _cameraPosition.Y += MovementSpeed * Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.A))
        {
            _cameraPosition.X -= MovementSpeed* Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.D))
        {
            _cameraPosition.X += MovementSpeed* Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.S))
        {
            _cameraPosition.Y -= MovementSpeed* Time.DeltaTime;
        }

        if (Rotation)
        {
            if (Input.KeyPressed(Key.Q))
            {
                _cameraRotation += RotationSpeed * Time.DeltaTime;
            }
            if (Input.KeyPressed(Key.E))
            {
                _cameraRotation -= RotationSpeed* Time.DeltaTime;
            }

            Camera.Rotation = _cameraRotation;
        }

        Camera.Position = _cameraPosition;
    }

    public void OnEvent(ref EventDispatcher e)
    {
        e.Dispatch<MouseScrolledEvent>(OnMouseScrolled);
        e.Dispatch<WindowResizedEvent>(OnWindowResized);
    }

    private bool OnWindowResized(WindowResizedEvent e)
    {
        _aspectRation = e.Width / (float) e.Height;
        Camera.SetProjectionMatrix(-_aspectRation * _zoomLevel, _aspectRation * _zoomLevel, -_zoomLevel,
            _zoomLevel);
        return false;
    }

    private bool OnMouseScrolled(MouseScrolledEvent e)
    {
        _zoomLevel -= ZoomStep*e.Offset;
        _zoomLevel = Math.Max(_zoomLevel, 0.1f);
        Camera.SetProjectionMatrix(-_aspectRation * _zoomLevel, _aspectRation * _zoomLevel, -_zoomLevel,
            _zoomLevel);
        return false;
    }

    public static implicit operator OrthographicCamera(OrthographicCameraController controller)
    {
        return controller.Camera;
    }
}