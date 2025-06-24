using System;
using Microsoft.Xna.Framework;

namespace SurvivalCraft.Systems
{
    public enum SpecialEventType
    {
        None,
        MeteorShower,
        SolarEclipse,
        BloodMoon
    }
    public class SpecialEventManager
    {
        private SpecialEventType _currentEvent = SpecialEventType.None;
        private float _timer;
        private readonly Random _random = new Random();

        public SpecialEventType CurrentEvent { get; private set; } = SpecialEventType.None;

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_currentEvent == SpecialEventType.None)
            {
                if (_timer > 70f) // Every 3 minutes try to trigger event
                {
                    _timer = 0f;
                    int chance = _random.Next(100);

                    if (chance < 50) _currentEvent = SpecialEventType.MeteorShower;
                    else if (chance < 60) _currentEvent = SpecialEventType.SolarEclipse;
                    else if (chance < 70) _currentEvent = SpecialEventType.BloodMoon;
                }
            }
            else
            {
                // If event is active, end after a duration
                if (_timer > 60f) // Event lasts for 1 minute
                {
                    _currentEvent = SpecialEventType.None;
                    _timer = 0f;
                }
            }
        }
    }
}