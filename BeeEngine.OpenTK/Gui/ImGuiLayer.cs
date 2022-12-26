using BeeEngine.OpenTK.Events;
namespace BeeEngine.OpenTK.Gui;

internal abstract class ImGuiLayer : Layer
{
    public abstract void OnBegin();

    public abstract void OnEnd();
}