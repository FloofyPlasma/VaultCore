using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace VaultCore.Mesh;

public class Cube : IMesh
{
    private readonly Vector3[] normals =
    [
        (0, 0, -1), (0, 0, -1), (0, 0, -1),
        (0, 0, -1), (0, 0, -1), (0, 0, -1),

        (0, 0, 1), (0, 0, 1), (0, 0, 1),
        (0, 0, 1), (0, 0, 1), (0, 0, 1),

        (-1, 0, 0), (-1, 0, 0), (-1, 0, 0),
        (-1, 0, 0), (-1, 0, 0), (-1, 0, 0),

        (1, 0, 0), (1, 0, 0), (1, 0, 0),
        (1, 0, 0), (1, 0, 0), (1, 0, 0),

        (0, -1, 0), (0, -1, 0), (0, -1, 0),
        (0, -1, 0), (0, -1, 0), (0, -1, 0),

        (0, 1, 0), (0, 1, 0), (0, 1, 0),
        (0, 1, 0), (0, 1, 0), (0, 1, 0)
    ];

    private readonly Vector2[] uvCoords =
    [
        (0.0f, 0.0f), (1.0f, 0.0f), (1.0f, 1.0f),
        (1.0f, 1.0f), (0.0f, 1.0f), (0.0f, 0.0f),
        (0.0f, 0.0f), (1.0f, 0.0f), (1.0f, 1.0f),
        (1.0f, 1.0f), (0.0f, 1.0f), (0.0f, 0.0f),
        (1.0f, 0.0f), (1.0f, 1.0f), (0.0f, 1.0f),
        (0.0f, 1.0f), (0.0f, 0.0f), (1.0f, 0.0f),
        (1.0f, 0.0f), (1.0f, 1.0f), (0.0f, 1.0f),
        (0.0f, 1.0f), (0.0f, 0.0f), (1.0f, 0.0f),
        (0.0f, 1.0f), (1.0f, 1.0f), (1.0f, 0.0f),
        (1.0f, 0.0f), (0.0f, 0.0f), (0.0f, 1.0f),
        (0.0f, 1.0f), (1.0f, 1.0f), (1.0f, 0.0f),
        (1.0f, 0.0f), (0.0f, 0.0f), (0.0f, 1.0f)
    ];

    private readonly Vector3[] verticies =
    [
        (-0.5f, -0.5f, -0.5f), (0.5f, -0.5f, -0.5f), (0.5f, 0.5f, -0.5f),
        (0.5f, 0.5f, -0.5f), (-0.5f, 0.5f, -0.5f), (-0.5f, -0.5f, -0.5f),

        (-0.5f, -0.5f, 0.5f), (0.5f, -0.5f, 0.5f), (0.5f, 0.5f, 0.5f),
        (0.5f, 0.5f, 0.5f), (-0.5f, 0.5f, 0.5f), (-0.5f, -0.5f, 0.5f),

        (-0.5f, 0.5f, 0.5f), (-0.5f, 0.5f, -0.5f), (-0.5f, -0.5f, -0.5f),
        (-0.5f, -0.5f, -0.5f), (-0.5f, -0.5f, 0.5f), (-0.5f, 0.5f, 0.5f),

        (0.5f, 0.5f, 0.5f), (0.5f, 0.5f, -0.5f), (0.5f, -0.5f, -0.5f),
        (0.5f, -0.5f, -0.5f), (0.5f, -0.5f, 0.5f), (0.5f, 0.5f, 0.5f),

        (-0.5f, -0.5f, -0.5f), (0.5f, -0.5f, -0.5f), (0.5f, -0.5f, 0.5f),
        (0.5f, -0.5f, 0.5f), (-0.5f, -0.5f, 0.5f), (-0.5f, -0.5f, -0.5f),

        (-0.5f, 0.5f, -0.5f), (0.5f, 0.5f, -0.5f), (0.5f, 0.5f, 0.5f),
        (0.5f, 0.5f, 0.5f), (-0.5f, 0.5f, 0.5f), (-0.5f, 0.5f, -0.5f)
    ];

    public int Vao { get; private set; }

    public int VertexCount => verticies.Length;

    public void BindVao(int shaderId)
    {
        Vao = GL.GenVertexArray();
        GL.BindVertexArray(Vao);

        var data = BuildVertexArray();

        var vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsage.StaticDraw);

        var position = (uint)GL.GetAttribLocation(shaderId, "position");
        GL.EnableVertexAttribArray(position);
        GL.VertexAttribPointer(position, 3, VertexAttribPointerType.Float, false, sizeof(float) * 8, 0);

        var normal = (uint)GL.GetAttribLocation(shaderId, "normal");
        GL.EnableVertexAttribArray(normal);
        GL.VertexAttribPointer(normal, 3, VertexAttribPointerType.Float, false, sizeof(float) * 8, sizeof(float) * 3);

        var uv = (uint)GL.GetAttribLocation(shaderId, "uv");
        GL.EnableVertexAttribArray(uv);
        GL.VertexAttribPointer(uv, 2, VertexAttribPointerType.Float, false, sizeof(float) * 8, sizeof(float) * 6);
    }

    private float[] BuildVertexArray()
    {
        var data = new float[verticies.Length * 8];

        var i = 0;
        for (var d = 0; d < data.Length; d += 8)
        {
            // Position
            data[d + 0] = verticies[i].X;
            data[d + 1] = verticies[i].Y;
            data[d + 2] = verticies[i].Z;

            // Normal
            data[d + 3] = normals[i].X;
            data[d + 4] = normals[i].Y;
            data[d + 5] = normals[i].Z;

            // UV
            data[d + 6] = uvCoords[i].X;
            data[d + 7] = uvCoords[i].Y;
            i++;
        }

        return data;
    }
}