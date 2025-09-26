using OpenTK.Core.Utility;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGLES2;
using OpenTK.Mathematics;
using OpenTK.Platform;

namespace VaultCore.Window;

public sealed class WindowManager : IDisposable
{
    public WindowManager(string title, int width, int height)
    {
        var logger = new ConsoleLogger
        {
            Filter = LogLevel.Info
        };

        ToolkitOptions options = new()
        {
            Logger = logger,
            ApplicationName = title
        };
        Toolkit.Init(options);

        OpenGLGraphicsApiHints hints = new();
        Window = Toolkit.Window.Create(hints);
        Context = Toolkit.OpenGL.CreateFromWindow(Window);

        Toolkit.Window.SetTitle(Window, title);

        Toolkit.OpenGL.SetCurrentContext(Context);
        GLLoader.LoadBindings(Toolkit.OpenGL.GetBindingsContext(Context));

        Toolkit.Window.SetMode(Window, WindowMode.Normal);
        Toolkit.Window.SetClientSize(Window, (width, height));

        GL.Enable(EnableCap.DepthTest);

        GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);

        GL.Viewport(0, 0, width, height);
    }

    public WindowHandle Window { get; }

    public OpenGLContextHandle Context { get; }

    public void Dispose()
    {
        Toolkit.OpenGL.DestroyContext(Context);
        Toolkit.Window.Destroy(Window);
    }

    public void ProcessEvents()
    {
        Toolkit.Window.ProcessEvents(false);
    }

    public bool IsDestroyed()
    {
        return Toolkit.Window.IsWindowDestroyed(Window);
    }

    public void SwapBuffers()
    {
        Toolkit.OpenGL.SwapBuffers(Context);
    }

    public void GetClientSize(out Vector2i size)
    {
        Toolkit.Window.GetClientSize(Window, out size);
    }
}