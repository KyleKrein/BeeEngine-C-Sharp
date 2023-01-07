namespace BeeEngine;

internal abstract class Context
{
    public abstract int SwapInterval { get; set; }
    public abstract void Init();
    public abstract void SwapBuffers();
    public abstract void MakeCurrent();
    public abstract void MakeNonCurrent();
    public abstract void Destroy();
}