using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SurvivalCraft.Core;

namespace SurvivalCraft.Entities
{
    public class Player : Entity
    {
        public PlayerStats Stats { get; }
        public Inventory PlayerInventory { get; }  // New inventory property
        public Player(Texture2D texture, Vector2 startPosition, float speed)
            : base(texture, startPosition, speed)
        {
            Stats = new PlayerStats();
            PlayerInventory = new Inventory();
        }
        public override void Update(GameTime gameTime)
        {
            var move = Vector2.Zero;
            if (InputManager.IsKeyDown(Keys.W)) move.Y -= 1;
            if (InputManager.IsKeyDown(Keys.S)) move.Y += 1;
            if (InputManager.IsKeyDown(Keys.A)) move.X -= 1;
            if (InputManager.IsKeyDown(Keys.D)) move.X += 1;

            bool isMoving = move != Vector2.Zero;
            if (isMoving)
            {
                move.Normalize();
                Position += move * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            Stats.Update(gameTime, isMoving);
        }
    }
}