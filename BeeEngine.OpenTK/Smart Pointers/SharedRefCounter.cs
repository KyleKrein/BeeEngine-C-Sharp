namespace BeeEngine.SmartPointers;

/// <summary>
/// Class which holds the reference count for a shared object.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class SharedRefCounter<T> where T : class,IDisposable
{
    private int _count;
    private readonly T _t;

    public T Get()
    {
        return _t;
    }

    public SharedRefCounter(T t)
    {
        _count = 0;
        _t = t;
    }

    /// <summary>
    /// Decrement the reference count, Dispose target if reaches 0
    /// </summary>
    public void Release()
    {
        lock (_t)
        {
            if (--_count == 0)
            {
                _t.Dispose();
            }
        }
    }

    /// <summary>
    /// Increment the reference count
    /// </summary>
    public void Retain()
    {
        lock (_t)
        {
            ++_count;
        }
    }
}