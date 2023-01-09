namespace BeeEngine.SmartPointers;

public struct SharedPointer<T>: IDisposable where T: unmanaged
{
    private SharedStruct<T> _obj;

    public readonly ref T Get()
    {
        return ref _obj.Ref;
    }

    public unsafe T* GetPtr()
    {
        return _obj.Ptr;
    }

    public unsafe IntPtr GetIntPtr()
    {
        return (IntPtr)_obj.Ptr;
    }
    public SharedPointer()
    {
        _obj = new SharedStruct<T>();
        _obj.Retain();
    }
    public unsafe SharedPointer(ref T obj)
    {
        _obj = new SharedStruct<T>();
        *_obj.Ptr = obj;
        _obj.Retain();
    }

    public readonly SharedPointer<T> Share()
    {
        return new SharedPointer<T>(this);
    }

    public unsafe void Reset(ref T obj)
    {
        _obj.Release();
        _obj = new SharedStruct<T>();
        *_obj.Ptr = obj;
        _obj.Retain();
    }

    public void Reset(SharedPointer<T> o)
    {
        if(o._obj == _obj)
            return;
        _obj.Release();
        _obj = o._obj;
        _obj.Retain();
    }

    public SharedPointer(SharedPointer<T> other)
    {
        _obj = other._obj;
        _obj.Retain();
    }

    private bool _released = false;

    public void Dispose()
    {
        Release();
    }
    public void Release()
    {
        if(_released)
            return;
        _obj.Release();
        _released = true;
    }
}

/// <summary>
/// Static helper class for easier construction syntax.
/// </summary>
public static class SharedRef
{
    /// <summary>
    /// Create a SharedRef directly from an object. Only use this once per object.
    /// After that, create SharedRefs from previous SharedRefs.
    /// </summary>
    /// <param name="t"></param>
    public static SharedRef<T> Create<T>(T t) where T : class,IDisposable
    {
        return new SharedRef<T>(t);
    }

    /// <summary>
    /// Create a SharedRef from a previous SharedRef, incrementing the reference count.
    /// </summary>
    /// <param name="o"></param>
    public static SharedRef<T> Create<T>(SharedRef<T> o) where T : class,IDisposable
    {
        return new SharedRef<T>(o);
    }

    public static SharedPointer<T> CreatePtr<T>(ref T obj) where T: unmanaged
    {
        return new SharedPointer<T>(ref obj);
    }
}