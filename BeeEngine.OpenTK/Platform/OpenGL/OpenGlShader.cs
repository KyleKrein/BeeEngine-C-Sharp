using System.Diagnostics;
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
    int _programId;

    public OpenGlShader(string name, string filepath)
    {
        Name = name;
        var shaderSource = File.ReadAllLines(filepath);
        Compile(Preprocess(shaderSource));
    }
    public OpenGlShader(string name, string vertexSrc, string fragmentSrc)
    {
        Name = name;
        Compile(new Dictionary<BeeShaderType, string>() {{BeeShaderType.Vertex, vertexSrc}, {BeeShaderType.Fragment, fragmentSrc}});
    }

    private static ShaderType BeeToOpenGL(BeeShaderType type)
    {
        switch (type)
        {
            case BeeShaderType.Vertex:
                return ShaderType.VertexShader;
            case BeeShaderType.Fragment:
                return ShaderType.FragmentShader;
        }
        Log.Error("Invalid shader type!");
        throw new InvalidOperationException();
    }
    private void Compile(Dictionary<BeeShaderType, string> shaders)
    {
        DebugTimer.Start();
        var program = GL.CreateProgram();
        List<int> shaderIds = new List<int>(shaders.Count);
        int success = 0;
        foreach (var shaderpair in shaders)
        {
            if (shaderpair.Value == String.Empty)
            {
                continue;
            }
            var shader = GL.CreateShader(BeeToOpenGL(shaderpair.Key));
            GL.ShaderSource(shader, shaderpair.Value);
            GL.CompileShader(shader);
            
            GL.GetShader(shader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                Log.Error(infoLog);
                GL.DeleteShader(shader);
            }
            shaderIds.Add(shader);
        }

        Log.Assert(shaderIds.Count > 0, "No shaders to compile!");

        _programId = program;

        foreach (var shader in shaderIds)
        {
            GL.AttachShader(_programId, shader);
        }

        GL.LinkProgram(_programId);

        GL.GetProgram(_programId, GetProgramParameterName.LinkStatus, out success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(_programId);
            Log.Error(infoLog);
        }

        foreach (var shaderId in shaderIds)
        {
            GL.DetachShader(_programId, shaderId);
            GL.DeleteShader(shaderId);
        }
        DebugTimer.End();
    }

    private static BeeShaderType[] _shaderTypes = Enum.GetValues<BeeShaderType>();
    private Dictionary<BeeShaderType, string> Preprocess(string[] source)
    {
        DebugTimer.Start();
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
        DebugTimer.End();
        return result;
    }

    public override void Bind()
    {
        DebugTimer.Start("OpenGLShader.Bind()");
        GL.UseProgram(_programId);
        DebugTimer.End("OpenGLShader.Bind()");
    }

    public override void Unbind()
    {
        GL.UseProgram(0);
    }

    public override void UploadUniformMatrix4(string name, ref Matrix4 matrix4)
    {
        DebugTimer.Start();
        global::OpenTK.Mathematics.Matrix4 newMatrix = matrix4;
        var location = GL.GetUniformLocation(_programId, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.UniformMatrix4(location, false, ref newMatrix);
        DebugTimer.End();
    }

    public override void UploadUniformFloat(string name, float value)
    {
        DebugTimer.Start();
        var location = GL.GetUniformLocation(_programId, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform1(location, value);
        DebugTimer.End();
    }

    public override void UploadUniformFloat2(string name, Vector2 vector)
    {
        DebugTimer.Start();
        var location = GL.GetUniformLocation(_programId, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform2(location, vector);
        DebugTimer.End();
    }

    public override void UploadUniformFloat3(string name, Vector3 vector)
    {
        DebugTimer.Start();
        var location = GL.GetUniformLocation(_programId, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform3(location, vector);
        DebugTimer.End();
    }

    public override void UploadUniformFloat4(string name, Vector4 vector)
    {
        DebugTimer.Start();
        var location = GL.GetUniformLocation(_programId, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform4(location, vector);
        DebugTimer.End();
    }

    public override void UploadUniformInt(string name, int value)
    {
        DebugTimer.Start();
        var location = GL.GetUniformLocation(_programId, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform1(location, value);
        DebugTimer.End();
    }

    public override void UploadUniformInt2(string name, Vector2i vector)
    {
        DebugTimer.Start();
        var location = GL.GetUniformLocation(_programId, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform2(location, vector);
        DebugTimer.End();
    }

    public override void UploadUniformInt3(string name, Vector3i vector)
    {
        DebugTimer.Start();
        var location = GL.GetUniformLocation(_programId, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform3(location, vector);
        DebugTimer.End();
    }

    public override void UploadUniformInt4(string name, Vector4i vector)
    {
        DebugTimer.Start();
        var location = GL.GetUniformLocation(_programId, name);
        DebugLog.Assert(location != -1, "Could not find {0}", name);
        GL.Uniform4(location, vector);
        DebugTimer.End();
    }

    private bool _disposedValue = false;

    

    protected override void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            GL.DeleteProgram(_programId);

            _disposedValue = true;
        }
    }
}