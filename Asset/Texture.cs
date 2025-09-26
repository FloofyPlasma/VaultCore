using OpenTK.Graphics.OpenGL;
using StbImageSharp;

namespace VaultCore.Asset;

public class Texture
{
    private int handle;

    public (int width, int height) Buffer(Stream stream)
    {
        handle = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2d, handle);

        StbImage.stbi_set_flip_vertically_on_load(1);
        var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

        GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba,
            PixelType.UnsignedByte, image.Data);

        GL.GenerateMipmap(TextureTarget.Texture2d);

        return (image.Width, image.Height);
    }

    public void Bind()
    {
        GL.BindTexture(TextureTarget.Texture2d, handle);
    }
}