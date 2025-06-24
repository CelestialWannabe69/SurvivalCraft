using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SurvivalCraft.Entities;

namespace SurvivalCraft
{
    public class Hud
    {
        private readonly SpriteFont _font;
        private readonly Texture2D _pixel;

        public Hud(GraphicsDevice graphicsDevice, SpriteFont font)
        {
            _font = font;
            // 1×1 white pixel for drawing bars
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public void Draw(SpriteBatch sb, PlayerStats stats)
        {
            // Background bar dimensions
            const int barWidth = 200, barHeight = 16;
            var pos = new Vector2(10, 10);

            // Health (red)
            sb.Draw(_pixel, new Rectangle((int)pos.X, (int)pos.Y, barWidth, barHeight), Color.DarkRed);
            sb.Draw(_pixel, new Rectangle((int)pos.X, (int)pos.Y, (int)(barWidth * stats.Health / 100f), barHeight), Color.Red);
            sb.DrawString(_font, "HP", pos + new Vector2(barWidth + 5, 0), Color.White);

            // Hunger (orange)
            pos.Y += barHeight + 5;
            sb.Draw(_pixel, new Rectangle((int)pos.X, (int)pos.Y, barWidth, barHeight), Color.DarkOrange);
            sb.Draw(_pixel, new Rectangle((int)pos.X, (int)pos.Y, (int)(barWidth * stats.Hunger / 100f), barHeight), Color.Orange);
            sb.DrawString(_font, "Hunger", pos + new Vector2(barWidth + 5, 0), Color.White);

            // Stamina (blue)
            pos.Y += barHeight + 5;
            sb.Draw(_pixel, new Rectangle((int)pos.X, (int)pos.Y, barWidth, barHeight), Color.DarkBlue);
            sb.Draw(_pixel, new Rectangle((int)pos.X, (int)pos.Y, (int)(barWidth * stats.Stamina / 100f), barHeight), Color.Blue);
            sb.DrawString(_font, "Stamina", pos + new Vector2(barWidth + 5, 0), Color.White);
        }
    }
}
