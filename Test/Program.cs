using BeeEngine.Drawing;
using BeeEngine.UI;

Window window = new Window();
Texture archer = Texture.FromFile("archer_red.png");
int x = 0;
window.Paint += (sender, g) =>
{
    g.DrawTexture(archer, x++,0);
    var w = sender as Window;
    w.Invalidate();
};
Application.Run(window);
window.Dispose();
