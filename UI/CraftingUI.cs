using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SurvivalCraft.Entities;
using SurvivalCraft.Systems;
using System.Collections.Generic;
using System.Linq;

namespace SurvivalCraft.UI
{
    public class CraftingUI
    {
        private SpriteFont _font;
        private CraftingManager _craftingManager;
        private Inventory _playerInventory;
        private bool _isVisible; // Tracks whether crafting menu is open

        private int _selectedRecipeIndex = 0; // Tracks selected recipe

        public CraftingUI(SpriteFont font, CraftingManager craftingManager, Inventory inventory)
        {
            _font = font;
            _craftingManager = craftingManager;
            _playerInventory = inventory;
            _isVisible = false;
        }

        public void ToggleVisibility()
        {
            _isVisible = !_isVisible;
        }

        public void Update()
        {
            if (!_isVisible) return;

            KeyboardState state = Keyboard.GetState();

            // Navigate recipes using arrow keys
            if (state.IsKeyDown(Keys.Down))
                _selectedRecipeIndex = (_selectedRecipeIndex + 1) % _craftingManager.Recipes.Count;
            if (state.IsKeyDown(Keys.Up))
                _selectedRecipeIndex = (_selectedRecipeIndex - 1 + _craftingManager.Recipes.Count) % _craftingManager.Recipes.Count;

            // Attempt crafting with Enter key
            if (state.IsKeyDown(Keys.Enter))
            {
                var recipe = _craftingManager.Recipes[_selectedRecipeIndex];
                if (_craftingManager.Craft(_playerInventory, recipe))
                {
                    // Craft successful feedback (you can add sound effects later)
                    System.Console.WriteLine($"Crafted: {recipe.Name}");
                }
                else
                {
                    System.Console.WriteLine($"Missing materials for: {recipe.Name}");
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_isVisible) return;

            spriteBatch.Begin();

            Vector2 pos = new Vector2(100, 100);
            spriteBatch.DrawString(_font, "Crafting Menu (Use Arrow Keys to Select, Enter to Craft)", pos, Color.White);
            pos.Y += 40;

            for (int i = 0; i < _craftingManager.Recipes.Count; i++)
            {
                var recipe = _craftingManager.Recipes[i];
                Color textColor = (_playerInventory.HasIngredients(recipe.Ingredients)) ? Color.Green : Color.Red;

                if (i == _selectedRecipeIndex)
                {
                    spriteBatch.DrawString(_font, $"> {recipe.Name}", pos, Color.Yellow);
                }
                else
                {
                    spriteBatch.DrawString(_font, recipe.Name, pos, textColor);
                }

                pos.Y += 30;
            }

            spriteBatch.End();
        }
    }
}
