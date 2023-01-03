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

    public static void DrawRectangle(float x, float y, float width, float height, Color color)
    {
        Matrix4 transform = Matrix4.CreateTranslation(x, y, 0) * Matrix4.CreateScale(width, height, 1);
        _rendererApi.DrawRectangle(ref transform, color);
    }

    public static void DrawTexture2D(float x, float y, float width, float height, Texture2D texture)
    {
        Matrix4 transform = Matrix4.CreateTranslation(x, y, 0) * Matrix4.CreateScale(width, height, 1);
        _rendererApi.DrawTexture2D(ref transform, texture);
    }
}