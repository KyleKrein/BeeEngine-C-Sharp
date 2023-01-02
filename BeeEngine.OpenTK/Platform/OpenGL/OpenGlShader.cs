using BeeEngine.Mathematics;
using BeeEngine.OpenTK.Renderer;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.OpenTK.Platform.OpenGL;

public class OpenGlShader: Shader
{
    private enum BeeShaderType
    {
        Vertex,
        Fragment,
    }
    int Handle;

    public OpenGlShader(string filepath)
    {
        var shaderSource = File.ReadAllLines(filepath);
        var result = Preprocess(shaderSource);
        Compile(result[BeeShaderType.Vertex], result[BeeShaderType.Fragment]);
    }
    public OpenGlShader(string vertexSrc, string fragmentSrc)
    {
        Compile(vertexSrc, fragmentSrc);
    }

    private void Compile(string vertexSrc, string fragmentSrc)
    {
        CompileVertexAndFragmentShaders(vertexSrc, fragmentSrc, out int VertexShader, out int FragmentShader);

        if (!CheckForCompilationErrors(VertexShader, FragmentShader))
        {
            GC.SuppressFinalize(this);
            return;
        }


        CombineShadersTogether(VertexShader, FragmentShader);

        CleanUp(VertexShader, FragmentShader);
    }

    private static BeeShaderType[] _shaderTypes = Enum.GetValues<BeeShaderType>();
    private Dictionary<BeeShaderType, string> Preprocess(string[] source)
    {
        Dictionary<BeeShaderType, string> result = new Dictionary<BeeShaderType, string>();
        foreach (var type in _shaderTypes)
        {
            result.Add(type, "");
        }
        List<int> indexes = new List<int>();
        for (int i = 0; i < source.Length; i++)
        {
            if (source[i].Contains("#type"))
            {
                indexes.Add(i);
            }
        }

        for (var index = 0; index < indexes.Count; index++)
        {
            var i = indexes[index];
            BeeShaderType type = (BeeShaderType) (-1);
            if (source[i].Contains("vertex"))
            {
                type = BeeShaderType.Vertex;
            }

            if (source[i].Contains("pixel") || source[i].Contains("fragment"))
            {
                type = BeeShaderType.Fragment;
            }
            Log.Assert((int)type != -1, "Doesn't support type {0}", source[i]);
            
            if (index == indexes.Count - 1)
            {
                result[type] = String.Concat(source[(i+1)..]);
                continue;
            }
            List<string> temp = new List<string>();
            for (int j = index+1; j < indexes[index + 1]; j++)
            {
                temp.Add(source[j]);
            }
            result[type] = String.Concat(temp);
        }

        foreach (var pair in result)
        {
            result[pair.Key] = pair.Value.Replace("\t", "\r\n   ");
        }
        return result;
    }

    private void CleanUp(int VertexShader, int FragmentShader)
    {
        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragmentShader);
        GL.DeleteShader(FragmentShader);
        GL.DeleteShader(VertexShader);
    }

    private void CombineShadersTogether(int VertexShader, int FragmentShader)
    {
        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragmentShader);

        GL.LinkProgram(Handle);

        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(Handle);
            Log.Error(infoLog);
        }
    }

    private bool CheckForCompilationErrors(int VertexShader, int FragmentShader)
    {
        bool compiledSuccessfully = true;
        GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(VertexShader);
            Log.Error(infoLog);
            GL.DeleteShader(VertexShader);
            compiledSuccessfully = false;
        }

        GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(FragmentShader);
            Log.Error(infoLog);
            GL.DeleteShader(FragmentShader);
            compiledSuccessfully = false;
        }

        return compiledSuccessfully;
    }

    private void CompileVertexAndFragmentShaders(string vertexShaderSource, string fragmentShaderSource, out int VertexShader, out int FragmentShader)
    {
        VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, vertexShaderSource);
        GL.CompileShader(VertexShader);

        FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, fragmentShaderSource);
        GL.CompileShader(FragmentShader);
    }

    public override void Bind()
    {
        GL.UseProgram(Handle);
    }

    public override void Unbind()
    {
        GL.UseProgram(0);
    }

    public override void UploadUniformMatrix4(string name, ref Matrix4 matrix4)
    {
        global::OpenTK.Mathematics.Matrix4 newMatrix = matrix4;
        var location = GL.GetUniformLocation(Handle, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.UniformMatrix4(location, false, ref newMatrix);
    }

    public override void UploadUniformFloat(string name, float value)
    {
        var location = GL.GetUniformLocation(Handle, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform1(location, value);
    }

    public override void UploadUniformFloat2(string name, Vector2 vector)
    {
        var location = GL.GetUniformLocation(Handle, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform2(location, vector);
    }

    public override void UploadUniformFloat3(string name, Vector3 vector)
    {
        var location = GL.GetUniformLocation(Handle, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform3(location, vector);
    }

    public override void UploadUniformFloat4(string name, Vector4 vector)
    {
        var location = GL.GetUniformLocation(Handle, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform4(location, vector);
    }

    public override void UploadUniformInt(string name, int value)
    {
        var location = GL.GetUniformLocation(Handle, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform1(location, value);
    }

    public override void UploadUniformInt2(string name, Vector2i vector)
    {
        var location = GL.GetUniformLocation(Handle, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform2(location, vector);
    }

    public override void UploadUniformInt3(string name, Vector3i vector)
    {
        var location = GL.GetUniformLocation(Handle, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform3(location, vector);
    }

    public override void UploadUniformInt4(string name, Vector4i vector)
    {
        var location = GL.GetUniformLocation(Handle, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform4(location, vector);
    }

    private bool _disposedValue = false;

    

    protected override void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            GL.DeleteProgram(Handle);

            _disposedValue = true;
        }
    }
}