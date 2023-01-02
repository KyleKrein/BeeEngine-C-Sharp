using BeeEngine.OpenTK.Events;
using BeeEngine.OpenTK.Renderer;

namespace BeeEngine.OpenTK;

public abstract class Layer
{
    public virtual void OnAttach()
    {
        
    }

    public virtual void OnDetach()
    {
        
    }

    public virtual void OnUpdate()
    {
        
    }

    public virtual void OnGUIRendering()
    {
        
    }

    public virtual void OnEvent(ref EventDispatcher e)
    {
        
    }
    //Work with shader library
    protected Shader LoadShader(string filepath)
    {
        return Renderer.Renderer.Shaders.Load(filepath);
    }
    protected Shader LoadShader(string name, string filepath)
    {
        return Renderer.Renderer.Shaders.Load(filepath);
    }

    protected void AddShader(string name, Shader shader)
    {
        Renderer.Renderer.Shaders.Add(name, shader);
    }
    protected void AddShader(Shader shader)
    {
        Renderer.Renderer.Shaders.Add(shader);
    }

    protected Shader GetShader(string name)
    {
        return Renderer.Renderer.Shaders.Get(name);
    }
}