using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalCraft.Systems
{
    public class DayNightCycle
    {
        private float _timeOfDay;
        private readonly float _dayLength;
        private Texture2D _overlay;

        public DayNightCycle(GraphicsDevice graphics, float dayLengthSeconds = 120f)
        {
            _dayLength = dayLengthSeconds;
            _overlay = new Texture2D(graphics, 1, 1);
            _overlay.SetData(new[] { Color.White });
        }

        public void Update(GameTime gt)
        {
            _timeOfDay = (_timeOfDay + (float)gt.ElapsedGameTime.TotalSeconds) % _dayLength;
        }

        public Color GetTint(Color nightColor, Color morningColor, Color dayColor, Color eveningColor)
        {
            float t = _timeOfDay / _dayLength;
            Color result;
            if (t < 0.25f)
                result = Color.Lerp(nightColor, morningColor, t / 0.25f);
            else if (t < 0.5f)
                result = Color.Lerp(morningColor, dayColor, (t - 0.25f) / 0.25f);
            else if (t < 0.75f)
                result = Color.Lerp(dayColor, eveningColor, (t - 0.5f) / 0.25f);
            else
                result = Color.Lerp(eveningColor, nightColor, (t - 0.75f) / 0.25f);

            result *= 0.5f; // half transparent
            return result;
        }

        public void DrawOverlay(SpriteBatch sb, int screenW, int screenH, Color tint)
        {
            sb.Begin();
            sb.Draw(_overlay, new Rectangle(0, 0, screenW, screenH), tint);
            sb.End();
        }
    }
}
