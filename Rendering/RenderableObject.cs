using OpenTK.Mathematics;
using VaultCore.Asset;

namespace VaultCore.Rendering;

public class RenderableObject
{
    public RenderableObject(int vao, int vertexCount, Shader shader, Texture texture)
    {
        Vao = vao;
        VertexCount = vertexCount;
        Shader = shader;
        Texture = texture;
    }

    public int Vao { get; }
    public int VertexCount { get; }
    public Shader Shader { get; }
    public Texture Texture { get; }
    public Matrix4 Transform { get; set; } = Matrix4.Identity;
}