using Microsoft.Xna.Framework;

namespace SurvivalCraft.Entities
{
    public class ResourceNode
    {
        public string ResourceType { get; private set; }
        public float Durability { get; private set; }
        public Vector2 Position { get; private set; }

        // Constructor: set the type of resource, its durability (health) and its location.
        public ResourceNode(string resourceType, float durability, Vector2 position)
        {
            ResourceType = resourceType;
            Durability = durability;
            Position = position;
        }

        // Harvest method: reduce durability by a given amount.
        // Returns true if the node is depleted.
        public bool Harvest(float amount)
        {
            Durability -= amount;
            return Durability <= 0;
        }
    }
}
