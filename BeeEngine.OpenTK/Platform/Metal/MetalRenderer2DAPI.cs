using BeeEngine._2D;
using BeeEngine.Mathematics;
using BeeEngine.SmartPointers;

namespace BeeEngine.Platform.Metal;

public class MetalRenderer2DAPI: Renderer2DAPI
{
    public override void Init()
    {
        
    }

    public override void DrawRectangle(ref Vector3 position, ref Vector2 size, ref Vector4 color)
    {
        
    }

    public override void DrawRotatedRectangle(ref Vector3 position, ref Vector2 size, ref Vector4 color, float rotationInRadians)
    {
        
    }

    public override void DrawRectangle(ref Matrix4 transform, Color color)
    {
        
    }

    public override void SetCameraTransform(Matrix4 cameraMatrix)
    {
        
    }

    public override void SetCameraTransform(SharedPointer<Matrix4> cameraMatrix)
    {
        
    }

    public override void SetColor(int r, int g, int b, int a)
    {
        
    }

    public override void SetTexture2D(Texture2D texture2D)
    {
        
    }

    public override void DrawTexture2D(ref Vector3 position, ref Vector2 size, Texture2D texture, ref Vector4 color, float textureScale)
    {
        
    }

    public override void DrawRotatedTexture2D(ref Vector3 position, ref Vector2 size, Texture2D texture, ref Vector4 color,
        float textureScale, float rotationInRadians)
    {
        
    }

    public override void DrawRotatedTexture2D(ref Vector3 position, ref Vector2 size, Sprite sprite, ref Vector4 color, float textureScale,
        float rotationInRadians)
    {
        
    }

    public override void DrawTexture2D(ref Matrix4 transform, Texture2D texture, Vector4 color, float textureScale)
    {
        
    }

    public override void BeginScene()
    {
        
    }

    public override void EndScene()
    {
        
    }

    public override void Flush()
    {
        
    }

    public override void ResetStatistics()
    {
        
    }

    public override Renderer2D.Statistics GetStatistics()
    {
        return default;
    }
}