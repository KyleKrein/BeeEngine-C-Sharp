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
using Test.Implementations;

TestGame window = new TestGame("Pochemu", 640, 480);

window.Run();

window.Dispose();
