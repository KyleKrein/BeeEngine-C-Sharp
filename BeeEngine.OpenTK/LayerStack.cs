using BeeEngine.OpenTK.Events;

namespace BeeEngine.OpenTK;

internal class LayerStack: IDisposable
{
    private LinkedList<Layer> _layers;
    //private int _layersInsert = 0;

    public LayerStack()
    {
        _layers = new LinkedList<Layer>();
    }
    public void PushLayer(Layer layer)
    {
        _layers.AddFirst(layer);
        layer.OnAttach();
        //_layersInsert++;
    }

    public void PushOverlay(Layer overlay)
    {
        _layers.AddLast(overlay);
        overlay.OnAttach();
    }

    public void PopLayer(Layer layer)
    {
        _layers.Remove(layer);
        layer.OnDetach();
    }

    public void PopOverlay(Layer overlay)
    {
        _layers.Remove(overlay);
        overlay.OnDetach();
    }

    private void ReleaseUnmanagedResources()
    {
        foreach (var layer in _layers)
        {
            layer.Dispose();
        }
    }

    public void OnEvent(IEvent e)
    {
        foreach (var layer in _layers.AsEnumerable().Reverse())
        {
            layer.OnEvent(e);
        }
    }

    public void Update(Time gameTime)
    {
        foreach (var layer in _layers)
        {
            layer.OnUpdate(gameTime);
        }
    }
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~LayerStack()
    {
        ReleaseUnmanagedResources();
    }
}