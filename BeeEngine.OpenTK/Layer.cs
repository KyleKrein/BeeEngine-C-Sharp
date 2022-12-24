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

    public virtual void OnUpdate()
    {
        
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual void OnEvent(Event e)
    {
        switch (e)
        {
            case MouseClick:
                
                break;
        }
    }
}