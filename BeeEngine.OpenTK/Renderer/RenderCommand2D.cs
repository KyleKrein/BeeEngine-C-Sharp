using BeeEngine.Mathematics;

namespace BeeEngine.OpenTK.Renderer;

public static class RenderCommand2D
{
    private static Renderer2DAPI _rendererApi;

    static RenderCommand2D()
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                _rendererApi = new OpenGLRenderer2DAPI();
                return;
        }
        Log.AssertAndThrow(false, "Unknown rendering api!");
    }
    public static void Init()
    {
        _rendererApi.Init();
    }

    public static void SetCamera(OrthographicCamera camera)
    {
        _rendererApi.SetCameraTransform(camera.ViewProjectionMatrix);
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
        _rendererApi.DrawRectangle(ref position, ref size, ref color1);
    }

    public static void DrawTexture2D(float x, float y, float z, float width, float height, Texture2D texture)
    {
        DrawTexture2D(x, y, z, width, height, texture, Color.White, 1, 0);
    }
    public static void DrawTexture2D(float x, float y, float z, float width, float height, Texture2D texture, Color color, float textureScale, float rotationInRadians)
    {
        Matrix4 transform;
        if (rotationInRadians == 0f)
        {
            transform = Matrix4.CreateTranslation(x, y, z) * Matrix4.CreateScale(width, height, 1);
        }
        else
        {
            transform = Matrix4.CreateTranslation(x, y, z) * Matrix4.CreateRotationZ(rotationInRadians) * Matrix4.CreateScale(width, height, 1);
        }
        _rendererApi.DrawTexture2D(ref transform, texture, (Vector4) color, 1);
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
}