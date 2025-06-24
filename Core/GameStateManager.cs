using Microsoft.Xna.Framework;

namespace SurvivalCraft.Core
{
    public class GameStateManager
    {
        public GameState CurrentState { get; private set; } = GameState.MainMenu;

        public void ChangeState(GameState newState) => CurrentState = newState;

        public void TogglePause()
        {
            if (CurrentState == GameState.Playing)
                CurrentState = GameState.Paused;
            else if (CurrentState == GameState.Paused)
                CurrentState = GameState.Playing;
        }
    }
}