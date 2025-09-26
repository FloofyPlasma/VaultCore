using JeremyAnsel.Media.WavefrontObj;
using OpenTK.Graphics.OpenGL;

namespace VaultCore.Mesh;

public class Wavefront : IMesh
{
    private float[] data;

    public int Vao { get; private set; }

    public int VertexCount { get; private set; }

    public void Load(string file)
    {
        var obj = ObjFile.FromFile(file);

        VertexCount = obj.Faces.Count * 3;

        data = new float[VertexCount * 5];
        var i = 0;
        for (var v = 0; v < obj.Faces.Count; v++)
        for (var f = 0; f < obj.Faces[v].Vertices.Count; f++)
        {
            var triplet = obj.Faces[v].Vertices[f];
            data[i++] = obj.Vertices[triplet.Vertex - 1].Position.X;
            data[i++] = obj.Vertices[triplet.Vertex - 1].Position.Y;
            data[i++] = obj.Vertices[triplet.Vertex - 1].Position.Z;

            data[i++] = obj.TextureVertices[triplet.Texture - 1].X;
            data[i++] = obj.TextureVertices[triplet.Texture - 1].Y;
        }
    }

    public void BindVao(int shaderId)
    {
        Vao = GL.GenVertexArray();
        GL.BindVertexArray(Vao);

        var vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsage.StaticDraw);

        var position = (uint)GL.GetAttribLocation(shaderId, "position");
        GL.EnableVertexAttribArray(position);
        GL.VertexAttribPointer(position, 3, VertexAttribPointerType.Float, false, sizeof(float) * 5, 0);

        var uv = (uint)GL.GetAttribLocation(shaderId, "uv");
        GL.EnableVertexAttribArray(uv);
        GL.VertexAttribPointer(uv, 2, VertexAttribPointerType.Float, false, sizeof(float) * 5, sizeof(float) * 3);
    }
}