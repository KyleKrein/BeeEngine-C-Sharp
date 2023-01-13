using System.Runtime.CompilerServices;

namespace BeeEngine.SmartPointers;

internal sealed unsafe class SharedStruct<T>: IDisposable where T: unmanaged
{
    public readonly T* Ptr;
    public readonly object o = new object();
    public uint Counter;
    private bool _disposed = false;
    public ref T Ref => ref Unsafe.AsRef<T>(Ptr);

    public SharedStruct()
    {
        Ptr = Allocate.New<T>();
        Counter = 0;
    }
    /// <summary>
    /// Decrement the reference count, Dispose target if reaches 0
    /// </summary>
    public void Release()
    {
        lock (o)
        {
            checked
            {
                Counter--;
            }
            if (Counter == 0)
            {
                Dispose();
            }
        }
    }

    ~SharedStruct()
    {
        Dispose();
    }

    /// <summary>
    /// Increment the reference count
    /// </summary>
    public void Retain()
    {
        lock (o)
        {
            ++Counter;
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        Allocate.Delete(Ptr);
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}