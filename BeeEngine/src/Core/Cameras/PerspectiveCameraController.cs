using BeeEngine.Events;
using BeeEngine.Mathematics;
using BeeEngine.OpenTK.Profiling;

namespace BeeEngine;

public class PerspectiveCameraController
{
    public float FieldOfView
    {
        get => Camera.Fov;
        set => Camera.Fov = value;
    }
    public PerspectiveCamera Camera { get; }

    public Vector3 Front => Camera.Front;
    public Vector3 Up => Camera.Up;
    public Vector3 Right => Camera.Right;

    public float Pitch
    {
        get => Camera.Pitch;
        set => Camera.Pitch = value;
    }

    public float Yaw
    {
        get => Camera.Yaw;
        set => Camera.Yaw = value;
    }

    public bool Enabled { get; private set; } = true;
    public bool Rotation { get; set; }
    private float _zoomLevel = 1.0f;
    private bool firstMouseInput = true;
    public Vector3 CameraPosition { get; set; } = Vector3.Zero;
    private float _cameraRotation = 0f;
    public float ZoomStep { get; set; } = .1f;
    public float MovementSpeed { get; set; } = 1;
    public float RotationSpeed { get; set; } = 90;
    public PerspectiveCameraController(float fov = 45, bool rotation = false): this(Application.Instance!.Width, Application.Instance.Height,fov, rotation) { }
    public PerspectiveCameraController(int width, int height,float fov, bool rotation = false)
    {
        Rotation = rotation;
        Camera = new PerspectiveCamera(fov, width, height);
    }
    private Vector2 lastPos = Vector2.Zero;
    public float Sensitivity { get; set; } = 0.5f;
    [ProfileMethod]
    public void OnUpdate()
    {
        if(!Enabled)
            return;
        //DebugTimer.Start();
        if (Input.KeyPressed(Key.W))
        {
            CameraPosition += Front * MovementSpeed * Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.A))
        {
            CameraPosition -=  Vector3.Normalize(Vector3.Cross(Front, Up)) * MovementSpeed * Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.D))
        {
            CameraPosition += Vector3.Normalize(Vector3.Cross(Front, Up)) * MovementSpeed * Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.S))
        {
            CameraPosition -= Front * MovementSpeed* Time.DeltaTime;
        }
        if (Input.KeyPressed(Key.Space))
        {
            CameraPosition += Up * MovementSpeed * Time.DeltaTime; //Up 
        }

        if (Input.KeyPressed(Key.LeftShift))
        {
            CameraPosition -= Up * MovementSpeed * Time.DeltaTime; //Down
        }

        if (firstMouseInput)
        {
            firstMouseInput = false;
            lastPos = Input.MousePosition;
        }
        else
        {
            float deltaX = Input.MousePosition.X - lastPos.X;
            float deltaY = Input.MousePosition.Y - lastPos.Y;
            lastPos = new Vector2(Input.MousePosition.X, Input.MousePosition.Y);

            Yaw += deltaX * Sensitivity;
            Pitch -= deltaY * Sensitivity;
        }
        /*if (Rotation)
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
        }*/

        Camera.Position = CameraPosition;
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
        //e.Dispatch<MouseScrolledEvent>(OnMouseScrolled);
        DebugTimer.End();
    }

    private bool OnWindowResized(WindowResizedEvent e)
    {
        DebugTimer.Start();
        Camera.SetProjectionMatrix(e.Width, e.Height, FieldOfView);
        DebugTimer.End();
        return false;
    }

    /*private bool OnMouseScrolled(MouseScrolledEvent e)
    {
        DebugTimer.Start();
        _zoomLevel -= ZoomStep*e.Offset;
        _zoomLevel = Math.Max(_zoomLevel, 0.1f);
        Camera.SetProjectionMatrix(-_aspectRation * _zoomLevel, _aspectRation * _zoomLevel, -_zoomLevel,
            _zoomLevel);
        MovementSpeed = _zoomLevel;
        DebugTimer.End();
        return true;
    }*/

    public void Enable()
    {
        if (Enabled)
        {
            return;
        }

        firstMouseInput = true;
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
    
    public static implicit operator Camera(PerspectiveCameraController controller)
    {
        return controller.Camera;
    }
}