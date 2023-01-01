namespace BeeEngine.OpenTK.Renderer;

public class Renderer
{
    private static API _api = API.None;

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

    public static void BeginScene()
    {
        
    }

    public static void EndScene()
    {
        
    }

    public static void Submit(VertexArray vertexArray)
    {
        vertexArray.Bind();
        RenderCommand.DrawIndexed(vertexArray);
    }
}