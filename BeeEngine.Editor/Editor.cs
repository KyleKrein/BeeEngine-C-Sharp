using BeeEngine.Events;

namespace BeeEngine.Editor;

public sealed class Editor: Application
{
    private OrthographicCameraController _cameraController;
    protected override void OnEvent(ref EventDispatcher e)
    {
        _cameraController.OnEvent(ref e);
    }

    protected override void Initialize()
    {
        PushLayer(new EditorLayer());
        _cameraController = new OrthographicCameraController();
    }

    protected override void LoadContent()
    {
        
    }

    protected override void UnloadContent()
    {
        
    }

    protected override void Update()
    {
        Renderer2D.ResetStatistics();
        _cameraController.OnUpdate();
        Renderer2D.BeginScene(_cameraController);
    }

    protected override void FixedUpdate()
    {
        
    }

    protected override void Render()
    {
        Renderer2D.EndScene();
    }

    public Editor(WindowProps initSettings) : base(initSettings)
    {
    }
}