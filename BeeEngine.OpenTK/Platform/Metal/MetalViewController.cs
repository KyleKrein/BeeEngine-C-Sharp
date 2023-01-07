using CoreAnimation;
using CoreGraphics;
using Metal;
using MetalKit;
using ObjCRuntime;
using UIKit;

namespace BeeEngine.Platform.Metal;

internal class MetalViewController: UIViewController
{
    //private CADisplayLink timer;

    private NativeHandle _handle;
    public MetalViewController()
    {
    }
    public override void ViewDidLoad()
    {
        //INIT
        base.ViewDidLoad();
        //Context.Init();
    }
}