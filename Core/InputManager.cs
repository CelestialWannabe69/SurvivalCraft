using Microsoft.Xna.Framework.Input;

namespace SurvivalCraft.Core
{
    public static class InputManager
    {
        private static KeyboardState _currentState;
        private static KeyboardState _previousState;

        public static void Update()
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();
        }

        public static bool IsKeyPressed(Keys key)
            => _currentState.IsKeyDown(key) && !_previousState.IsKeyDown(key);

        public static bool IsKeyDown(Keys key)
            => _currentState.IsKeyDown(key);
    }
}