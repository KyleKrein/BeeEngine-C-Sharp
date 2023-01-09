#if IOS
using MetalKit;
using ObjCRuntime;

namespace BeeEngine.Platform.Metal;

internal class MetalView : MTKView
{
    public MetalView(NativeHandle handle)
    {
        
    }
    public override void Draw()
    {
        Delegate.Draw(this);
    }
}
#endif