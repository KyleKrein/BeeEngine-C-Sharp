using System.Runtime.InteropServices;
using BeeEngine.OpenTK.Profiling;

namespace BeeEngine;

public static class Allocate
{
    [ProfileMethod]
    public static unsafe T* New<T>() where T: unmanaged
    {
        var ptr = (T*) NativeMemory.Alloc((nuint) sizeof(T));
        //*ptr = default;
        return ptr;
    }
    [ProfileMethod]
    public static unsafe void Delete<T>(T* obj) where T: unmanaged
    {
        NativeMemory.Free(obj);
    }
}