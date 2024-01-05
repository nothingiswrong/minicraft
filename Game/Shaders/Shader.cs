using OpenTK.Graphics.ES20;
namespace Game.Shaders;

public class Shader : IDisposable
{
    private readonly int _handle;
    private bool _disposedValue = false;

    public Shader(string vertexPath, string fragmentPath)
    {
        var vertexShaderSource = File.ReadAllText(vertexPath);
        var fragmentShaderSource = File.ReadAllText(fragmentPath);

        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

        string infoLog;
        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        GL.CompileShader(vertexShader);
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            infoLog = GL.GetShaderInfoLog(vertexShader);
            Console.WriteLine(infoLog);
        }

        GL.CompileShader(fragmentShader);
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            infoLog = GL.GetShaderInfoLog(fragmentShader);
            Console.WriteLine(infoLog);
        }

        _handle = GL.CreateProgram();
        GL.AttachShader(_handle, vertexShader);
        GL.AttachShader(_handle, fragmentShader);
        GL.LinkProgram(_handle);

        GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out success);
        if (success == 0)
        {
            infoLog = GL.GetProgramInfoLog(_handle);
            Console.WriteLine(infoLog);
        }
        
        //Cleanup
        GL.DetachShader(_handle, vertexShader); 
        GL.DetachShader(_handle, fragmentShader); 
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
        
    }

    public void Use()
    {
        GL.UseProgram(_handle);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            GL.DeleteProgram(_handle);
            _disposedValue = true;
        }
    }
    
    public void Dispose()
    {
       Dispose(true); 
       GC.SuppressFinalize(this);
    }

    ~Shader()
    {
       if (!_disposedValue) Console.WriteLine("GPU LEAK");
    }
}