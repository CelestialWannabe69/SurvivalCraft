using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalCraft
{
    public enum TileType { Grass, Dirt }

    public class Tile
    {
        public TileType Type { get; }
        public Texture2D Texture { get; }
        public Rectangle Bounds { get; }

        public Tile(TileType type, Texture2D texture, Rectangle bounds)
        {
            Type = type;
            Texture = texture;
            Bounds = bounds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Bounds, Color.White);
        }
    }
}