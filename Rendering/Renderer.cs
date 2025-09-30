using OpenTK.Graphics.OpenGLES2;

namespace VaultCore.Rendering;

public class Renderer
{
    public void Draw(RenderableObject obj, Camera camera)
    {
        obj.Shader.Use();
        GL.BindVertexArray(obj.Vao);

        obj.Shader.SetMatrix4f("projection", camera.Projection);
        obj.Shader.SetMatrix4f("view", camera.View);
        obj.Shader.SetMatrix4f("translation", obj.Transform);

        obj.Texture.Bind();

        GL.DrawArrays(PrimitiveType.Triangles, 0, obj.VertexCount);
    }

    public void Draw(RenderableObject obj, Camera camera, Material material)
    {
        obj.Shader.Use();
        GL.BindVertexArray(obj.Vao);

        material.Texture?.Bind();

        obj.Shader.SetVector3("objectColor", material.ObjectColor);
        obj.Shader.SetFloat("shininess", material.Shininess);
        obj.Shader.SetVector3("cameraPos", camera.Position);
        obj.Shader.SetMatrix4f("projection", camera.Projection);
        obj.Shader.SetMatrix4f("view", camera.View);
        obj.Shader.SetMatrix4f("translation", obj.Transform);

        GL.DrawArrays(PrimitiveType.Triangles, 0, obj.VertexCount);
    }

    public void Clear()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }
}