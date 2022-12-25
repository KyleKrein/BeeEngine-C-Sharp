/*
using BeeEngine.Drawing;
using BeeEngine.UI;
Window window = new Window();

Texture archer = null;
int x = 0;
window.Load += (sender, e) =>
{
    archer = Texture.FromFile("archer_red.png");
};
window.Paint += (sender, g) =>
{
    g.DrawTexture(archer!, x++,0);
    var w = sender as Window;
    //w.Invalidate();
};
Application.Run(window);
window.Dispose();
*/


using BeeEngine.OpenTK;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Test.Implementations;

GLFW.WindowHint(WindowHintBool.OpenGLForwardCompat, true);
TestGame window = new TestGame("Pochemu", 1280, 720);

window.Run();

window.Dispose();
