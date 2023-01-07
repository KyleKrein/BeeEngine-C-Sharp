namespace BeeEngine.SmartPointers;

/// <summary>
/// SharedRef class, which implements reference counted IDisposable ownership.
/// See also the static helper class for an easier construction syntax.
/// </summary>
public sealed class SharedRef<T> : IDisposable
    where T:class,IDisposable
{
    private SharedRefCounter<T> _t;

    /// <summary>
    /// Create a SharedRef directly from an object. Only use this once per object.
    /// After that, create SharedRefs from previous SharedRefs.
    /// </summary>
    /// <param name="t"></param>
    public SharedRef(T t)
    {
        _t = new SharedRefCounter<T>(t);
        _t.Retain();
    }

    /// <summary>
    /// Create a SharedRef from a previous SharedRef, incrementing the reference count.
    /// </summary>
    /// <param name="o"></param>
    public SharedRef(SharedRef<T> o)
    {
        o._t.Retain();
        _t = o._t;
    }

    public static SharedRef<T> Create(T t)
    {
        return new SharedRef<T>(t);
    }

    private bool _disposed = false;

    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            if (_t != null)
            {
                _t.Release();
                _t = null;
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    public T Get()
    {
        return _t.Get();
    }

    public SharedRef<T> Share()
    {
        return new SharedRef<T>(this);
    }
}