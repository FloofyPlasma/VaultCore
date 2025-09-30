using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace VaultCore.Asset;

public class Shader : IDisposable
{
    public int Id { get; private set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private (int, int) SetupBasicShaders()
    {
        var vertexShader = File.ReadAllText("../VaultCore/EngineAssets/Shader/BasicShader.vert");
        var fragmentShader = File.ReadAllText("../VaultCore/EngineAssets/Shader/BasicShader.frag");

        var vertexHandle = GL.CreateShader(ShaderType.VertexShader);
        Console.WriteLine("Vertex shader: {0}", vertexShader);
        GL.ShaderSource(vertexHandle, vertexShader);
        CompileShader(vertexHandle);

        var fragmentHandle = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentHandle, fragmentShader);
        CompileShader(fragmentHandle);

        return (vertexHandle, fragmentHandle);
    }

    public void Setup(string vertexShader, string fragmentShader)
    {
        var (basicFragment, basicVertex) = SetupBasicShaders();

        var vertexHandle = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexHandle, vertexShader);
        CompileShader(vertexHandle);

        var fragmentHandle = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentHandle, fragmentShader);
        CompileShader(fragmentHandle);


        var shaderHandle = GL.CreateProgram();
        GL.AttachShader(shaderHandle, basicVertex);
        GL.AttachShader(shaderHandle, basicFragment);
        GL.AttachShader(shaderHandle, vertexHandle);
        GL.AttachShader(shaderHandle, fragmentHandle);
        GL.LinkProgram(shaderHandle);

        GL.DetachShader(shaderHandle, basicVertex);
        GL.DetachShader(shaderHandle, basicFragment);
        GL.DetachShader(shaderHandle, vertexHandle);
        GL.DetachShader(shaderHandle, fragmentHandle);

        GL.DeleteShader(basicVertex);
        GL.DeleteShader(basicFragment);
        GL.DeleteShader(vertexHandle);
        GL.DeleteShader(fragmentHandle);

        GL.UseProgram(shaderHandle);

        Id = shaderHandle;
    }

    public void SetVector3(string name, Vector3 newValue)
    {
        var uLocation = GL.GetUniformLocation(Id, name);

        if (uLocation == -1)
        {
            Console.WriteLine("[Error] Shader does not have uniform: {0}", name);
            return;
        }

        var value = newValue;

        GL.Uniform3f(uLocation, 1, in value);
    }

    public void SetFloat(string name, float newValue)
    {
        var uLocation = GL.GetUniformLocation(Id, name);

        if (uLocation == -1)
        {
            Console.WriteLine("[Error] Shader does not have uniform: {0}", name);
            return;
        }

        GL.Uniform1f(uLocation, newValue);
    }

    public void SetMatrix4f(string name, Matrix4 newValue)
    {
        var uLocation = GL.GetUniformLocation(Id, name);

        if (uLocation == -1)
        {
            Console.WriteLine("[Error] Shader does not have uniform: {0}", name);
            return;
        }

        var value = newValue;

        GL.UniformMatrix4f(uLocation, 1, false, in value);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        if (GL.IsProgram(Id))
            GL.DeleteProgram(Id);
    }

    private void CompileShader(int shader)
    {
        GL.CompileShader(shader);

        GL.GetShaderi(shader, ShaderParameterName.CompileStatus, out var code);
        if (code != (int)All.True)
        {
            GL.GetShaderInfoLog(shader, out var infoLog);
            throw new Exception($"Error compiling shader.{Environment.NewLine}{infoLog}");
        }
    }

    public void Use()
    {
        GL.UseProgram(Id);
    }
}