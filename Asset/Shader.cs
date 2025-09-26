using OpenTK.Graphics.OpenGL;

namespace VaultCore.Asset;

public class Shader
{
    public int Id { get; private set; }

    public void Setup(string vertexShader, string fragmentShader)
    {
        var vertexHandle = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexHandle, vertexShader);
        CompileShader(vertexHandle);

        var fragmentHandle = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentHandle, fragmentShader);
        CompileShader(fragmentHandle);

        var shaderHandle = GL.CreateProgram();
        GL.AttachShader(shaderHandle, vertexHandle);
        GL.AttachShader(shaderHandle, fragmentHandle);
        GL.LinkProgram(shaderHandle);

        GL.DetachShader(shaderHandle, vertexHandle);
        GL.DetachShader(shaderHandle, fragmentHandle);

        GL.DeleteShader(vertexHandle);
        GL.DeleteShader(fragmentHandle);

        GL.UseProgram(shaderHandle);

        Id = shaderHandle;
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