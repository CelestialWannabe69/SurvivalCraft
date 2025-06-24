using Microsoft.Xna.Framework;

namespace SurvivalCraft.Systems
{
    public enum Season { Spring, Summer, Fall, Winter }

    public class SeasonManager
    {
        private Season _current = Season.Spring;
        private float _timer;
        private readonly float _length;

        public Season Current => _current;

        public SeasonManager(float seasonLengthSeconds = 300f)
        {
            _length = seasonLengthSeconds;
        }

        public void Update(GameTime gt)
        {
            _timer += (float)gt.ElapsedGameTime.TotalSeconds;
            if (_timer >= _length)
            {
                _timer -= _length;
                _current = (Season)(((int)_current + 1) % 4);
            }
        }

        public void GetSeasonColors(out Color night, out Color morning, out Color day, out Color evening)
        {
            switch (_current)
            {
                case Season.Spring:
                    night = new Color(60, 60, 100);       // night = soft blue
                    morning = new Color(180, 220, 180);   // fresh green morning
                    day = new Color(200, 250, 200);       // bright green day
                    evening = new Color(150, 120, 180);   // purple-pink evening
                    break;

                case Season.Summer:
                    night = new Color(40, 40, 80);        // deep blue night
                    morning = new Color(255, 220, 150);   // orange-yellow morning
                    day = new Color(255, 255, 200);       // bright sunny day
                    evening = new Color(255, 180, 120);   // orange sunset
                    break;

                case Season.Fall:
                    night = new Color(70, 50, 40);        // brown night
                    morning = new Color(220, 180, 140);   // soft orange morning
                    day = new Color(240, 200, 160);       // golden day
                    evening = new Color(200, 120, 80);    // strong orange evening
                    break;

                default: // Winter
                    night = new Color(100, 100, 130);     // cold blue night
                    morning = new Color(220, 220, 250);   // pale blue morning
                    day = new Color(255, 255, 255);       // white snowy day
                    evening = new Color(180, 180, 220);   // soft blue evening
                    break;
            }
        }
    }
}
