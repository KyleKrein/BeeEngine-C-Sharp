namespace BeeEngine.OpenTK.Renderer;

public class Renderer
{
    private static RendererAPI _rendererApi = RendererAPI.None;

    public static RendererAPI RendererAPI
    {
        get => _rendererApi;
        set
        {
            if (_rendererApi != RendererAPI.None)
                throw new Exception("Can't change Renderer api if it was assigned before");
            _rendererApi = value;
        }
    }
}