using BeeEngine.Mathematics;

namespace BeeEngine.OpenTK.Renderer;

public abstract class Renderer2DAPI
{
    public abstract void Init();
    public abstract void DrawRectangle(ref Matrix4 transform, Color color);

    public abstract void SetCameraTransform(Matrix4 cameraMatrix);
    public abstract void SetColor(int r, int g, int b, int a);
    public abstract void SetTexture2D(Texture2D texture2D);

    public abstract void DrawTexture2D(ref Matrix4 transform, Texture2D texture);
}