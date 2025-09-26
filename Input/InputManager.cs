using OpenTK.Mathematics;
using OpenTK.Platform;

namespace VaultCore.Input;

public sealed class InputManager
{
    private readonly CursorHandle _defaultCursor;
    private readonly WindowHandle _window;
    private Vector2 _lastMouse;

    private Vector3 _move;

    public InputManager(WindowHandle window)
    {
        _window = window;
        _defaultCursor = Toolkit.Cursor.Create(SystemCursorType.Default);
        EventQueue.EventRaised += HandleEvents;
        Move = new Vector3();
        _lastMouse = Vector2.Zero;
    }

    public ref Vector3 Move => ref _move;

    public Vector2 MouseDelta { get; private set; }
    public bool Grabbed { get; private set; }
    public bool RequestClose { get; private set; }

    private void HandleEvents(PalHandle? handle, PlatformEventType type, EventArgs args)
    {
        switch (args)
        {
            case CloseEventArgs closeEvent:
            {
                RequestClose = true;
            }
                break;
            case MouseMoveEventArgs mouseMove:
            {
                var diff = mouseMove.ClientPosition - _lastMouse;
                MouseDelta += Grabbed ? diff / 1000f : Vector2.Zero;
                _lastMouse = mouseMove.ClientPosition;
            }
                break;
            case KeyDownEventArgs keyDown:
                if (keyDown.IsRepeat) break;
                switch (keyDown.Scancode)
                {
                    case Scancode.LeftAlt:
                        Toolkit.Window.SetCursorCaptureMode(_window, CursorCaptureMode.Locked);
                        Toolkit.Window.SetCursor(_window, null);
                        Grabbed = true;
                        break;
                    case Scancode.W: _move.Z -= 1f; break;
                    case Scancode.S: _move.Z += 1f; break;
                    case Scancode.A: _move.X -= 1f; break;
                    case Scancode.D: _move.X += 1f; break;
                }

                break;

            case KeyUpEventArgs keyUp:
                switch (keyUp.Scancode)
                {
                    case Scancode.LeftAlt:
                        Toolkit.Window.SetCursorCaptureMode(_window, CursorCaptureMode.Normal);
                        Toolkit.Window.SetCursor(_window, _defaultCursor);
                        Grabbed = false;
                        break;
                    case Scancode.W: _move.Z += 1f; break;
                    case Scancode.S: _move.Z -= 1f; break;
                    case Scancode.A: _move.X += 1f; break;
                    case Scancode.D: _move.X -= 1f; break;
                }

                break;
        }
    }

    public void ResetMouseDelta()
    {
        MouseDelta = Vector2.Zero;
    }
}