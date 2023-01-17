using BeeEngine.Mathematics;
using BeeEngine;
using BeeEngine.OpenTK.Profiling;
using BeeEngine.SmartPointers;

namespace BeeEngine;

public class Renderer
{
    private static API _api = API.None;
    private static SharedPointer<Matrix4> _viewProjectionMatrix;

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

    public static void BeginScene(Camera camera)
    {
        _viewProjectionMatrix = camera.GetViewProjectionMatrix();
    }

    public static void EndScene()
    {
        _viewProjectionMatrix.Release();
    }
    [ProfileMethod]
    public static unsafe void Submit(VertexArray vertexArray, Shader shader, Matrix4 transform, Action<Shader> func = null)
    {
        shader.Bind();
        shader.UploadUniformMatrix4("u_ViewProjection",_viewProjectionMatrix.GetPtr());
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