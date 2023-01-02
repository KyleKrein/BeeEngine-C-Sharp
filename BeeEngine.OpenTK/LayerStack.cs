using BeeEngine.OpenTK.Events;
using BeeEngine.OpenTK.Gui;

namespace BeeEngine.OpenTK;

internal class LayerStack: IDisposable
{
    private LinkedList<Layer> _layers;

    private ImGuiLayer _guiLayer;
    //private int _layersInsert = 0;

    public LayerStack(ImGuiLayer guiLayer)
    {
        _guiLayer = guiLayer;
        _layers = new LinkedList<Layer>();
        _guiLayer.OnAttach();
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
        _guiLayer.OnDetach();
    }

    public void OnEvent(Event e)
    {
        EventDispatcher dispatcher = new EventDispatcher(e);
        _guiLayer.OnEvent(ref dispatcher);
        foreach (var layer in _layers.AsEnumerable().Reverse())
        {
            layer.OnEvent(ref dispatcher);
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
        _guiLayer.OnBegin();
        foreach (var layer in _layers)
        {
            layer.OnGUIRendering();
        }
        _guiLayer.OnGUIRendering();
        _guiLayer.OnEnd();
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