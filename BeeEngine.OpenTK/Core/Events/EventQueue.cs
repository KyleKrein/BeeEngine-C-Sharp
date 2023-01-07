namespace BeeEngine.Events;

internal sealed class EventQueue
{
    private readonly Queue<Event> _events;
    private readonly LayerStack _layerStack;
    private readonly object locker = new object();
    Application _app = Application.Instance;

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
        DebugTimer.Start();
        lock (locker)
        {
            while (_events.Count != 0)
            {
                var @event = _events.Dequeue();
                EventDispatcher dispatcher = new EventDispatcher(@event);
                ApplicationOnEvent(ref dispatcher);
                Input.OnEvent(@event);
                _layerStack.OnEvent(ref dispatcher);
            }
        }
        DebugTimer.End();
    }

    private void ApplicationOnEvent(ref EventDispatcher e)
    {
        _app.Dispatch(ref e);
    }
}