using OpenTK.Mathematics;

namespace VaultCore.Rendering;

public class Camera
{
    private readonly float far = 100f;
    private readonly float near = 0.1f;
    private Vector3 front;
    private float pitch;
    private Vector3 position;
    private Vector3 right;
    private Matrix4 transform;
    private Vector3 up;
    private float yaw;

    public Camera(float aspectRatio)
    {
        position = new Vector3(0f, 0.5f, -1.5f);
        yaw = 0f;
        pitch = 0f;

        Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90f), aspectRatio, near, far);

        Update();
    }

    public Matrix4 Projection { get; }

    public Matrix4 View { get; private set; }

    private void Update()
    {
        var x = MathF.Cos(pitch) * MathF.Sin(yaw);
        var y = MathF.Sin(pitch);
        var z = MathF.Cos(pitch) * MathF.Cos(yaw);

        front = Vector3.Normalize(new Vector3(x, y, z));
        right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
        up = Vector3.Normalize(Vector3.Cross(right, front));

        View = Matrix4.LookAt(position, position + front, up);
    }

    public void Look(Vector2 delta)
    {
        yaw -= delta.X;
        pitch -= delta.Y;

        yaw = MathHelper.NormalizeRadians(yaw);
        pitch = float.Clamp(pitch, -1.5f, 1.5f);

        Update();
    }

    public void Move(Vector3 move)
    {
        var direction = -move.X * right + move.Y * up + move.Z * front;

        position -= direction * 0.05f;

        Update();
    }
}