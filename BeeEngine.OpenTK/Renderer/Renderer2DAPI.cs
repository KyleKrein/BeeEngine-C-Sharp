using BeeEngine.Mathematics;
using BeeEngine;
using BeeEngine._2D;
using BeeEngine.SmartPointers;

namespace BeeEngine;

public abstract class Renderer2DAPI
{
    public abstract void Init();
    public abstract void DrawRectangle(ref Vector3 position, ref Vector2 size, ref Vector4 color);
    public abstract void DrawRotatedRectangle(ref Vector3 position, ref Vector2 size, ref Vector4 color, float rotationInRadians);
    public abstract void DrawRectangle(ref Matrix4 transform, Color color);

    public abstract void SetCameraTransform(Matrix4 cameraMatrix);
    public abstract void SetCameraTransform(SharedPointer<Matrix4> cameraMatrix);
    public abstract void SetColor(int r, int g, int b, int a);
    public abstract void SetTexture2D(Texture2D texture2D);

    public abstract void DrawTexture2D(ref Vector3 position, ref Vector2 size, Texture2D texture, ref Vector4 color,
        float textureScale);
    public abstract void DrawRotatedTexture2D(ref Vector3 position, ref Vector2 size, Texture2D texture, ref Vector4 color,
        float textureScale, float rotationInRadians);
    public abstract void DrawRotatedTexture2D(ref Vector3 position, ref Vector2 size, Sprite sprite, ref Vector4 color,
        float textureScale, float rotationInRadians);

    public abstract void DrawTexture2D(ref Matrix4 transform, Texture2D texture, Vector4 color, float textureScale);
    public abstract void BeginScene();
    public abstract void EndScene();
    public abstract void Flush();
    public abstract void ResetStatistics();
    public abstract Renderer2D.Statistics GetStatistics();
}