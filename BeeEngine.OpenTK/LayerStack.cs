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
            layer.OnDetach();
        }
    }

    public void OnEvent(Event e)
    {
        EventDispatcher dispatcher = new EventDispatcher(e);
        foreach (var layer in _layers.AsEnumerable().Reverse())
        {
            layer.OnEvent(dispatcher);
            if (e.IsHandled)
            {
                break;
            }
        }
    }

    public void Update()
    {
        foreach (var layer in _layers)
        {
            layer.OnUpdate();
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