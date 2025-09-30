using OpenTK.Mathematics;
using VaultCore.Asset;

namespace VaultCore.Rendering;

public class Material
{
    public Material(Vector3 objectColor, float shininess = 32f, Texture? texture = null)
    {
        ObjectColor = objectColor;
        Shininess = shininess;
        Texture = texture;
    }

    public Vector3 ObjectColor { get; set; }
    public float Shininess { get; set; }
    public Texture? Texture { get; set; }
}