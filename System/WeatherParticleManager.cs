using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SurvivalCraft.Systems
{
    public class WeatherParticleManager
    {
        private class Particle
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public Color Color;
            public float Size;
            public bool IsRain;
            public float LifeTime;
        }

        private List<Particle> _particles = new List<Particle>();
        private Random _random = new Random();
        private Texture2D _pixel;
        private GraphicsDevice _graphics;
        private Weather _currentWeather = Weather.Clear;

        // ---- NEW for Lightning ----
        private float _lightningTimer = 0f;
        private float _lightningDuration = 0f;
        private bool _isFlashing = false;
        // ----------------------------

        public WeatherParticleManager(GraphicsDevice graphicsDevice)
        {
            _graphics = graphicsDevice;
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public void Update(GameTime gameTime, Weather currentWeather)
        {
            _currentWeather = currentWeather;

            SpawnParticles();
            MoveParticles(gameTime);
            RemoveOffScreenParticles();

            UpdateLightning(gameTime); // <-- NEW
        }

        private void SpawnParticles()
        {
            if (_currentWeather == Weather.Rain || _currentWeather == Weather.Thunder)
            {
                for (int i = 0; i < 5; i++)
                {
                    _particles.Add(new Particle
                    {
                        Position = new Vector2(_random.Next(0, _graphics.Viewport.Width), -10),
                        Velocity = new Vector2(0, 400 + _random.Next(100)),
                        Color = Color.LightBlue,
                        Size = 2f,
                        IsRain = true,
                        LifeTime = (_graphics.Viewport.Height + 20) / (400f + _random.Next(100))
                    });
                }
            }
            else if (_currentWeather == Weather.Snow)
            {
                for (int i = 0; i < 3; i++)
                {
                    _particles.Add(new Particle
                    {
                        Position = new Vector2(_random.Next(0, _graphics.Viewport.Width), -10),
                        Velocity = new Vector2(_random.Next(-20, 20), 50 + _random.Next(30)),
                        Color = Color.White,
                        Size = 4f,
                        IsRain = false,
                        LifeTime = (_graphics.Viewport.Height + 20) / (50f + _random.Next(30))
                    });
                }
            }
        }

        private void MoveParticles(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                var p = _particles[i];
                p.Position += p.Velocity * dt;
                p.LifeTime -= dt;

                if (p.LifeTime <= 0)
                {
                    _particles.RemoveAt(i);
                }
            }
        }

        private void RemoveOffScreenParticles()
        {
            _particles.RemoveAll(p => p.Position.Y > _graphics.Viewport.Height + 20);
        }

        /// <summary>
        /// Manage random lightning flashes during thunderstorm.
        /// </summary>
        private void UpdateLightning(GameTime gameTime)
        {
            if (_currentWeather != Weather.Thunder)
            {
                _isFlashing = false;
                _lightningTimer = 0f;
                return;
            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_isFlashing)
            {
                _lightningDuration -= dt;
                if (_lightningDuration <= 0)
                {
                    _isFlashing = false;
                    _lightningTimer = _random.Next(2, 5); // 2 to 5 sec before next flash
                }
            }
            else
            {
                _lightningTimer -= dt;
                if (_lightningTimer <= 0)
                {
                    _isFlashing = true;
                    _lightningDuration = 0.1f + (float)_random.NextDouble() * 0.2f; // 0.1 - 0.3 sec flash
                }
            }
        }

        /// <summary>
        /// Draw all active particles. Call between world drawing and HUD.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var p in _particles)
            {
                if (p.IsRain)
                {
                    var rect = new Rectangle((int)p.Position.X, (int)p.Position.Y, 1, (int)(p.Size * 5));
                    spriteBatch.Draw(_pixel, rect, p.Color);
                }
                else
                {
                    var rect = new Rectangle((int)p.Position.X, (int)p.Position.Y, (int)p.Size, (int)p.Size);
                    spriteBatch.Draw(_pixel, rect, p.Color);
                }
            }

            spriteBatch.End();

            // --- NEW: Draw Lightning Flash ---
            if (_isFlashing)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(_pixel, new Rectangle(0, 0, _graphics.Viewport.Width, _graphics.Viewport.Height), Color.White * 0.6f); // White flash
                spriteBatch.End();
            }
            // ---------------------------------
        }
    }
}
