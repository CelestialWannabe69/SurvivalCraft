using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalCraft.Entities
{
    public abstract class Entity
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public float Speed { get; protected set; }

        protected Entity(Texture2D texture, Vector2 startPosition, float speed)
        {
            Texture = texture;
            Position = startPosition;
            Speed = speed;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}