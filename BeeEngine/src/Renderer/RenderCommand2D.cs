using BeeEngine.Mathematics;
using BeeEngine;
using BeeEngine.Platform.Metal;

namespace BeeEngine;

public static class RenderCommand2D
{
    private static Renderer2DAPI _rendererApi;

    static RenderCommand2D()
    {
        
    }
    public static void Init()
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                _rendererApi = new OpenGLRenderer2DAPI();
                break;
            case API.Metal:
                #if IOS
                _rendererApi = new MetalRenderer2DAPI();
                #endif
                break;
        }
        Log.AssertAndThrow(_rendererApi != null, "Unknown rendering api!");
        _rendererApi.Init();
    }

    public static void SetCamera(Camera camera)
    {
        var ptr = camera.GetViewProjectionMatrix();
        _rendererApi.SetCameraTransform(ptr.Get());
        ptr.Release();
    }

    public static void DrawRectangle(float x, float y, float z, float width, float height, Color color, float rotationInRadians)
    {
        /*Matrix4 transform;
        if (rotationInRadians == 0f)
        {
            transform = Matrix4.CreateTranslation(x, y, z) * Matrix4.CreateScale(width, height, 1);
        }
        else
        {
            transform = Matrix4.CreateTranslation(x, y, z) * Matrix4.CreateRotationZ(rotationInRadians) * Matrix4.CreateScale(width, height, 1);
        }*/
        //_rendererApi.DrawRectangle(ref transform, color);
        var position = new Vector3(x, y, z);
        var size = new Vector2(width, height);
        var color1 = (Vector4)color;
        _rendererApi.DrawRotatedRectangle(ref position, ref size, ref color1, rotationInRadians);
        /*if (rotationInRadians == 0f)
        {
            _rendererApi.DrawRectangle(ref position, ref size, ref color1);
        }
        else
        {
            _rendererApi.DrawRotatedRectangle(ref position, ref size, ref color1, rotationInRadians);
        }*/
        
    }

    public static void DrawTexture2D(float x, float y, float z, float width, float height, Texture2D texture)
    {
        DrawTexture2D(x, y, z, width, height, texture, Color.White, 1, 0);
    }
    public static void DrawTexture2D(float x, float y, float z, float width, float height, Texture2D texture, Color color, float textureScale, float rotationInRadians)
    {
        var position = new Vector3(x, y, z);
        var size = new Vector2(width, height);
        var colorVector = (Vector4)color;
        _rendererApi.DrawRotatedTexture2D(ref position, ref size, texture, ref colorVector, textureScale, rotationInRadians);
        /*
        if (rotationInRadians == 0.0f)
        {
            _rendererApi.DrawTexture2D(ref position, ref size, texture, ref colorVector, textureScale);
        }
        else
        {
            _rendererApi.DrawRotatedTexture2D(ref position, ref size, texture, ref colorVector, textureScale, rotationInRadians);
        }
       */
        /*Matrix4 transform;
        if (rotationInRadians == 0f)
        {
            transform = Matrix4.CreateTranslation(x, y, z) * Matrix4.CreateScale(width, height, 1);
        }
        else
        {
            transform = Matrix4.CreateTranslation(x, y, z) * Matrix4.CreateRotationZ(rotationInRadians) * Matrix4.CreateScale(width, height, 1);
        }
        _rendererApi.DrawTexture2D(ref transform, texture, (Vector4) color, 1);*/
    }
    public static void DrawTexture2D(float x, float y, float width, float height, Texture2D texture)
    {
        DrawTexture2D(x, y, 0, width , height, texture, Color.White, 1f, 0f);
    }
    public static void DrawTexture2D(float x, float y, float width, float height, Texture2D texture, Color color)
    {
        DrawTexture2D(x, y, 0, width, height, texture, color, 1f, 0f);
    }

    public static void BeginScene()
    {
        _rendererApi.BeginScene();
    }

    public static void EndScene()
    {
        _rendererApi.EndScene();
    }

    public static void ResetStatistics()
    {
        _rendererApi.ResetStatistics();
    }

    public static Renderer2D.Statistics GetStatistics()
    {
        return _rendererApi.GetStatistics();
    }
}