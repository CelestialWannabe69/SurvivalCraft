using System.Collections.Generic;
using System.Linq;

namespace SurvivalCraft.Entities
{
    public class Inventory
    {
        public List<Item> Items { get; private set; } = new List<Item>();

        // Adds an item to the inventory or increases the quantity if it already exists.
        public void AddItem(Item item, int amount)
        {
            var existing = Items.FirstOrDefault(i => i.Name == item.Name);
            if (existing != null)
            {
                existing.Quantity += amount;
            }
            else
            {
                item.Quantity = amount;
                Items.Add(item);
            }
        }

        // Checks if the inventory contains enough of the items specified in the recipe.
        public bool HasIngredients(Dictionary<string, int> recipeIngredients)
        {
            foreach (var ingredient in recipeIngredients)
            {
                var item = Items.FirstOrDefault(i => i.Name == ingredient.Key);
                if (item == null || item.Quantity < ingredient.Value)
                    return false;
            }
            return true;
        }

        // Removes items from the inventory once they are used.
        public void RemoveItems(Dictionary<string, int> recipeIngredients)
        {
            foreach (var ingredient in recipeIngredients)
            {
                var item = Items.FirstOrDefault(i => i.Name == ingredient.Key);
                if (item != null)
                {
                    item.Quantity -= ingredient.Value;
                    if (item.Quantity <= 0)
                        Items.Remove(item);
                }
            }
        }
    }
}
