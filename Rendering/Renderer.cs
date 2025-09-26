using OpenTK.Graphics.OpenGLES2;

namespace VaultCore.Rendering;

public class Renderer
{
    public void Draw(RenderableObject obj, Camera camera)
    {
        // if (changeActiveShader) obj.Shader.Use();
        GL.BindVertexArray(obj.Vao);

        var uTranslation = GL.GetUniformLocation(obj.Shader.Id, "translation");
        var uProjection = GL.GetUniformLocation(obj.Shader.Id, "projection");
        var uView = GL.GetUniformLocation(obj.Shader.Id, "view");

        var projection = camera.Projection;
        GL.UniformMatrix4f(uProjection, 1, false, ref projection);
        var view = camera.View;
        GL.UniformMatrix4f(uView, 1, false, ref view);
        var transform = obj.Transform;
        GL.UniformMatrix4f(uTranslation, 1, false, ref transform);

        //obj.Texture.Bind();

        GL.DrawArrays(PrimitiveType.Triangles, 0, obj.VertexCount);
    }

    public void Clear()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }
}