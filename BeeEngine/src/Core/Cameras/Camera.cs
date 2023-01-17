using BeeEngine.Mathematics;
using BeeEngine.SmartPointers;

namespace BeeEngine;

public abstract class Camera
{
    public abstract SharedPointer<Matrix4> GetViewProjectionMatrix();
}