
// This is the main entry point of the application.
// If you want to use a different Application Delegate class from "AppDelegate"
// you can specify it here.

using BeeEngine;
using TestiOS;
Application.SetPlatformOS(OS.IOS);
using (var App = new App(new WindowProps()))
{
    App.Run();
}