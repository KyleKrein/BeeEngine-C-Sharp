using MetalKit;

namespace BeeEngine.IOS;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override UIWindow? Window { get; set; }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // create a new window instance based on the screen size
        Window = new UIWindow(UIScreen.MainScreen.Bounds);

        // create a UIViewController with a single UILabel
        var vc = new MetalViewController(Window.Handle);
        /*vc.View!.AddSubview(new UILabel(Window!.Frame)
        {
            BackgroundColor = UIColor.SystemBackground,
            TextAlignment = UITextAlignment.Center,
            Text = "Hello, iOS!",
            AutoresizingMask = UIViewAutoresizing.All,
        });*/
        Window.RootViewController = vc;
        // make the window visible
        Window.MakeKeyAndVisible();
        
        //Window.WillDrawLayer(vc._layer);
        //((MTKView) vc.View).Delegate.Draw((MTKView) vc.View);
        return true;
    }
}