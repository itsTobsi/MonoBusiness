using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoBusiness.Input;

public class InputState
{
    KeyboardState _keyboard,
        _prevKeyboard;
    MouseState _mouse,
        _prevMouse;

    public InputState()
    {
        _keyboard = _prevKeyboard = Keyboard.GetState();
        _mouse = _prevMouse = Mouse.GetState();
    }

    public void Update()
    {
        _prevKeyboard = _keyboard;
        _prevMouse = _mouse;
        _keyboard = Keyboard.GetState();
        _mouse = Mouse.GetState();
    }

    public bool IsDown(Keys key) => _keyboard.IsKeyDown(key);

    public bool WasPressed(Keys key) => _keyboard.IsKeyDown(key) && _prevKeyboard.IsKeyUp(key);

    public Vector2 MousePosition => _mouse.Position.ToVector2();
    public Vector2 MouseDelta => (_mouse.Position - _prevMouse.Position).ToVector2();
    public int ScrollDelta => _mouse.ScrollWheelValue - _prevMouse.ScrollWheelValue;

    public bool LeftClicked =>
        _mouse.LeftButton == ButtonState.Pressed && _prevMouse.LeftButton == ButtonState.Released;
    public bool RightClicked =>
        _mouse.RightButton == ButtonState.Pressed && _prevMouse.RightButton == ButtonState.Released;
    public bool MiddleHeld =>
        _mouse.MiddleButton == ButtonState.Pressed
        && _prevMouse.MiddleButton == ButtonState.Pressed;
}
