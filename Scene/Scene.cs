using VaultCore.Input;
using VaultCore.Rendering;

namespace VaultCore.Scene;

public interface IScene
{
    Camera Camera { get; }
    void Update(float deltaTime, InputManager input);
    void Render(Renderer renderer, Camera camera);
}