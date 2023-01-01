using BeeEngine.OpenTK.Renderer;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.OpenTK.Platform.OpenGL;

public class OpenGlShader: Shader
{
    int Handle;

    public OpenGlShader(string vertexSrc, string fragmentSrc)
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
    
    public void Use()
    {
        GL.UseProgram(Handle);
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