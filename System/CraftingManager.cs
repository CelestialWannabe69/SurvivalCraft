using System.Collections.Generic;
using System.Linq;
using SurvivalCraft.Entities;

namespace SurvivalCraft.Systems
{
    public class CraftingManager
    {
        public List<CraftingRecipe> Recipes { get; private set; } = new List<CraftingRecipe>();

        // In the constructor, you define your available recipes.
        public CraftingManager()
        {
            // Example recipe: Craft a "Wooden Axe"
            Recipes.Add(new CraftingRecipe
            {
                Name = "Wooden Axe",
                Ingredients = new Dictionary<string, int>
                {
                    { "Wood", 10 },
                    { "Stone", 2 }
                },
                Result = new Item { Name = "Wooden Axe", Quantity = 1 }
            });
        }

        // Attempts to craft an item using the player's inventory.
        public bool Craft(Inventory inventory, CraftingRecipe recipe)
        {
            if (inventory.HasIngredients(recipe.Ingredients))
            {
                inventory.RemoveItems(recipe.Ingredients);
                inventory.AddItem(recipe.Result, recipe.Result.Quantity);
                return true;
            }
            return false;
        }
        public string TryCraft(Inventory inventory, CraftingRecipe recipe)
        {
            if (inventory.HasIngredients(recipe.Ingredients))
            {
                inventory.RemoveItems(recipe.Ingredients);
                inventory.AddItem(recipe.Result, recipe.Result.Quantity);
                return $"Successfully crafted {recipe.Name}!";
            }

            // Generate a missing items message
            List<string> missingItems = new List<string>();
            foreach (var ingredient in recipe.Ingredients)
            {
                var item = inventory.Items.FirstOrDefault(i => i.Name == ingredient.Key);
                if (item == null || item.Quantity < ingredient.Value)
                {
                    missingItems.Add($"{ingredient.Key} ({ingredient.Value} needed)");
                }
            }

            return $"Cannot craft {recipe.Name}. Missing: " + string.Join(", ", missingItems);
        }

    }
}
