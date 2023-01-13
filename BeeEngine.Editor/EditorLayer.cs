using BeeEngine.Events;
using BeeEngine.Gui;
using BeeEngine.Mathematics;
using ImGuiNET;
using Vector2 = System.Numerics.Vector2;

namespace BeeEngine.Editor;

public class EditorLayer : Layer
{
    private FpsCounter _fpsCounter;
    private ViewPort _viewPort;
    public override void OnAttach()
    {
        _fpsCounter = new FpsCounter();
        _viewPort = new ViewPort(100, 100)
        {
            Func = DrawToScene
        };
        
    }

    private void DrawToScene()
    {
        Renderer2D.DrawRectangle(0,0,1,1,Color.Green);
    }

    public override void OnEvent(ref EventDispatcher e)
    {
        _viewPort.OnEvent(ref e);
    }

    public override void OnGUIRendering()
    {
        ShowExampleAppDockSpace();
        _viewPort.Render();
        _fpsCounter.Render();
    }

    public override void OnUpdate()
    {
        _viewPort.Update();
        _fpsCounter.Update();
    }

    public override void OnDetach()
    {
        
    }


    //-----------------------------------------------------------------------------
// [SECTION] Example App: Docking, DockSpace / ShowExampleAppDockSpace()
//-----------------------------------------------------------------------------
// Demonstrate using DockSpace() to create an explicit docking node within an existing window.
// Note: You can use most Docking facilities without calling any API. You DO NOT need to call DockSpace() to use Docking!
// - Drag from window title bar or their tab to dock/undock. Hold SHIFT to disable docking.
// - Drag from window menu button (upper-left button) to undock an entire node (all windows).
// - When io.ConfigDockingWithShift == true, you instead need to hold SHIFT to _enable_ docking/undocking.
// About dockspaces:
// - Use DockSpace() to create an explicit dock node _within_ an existing window.
// - Use DockSpaceOverViewport() to create an explicit dock node covering the screen or a specific viewport.
//   This is often used with ImGuiDockNodeFlags_PassthruCentralNode.
// - Important: Dockspaces need to be submitted _before_ any window they can host. Submit it early in your frame! (*)
// - Important: Dockspaces need to be kept alive if hidden, otherwise windows docked into it will be undocked.
//   e.g. if you have multiple tabs with a dockspace inside each tab: submit the non-visible dockspaces with ImGuiDockNodeFlags_KeepAliveOnly.
// (*) because of this constraint, the implicit \"Debug\" window can not be docked into an explicit DockSpace() node,
// because that window is submitted as part of the part of the NewFrame() call. An easy workaround is that you can create
// your own implicit "Debug##2" window after calling DockSpace() and leave it in the window stack for anyone to use.
    unsafe void ShowExampleAppDockSpace(bool p_open = true)
    {
        // If you strip some features of, this demo is pretty much equivalent to calling DockSpaceOverViewport()!
        // In most cases you should be able to just call DockSpaceOverViewport() and ignore all the code below!
        // In this specific demo, we are not using DockSpaceOverViewport() because:
        // - we allow the host window to be floating/moveable instead of filling the viewport (when opt_fullscreen == false)
        // - we allow the host window to have padding (when opt_padding == true)
        // - we have a local menu bar in the host window (vs. you could use BeginMainMenuBar() + DockSpaceOverViewport() in your code!)
        // TL;DR; this demo is more complicated than what you would normally use.
        // If we removed all the options we are showcasing, this demo would become:
        //     void ShowExampleAppDockSpace()
        //     {
        //         ImGui::DockSpaceOverViewport(ImGui::GetMainViewport());
        //     }
        bool opt_fullscreen = true;
        bool opt_padding = false;
        ImGuiDockNodeFlags dockspace_flags = ImGuiDockNodeFlags.None;
        // We are using the ImGuiWindowFlags_NoDocking flag to make the parent window not dockable into,
        // because it would be confusing to have two docking targets within each others.
        ImGuiWindowFlags window_flags = ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoDocking;
        if (opt_fullscreen)
        {
            ImGuiViewport* viewport = ImGui.GetMainViewport();
            ImGui.SetNextWindowPos(viewport->WorkPos);
            ImGui.SetNextWindowSize(viewport->WorkSize);
            ImGui.SetNextWindowViewport(viewport->ID);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
            window_flags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize |
                            ImGuiWindowFlags.NoMove;
            window_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;
        }
        else
        {
            dockspace_flags &= ~ImGuiDockNodeFlags.PassthruCentralNode;
        }

        // When using ImGuiDockNodeFlags_PassthruCentralNode, DockSpace() will render our background
        // and handle the pass-thru hole, so we ask Begin() to not render a background.
        if (dockspace_flags.HasFlag(ImGuiDockNodeFlags.PassthruCentralNode))
            window_flags |= ImGuiWindowFlags.NoBackground;
        // Important: note that we proceed even if Begin() returns false (aka window is collapsed).
        // This is because we want to keep our DockSpace() active. If a DockSpace() is inactive,
        // all active windows docked into it will lose their parent and become undocked.
        // We cannot preserve the docking relationship between an active window and an inactive docking, otherwise
        // any change of dockspace/settings would lead to windows being stuck in limbo and never being visible.
        if (!opt_padding)
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0.0f, 0.0f));
        ImGui.Begin("DockSpace Demo", ref p_open, window_flags);
        if (!opt_padding)
            ImGui.PopStyleVar();
        if (opt_fullscreen)
            ImGui.PopStyleVar(2);
        // Submit the DockSpace
        ImGuiIOPtr io = ImGui.GetIO();
        if (io.ConfigFlags.HasFlag(ImGuiConfigFlags.DockingEnable))
        {
            uint dockspace_id = ImGui.GetID("MyDockSpace");
            ImGui.DockSpace(dockspace_id, new Vector2(0.0f, 0.0f), dockspace_flags);
        }

        if (ImGui.BeginMenuBar())
        {
            if (ImGui.BeginMenu("Options"))
            {
                // Disabling fullscreen would allow the window to be moved to the front of other windows,
                // which we can't undo at the moment without finer window depth/z control.
                ImGui.MenuItem("Fullscreen", null, opt_fullscreen);
                ImGui.MenuItem("Padding", null, opt_padding);
                ImGui.Separator();
                if (ImGui.MenuItem("Flag: NoSplit", "", dockspace_flags.HasFlag(ImGuiDockNodeFlags.NoSplit)))
                {
                    dockspace_flags ^= ImGuiDockNodeFlags.NoSplit;
                }

                if (ImGui.MenuItem("Flag: NoResize", "", (dockspace_flags.HasFlag(ImGuiDockNodeFlags.NoResize))))
                {
                    dockspace_flags ^= ImGuiDockNodeFlags.NoResize;
                }

                if (ImGui.MenuItem("Flag: NoDockingInCentralNode", "",
                        (dockspace_flags.HasFlag(ImGuiDockNodeFlags.NoDockingInCentralNode))))
                {
                    dockspace_flags ^= ImGuiDockNodeFlags.NoDockingInCentralNode;
                }

                if (ImGui.MenuItem("Flag: AutoHideTabBar", "",
                        (dockspace_flags.HasFlag(ImGuiDockNodeFlags.AutoHideTabBar))))
                {
                    dockspace_flags ^= ImGuiDockNodeFlags.AutoHideTabBar;
                }

                if (ImGui.MenuItem("Flag: PassthruCentralNode", "",
                        (dockspace_flags.HasFlag(ImGuiDockNodeFlags.PassthruCentralNode)), opt_fullscreen))
                {
                    dockspace_flags ^= ImGuiDockNodeFlags.PassthruCentralNode;
                }

                ImGui.Separator();
                ImGui.EndMenu();
            }

            ImGui.EndMenuBar();
        }

        ImGui.End();
    }
}