using BeeEngine.OpenTK.Events;

namespace BeeEngine.OpenTK;

public abstract class Layer: IDisposable
{
    public virtual void OnAttach()
    {
        
    }

    public virtual void OnDetach()
    {
        
    }

    public virtual void OnUpdate(Time gameTime)
    {
        
    }

    protected virtual void Dispose(bool disposing)
    {
        
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual void OnEvent(EventDispatcher e)
    {
        
    }
}