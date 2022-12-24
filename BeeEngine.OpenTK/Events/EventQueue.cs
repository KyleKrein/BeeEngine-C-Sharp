namespace BeeEngine.OpenTK.Events;

internal sealed class EventQueue
{
    private readonly Queue<Event> _events;
    private readonly LayerStack _layerStack;
    private readonly object locker = new object();

    public EventQueue(LayerStack layerStack)
    {
        _events = new Queue<Event>();
        _layerStack = layerStack;
    }

    public void AddEvent(Event e)
    {
        lock (locker)
        {
            _events.Enqueue(e);
        }
    }

    public void Dispatch()
    {
        lock (locker)
        {
            while (_events.Count != 0)
            {
                var @event = _events.Dequeue();
                Input.OnEvent(@event);
                _layerStack.OnEvent(@event);
            }
        }
    }
}