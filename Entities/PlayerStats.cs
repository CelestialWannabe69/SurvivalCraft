using Microsoft.Xna.Framework;

namespace SurvivalCraft.Entities
{
    public class PlayerStats
    {
        public float Health { get; private set; } = 100f;
        public float Hunger { get; private set; } = 100f;
        public float Stamina { get; private set; } = 100f;

        // Rates per second
        private const float HungerDrainRate = 2f;
        private const float StaminaRegenRate = 10f;
        private const float StaminaDrainRate = 20f;

        public void Update(GameTime gameTime, bool isMoving)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Hunger drains always
            Hunger = MathHelper.Clamp(Hunger - HungerDrainRate * dt, 0f, 100f);

            // Stamina drains when moving, regenerates otherwise
            if (isMoving)
                Stamina = MathHelper.Clamp(Stamina - StaminaDrainRate * dt, 0f, 100f);
            else
                Stamina = MathHelper.Clamp(Stamina + StaminaRegenRate * dt, 0f, 100f);

            // If hunger hits zero, health drains
            if (Hunger <= 0f)
                Health = MathHelper.Clamp(Health - 5f * dt, 0f, 100f);
        }
    }
}
