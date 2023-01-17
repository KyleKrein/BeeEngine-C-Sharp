using BeeEngine.Mathematics;
using BeeEngine.SmartPointers;

namespace BeeEngine;

public class PerspectiveCamera: Camera, IDisposable
{
    private Matrix4 PerspectiveProjectionMatrix;
    // Those vectors are directions pointing outwards from the camera to define how it rotated.
        private Vector3 _front = -Vector3.UnitZ;

        private Vector3 _up = Vector3.UnitY;

        private Vector3 _right = Vector3.UnitX;
        private SharedPointer<Matrix4> _viewProjectionMatrix;

        // Rotation around the X axis (radians)
        private float _pitch;

        // Rotation around the Y axis (radians)
        private float _yaw = -MathU.PiOver2; // Without this, you would be started rotated 90 degrees right.

        // The field of view of the camera (radians)
        private float _fov = MathU.PiOver2;
        private Vector3 _position;

        public PerspectiveCamera(float fieldOfView, float width, float height, 
            float depthNear = 0.1f, float depthFar = 100.0f)
        {
            _viewProjectionMatrix = new SharedPointer<Matrix4>();
            SetProjectionMatrix(width, height, fieldOfView, depthNear, depthFar);
            Position = Vector3.Zero;
        }

        // The position of the camera
        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                RecalculateViewProjection();
            }
        }

        // This is simply the aspect ratio of the viewport, used for the projection matrix.
        public float AspectRatio { get; private set; }

        public Vector3 Front => _front;

        public Vector3 Up => _up;

        public Vector3 Right => _right;

        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float Pitch
        {
            get => MathU.RadiansToDegrees(_pitch);
            set
            {
                // We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
                // of weird "bugs" when you are using euler angles for rotation.
                // If you want to read more about this you can try researching a topic called gimbal lock
                var angle = MathU.Clamp(value, -89f, 89f);
                _pitch = MathU.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float Yaw
        {
            get => MathU.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathU.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        // The field of view (FOV) is the vertical angle of the camera view.
        // This has been discussed more in depth in a previous tutorial,
        // but in this tutorial, you have also learned how we can use this to simulate a zoom feature.
        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float Fov
        {
            get => MathU.RadiansToDegrees(_fov);
            set
            {
                var angle = MathU.Clamp(value, 1f, 90f);
                _fov = MathU.DegreesToRadians(angle);
            }
        }

        // Get the view matrix using the amazing LookAt function described more in depth on the web tutorials
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }

        // Get the projection matrix using the same method we have used up until this point
        public Matrix4 GetProjectionMatrix()
        {
            return PerspectiveProjectionMatrix;
        }

        // This function is going to update the direction vertices using some of the math learned in the web tutorials.
        private void UpdateVectors()
        {
            // First, the front matrix is calculated using some basic trigonometry.
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            // We need to make sure the vectors are all normalized, as otherwise we would get some funky results.
            _front = Vector3.Normalize(_front);

            // Calculate both the right and the up vector using cross product.
            // Note that we are calculating the right from the global up; this behaviour might
            // not be what you need for all cameras so keep this in mind if you do not want a FPS camera.
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
            RecalculateViewProjection();
        }

        public void SetProjectionMatrix(float width, float height, float fieldOfView, float depthNear = 0.1f, float depthFar = 100.0f)
        {
            PerspectiveProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathU.DegreesToRadians(fieldOfView),
                width / height, depthNear, depthFar);
            AspectRatio = width / height;
            RecalculateViewProjection();
        }

        private void RecalculateViewProjection()
        {
            _viewProjectionMatrix.GetRef() = GetViewMatrix() * PerspectiveProjectionMatrix;
        }

        public override SharedPointer<Matrix4> GetViewProjectionMatrix()
        {
            return _viewProjectionMatrix.Share();
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                _viewProjectionMatrix.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PerspectiveCamera()
        {
            Dispose(false);
        }
}