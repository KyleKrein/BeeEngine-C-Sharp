using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.OpenTK;

public class Shader: IDisposable
{
    int Handle;

    public Shader(string vertexPath, string fragmentPath)
    {
        ImportShadersAndCompile(vertexPath, fragmentPath, out int VertexShader, out int FragmentShader);

        CheckForCompilationErrors(VertexShader, FragmentShader);
        
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
            Console.WriteLine(infoLog);
        }
    }

    private void CheckForCompilationErrors(int VertexShader, int FragmentShader)
    {
        GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(VertexShader);
            Console.WriteLine(infoLog);
        }

        GL.CompileShader(FragmentShader);

        GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(FragmentShader);
            Console.WriteLine(infoLog);
        }
    }

    private void ImportShadersAndCompile(string vertexPath, string fragmentPath, out int VertexShader, out int FragmentShader)
    {
        string VertexShaderSource = File.ReadAllText(vertexPath);

        string FragmentShaderSource = File.ReadAllText(fragmentPath);
        VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, VertexShaderSource);

        FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, FragmentShaderSource);

        GL.CompileShader(VertexShader);
    }
    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(Handle);

            disposedValue = true;
        }
    }

    ~Shader()
    {
        GL.DeleteProgram(Handle);
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}