using System.Collections.Generic;
using SurvivalCraft.Entities;

namespace SurvivalCraft.Systems
{
    public class CraftingRecipe
    {
        public string Name { get; set; }
        public Dictionary<string, int> Ingredients { get; set; }
        public Item Result { get; set; }
    }
}
