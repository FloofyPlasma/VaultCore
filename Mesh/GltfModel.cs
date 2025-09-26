using System.Buffers.Binary;
using glTFLoader;
using OpenTK.Graphics.OpenGL;

namespace VaultCore.Mesh;

internal class GltfModel : IMesh
{
    private float[] data;
    private ushort[] indicies;

    public int Vao { get; private set; }

    public int VertexCount { get; private set; }

    public void Load(string file)
    {
        var model = Interface.LoadModel(file);
        var buffer = Interface.LoadBinaryBuffer(file).AsSpan();

        var mesh = model.Meshes[0];
        var primitive = mesh.Primitives[0];

        var position = model.Accessors[primitive.Attributes["POSITION"]];
        var texture = model.Accessors[primitive.Attributes["TEXCOORD_0"]];

        data = new float[position.Count * 5];

        var positionView = model.BufferViews[position.BufferView!.Value];
        var textureView = model.BufferViews[texture.BufferView!.Value];

        float ReadSingle(ref Span<byte> buffer, int index)
        {
            return BinaryPrimitives.ReadSingleLittleEndian(buffer.Slice(index, 4));
        }

        var p = positionView.ByteOffset;
        var t = textureView.ByteOffset;
        for (var i = 0; i < position.Count * 5; i += 5)
        {
            data[i + 0] = ReadSingle(ref buffer, p + 0);
            data[i + 1] = ReadSingle(ref buffer, p + 4);
            data[i + 2] = ReadSingle(ref buffer, p + 8);
            p += 12;

            data[i + 3] = ReadSingle(ref buffer, t + 0);
            data[i + 4] = ReadSingle(ref buffer, t + 4);
            t += 8;
        }

        var indiciesView = model.BufferViews[primitive.Indices!.Value];
        indicies = new ushort[indiciesView.ByteLength / sizeof(ushort)];
        for (var i = 0; i < indicies.Length; i++)
            indicies[i] =
                BinaryPrimitives.ReadUInt16LittleEndian(buffer.Slice(indiciesView.ByteOffset + i * sizeof(ushort)));

        VertexCount = indicies.Length;
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

        var ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indicies.Length * sizeof(ushort), indicies,
            BufferUsage.StaticDraw);
    }
}