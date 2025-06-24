using System;
using Microsoft.Xna.Framework;

namespace SurvivalCraft.Systems
{
    public enum Weather { Clear, Rain, Snow, Thunder }

    public class WeatherManager
    {
        private readonly SeasonManager _seasons;
        private Weather _current = Weather.Clear;
        private float _timer;

        public Weather Current => _current;

        public WeatherManager(SeasonManager seasonMgr)
        {
            _seasons = seasonMgr;
        }

        public void Update(GameTime gt)
        {
            _timer += (float)gt.ElapsedGameTime.TotalSeconds;
            if (_timer >= 60f)
            {
                _timer = 0f;
                var rnd = new Random();
                int roll = rnd.Next(100);

                switch (_seasons.Current)
                {
                    case Season.Spring:
                        _current = roll < 30 ? Weather.Rain : Weather.Clear;
                        break;
                    case Season.Summer:
                        _current = roll < 10 ? Weather.Thunder : Weather.Clear;
                        break;
                    case Season.Fall:
                        _current = roll < 20 ? Weather.Rain : Weather.Clear;
                        break;
                    case Season.Winter:
                        _current = roll < 40 ? Weather.Snow : Weather.Clear;
                        break;
                }
            }
        }
    }
}
