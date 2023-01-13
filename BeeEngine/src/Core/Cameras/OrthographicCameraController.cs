using BeeEngine.Mathematics;
using BeeEngine;
using BeeEngine.Events;
using BeeEngine.OpenTK.Profiling;

// ReSharper disable once CheckNamespace
namespace BeeEngine;

public class OrthographicCameraController
{
    private float _aspectRation;
    public OrthographicCamera Camera { get; }
    public bool Enabled { get; private set; } = true;
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
    [ProfileMethod]
    public void OnUpdate()
    {
        if(!Enabled)
            return;
        //DebugTimer.Start();
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
        //DebugTimer.End();
    }
    [ProfileMethod]
    public void OnEvent(ref EventDispatcher e)
    {
        e.Dispatch<WindowResizedEvent>(OnWindowResized);
        if (!Enabled)
        {
            return;
        }
        DebugTimer.Start();
        e.Dispatch<MouseScrolledEvent>(OnMouseScrolled);
        DebugTimer.End();
    }

    private bool OnWindowResized(WindowResizedEvent e)
    {
        DebugTimer.Start();
        _aspectRation = e.Width / (float) e.Height;
        Camera.SetProjectionMatrix(-_aspectRation * _zoomLevel, _aspectRation * _zoomLevel, -_zoomLevel,
            _zoomLevel);
        DebugTimer.End();
        return false;
    }

    private bool OnMouseScrolled(MouseScrolledEvent e)
    {
        DebugTimer.Start();
        _zoomLevel -= ZoomStep*e.Offset;
        _zoomLevel = Math.Max(_zoomLevel, 0.1f);
        Camera.SetProjectionMatrix(-_aspectRation * _zoomLevel, _aspectRation * _zoomLevel, -_zoomLevel,
            _zoomLevel);
        MovementSpeed = _zoomLevel;
        DebugTimer.End();
        return true;
    }

    public void Enable()
    {
        if (Enabled)
        {
            return;
        }

        Enabled = true;
    }

    public void Disable()
    {
        if (!Enabled)
        {
            return;
        }

        Enabled = false;
    }

    public static implicit operator OrthographicCamera(OrthographicCameraController controller)
    {
        return controller.Camera;
    }
}