using BeeEngine.OpenTK;
using BeeEngine.OpenTK.Gui;
using ImGuiNET;

namespace Test.Implementations;

public class TestGame: Game
{
    public TestGame(string title, int width, int height) : base(title, width, height)
    {
    }

    protected override void UnloadResources()
    {
        
    }

    protected override void Initialize()
    {
        PushOverlay(new ImGuiLayer());
    }

    protected override void LoadContent()
    {
        
    }

    protected override void Update()
    {
        
    }

    protected override void Render()
    {
        ImGui.ShowDemoWindow();
    }
}