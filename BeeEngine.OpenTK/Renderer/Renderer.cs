using BeeEngine.Mathematics;
using BeeEngine;
using BeeEngine.OpenTK.Profiling;

namespace BeeEngine;

public class Renderer
{
    private static API _api = API.None;
    private static Matrix4 _viewProjectionMatrix;

    public static ShaderLibrary Shaders { get; } = new ShaderLibrary();

    // ReSharper disable once InconsistentNaming
    public static API API
    {
        get => _api;
        set
        {
            if (_api != API.None)
                throw new Exception("Can't change Renderer api if it was assigned before");
            _api = value;
        }
    }

    public static void BeginScene(OrthographicCamera camera)
    {
        _viewProjectionMatrix = camera.ViewProjectionMatrix;
    }

    public static void EndScene()
    {
        
    }
    [ProfileMethod]
    public static void Submit(VertexArray vertexArray, Shader shader, Matrix4 transform, Action<Shader> func = null)
    {
        shader.Bind();
        shader.UploadUniformMatrix4("u_ViewProjection",ref _viewProjectionMatrix);
        shader.UploadUniformMatrix4("u_Transform",ref transform);
        func?.Invoke(shader);
        vertexArray.Bind();
        RenderCommand.DrawIndexed(vertexArray);
    }
    [ProfileMethod]
    public static void Init()
    {
        RenderCommand.Init();
        Renderer2D.Init();
    }
    [ProfileMethod]
    internal static void OnWindowResized(int width, int height)
    {
        RenderCommand.SetViewPort(0, 0, width, height);
    }
}