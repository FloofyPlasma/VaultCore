using VaultCore.Input;
using VaultCore.Rendering;
using VaultCore.Scene;
using VaultCore.Window;

namespace VaultCore;

public sealed class Engine
{
    private readonly InputManager _input;
    private readonly Renderer _renderer;
    private readonly WindowManager _window;
    private IScene? _activeScene;

    public Engine(string title, int width, int height)
    {
        _window = new WindowManager(title, width, height);
        _input = new InputManager(_window.Window);
        _renderer = new Renderer();
    }

    public void SetActiveScene(IScene scene)
    {
        _activeScene = scene;
    }

    public void Run()
    {
        var lastTime = DateTime.Now;

        while (!_window.IsDestroyed() && !_input.RequestClose)
        {
            _window.ProcessEvents();

            var currentTime = DateTime.Now;
            var deltaTime = (float)(currentTime - lastTime).TotalSeconds;
            lastTime = currentTime;


            _renderer.Clear();
            _activeScene?.Update(deltaTime, _input);
            _activeScene?.Render(_renderer, _activeScene.Camera);
            _window.SwapBuffers();
        }

        _window.Dispose();
    }
}