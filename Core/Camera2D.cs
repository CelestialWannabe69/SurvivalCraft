using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalCraft.Core
{
    public class Camera2D
    { 
        private readonly Viewport _viewport;
        private Vector2 _position;
        private float _zoom = 1f;
        private float _rotation = 0f;
        private readonly int _worldWidth;
        private readonly int _worldHeight;

        public Camera2D(Viewport viewport, int worldWidth, int worldHeight)
        {
            _viewport = viewport;
            _worldWidth = worldWidth;
            _worldHeight = worldHeight;
        }

        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-_position, 0f)) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateScale(_zoom, _zoom, 1f) *
                Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0f));
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                // Center limits based on viewport size and zoom
                float halfW = _viewport.Width * 0.5f / _zoom;
                float halfH = _viewport.Height * 0.5f / _zoom;

                // Clamp within [halfExtent, worldExtent - halfExtent]
                float clampedX = MathHelper.Clamp(
                    value.X,
                    halfW,
                    _worldWidth - halfW);
                float clampedY = MathHelper.Clamp(
                    value.Y,
                    halfH,
                    _worldHeight - halfH);

                _position = new Vector2(clampedX, clampedY);
            }
        }
        public float Zoom
        {
            get => _zoom;
            set => _zoom = MathHelper.Clamp(value, 0.5f, 2f);
        }

        public float Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }
    }
}