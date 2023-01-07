using BeeEngine;

namespace BeeEngine;

public class ShaderLibrary
{
    private readonly Dictionary<string, Shader> _shaders = new Dictionary<string, Shader>();

    public void Add(string name, Shader shader)
    {
        shader.Name = name;
        Add(shader);
    }
    public void Add(Shader shader)
    {
        Log.Assert(!_shaders.ContainsKey(shader.Name) && !_shaders.ContainsValue(shader), "Shader {0} already exists in Shader Library", shader.Name);
        _shaders.Add(shader.Name, shader);
    }

    public Shader Load(string filepath)
    {
        var shader = Shader.Create(filepath);
        Add(shader);
        return shader;
    }
    public Shader Load(string name, string filepath)
    {
        var shader = Shader.Create(name, filepath);
        Add(shader);
        return shader;
    }

    public Shader Get(string name)
    {
        Log.Assert(_shaders.ContainsKey(name), "Shader {0} doesn't exist", name);
        return _shaders[name];
    }
}