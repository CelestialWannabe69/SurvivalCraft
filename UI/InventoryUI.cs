using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SurvivalCraft.Entities;
using System.Text;

namespace SurvivalCraft.UI
{
    public class InventoryUI
    {
        private SpriteFont _font;

        // Pass a sprite font when creating the InventoryUI instance.
        public InventoryUI(SpriteFont font)
        {
            _font = font;
        }

        // Draws a simple list of items in the player's inventory.
        public void Draw(SpriteBatch spriteBatch, Inventory inventory)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in inventory.Items)
            {
                sb.AppendLine($"{item.Name}: {item.Quantity}");
            }

            if (spriteBatch != null)
            {
                spriteBatch.DrawString(_font, sb.ToString(), new Vector2(10, 150), Color.White);
            }
        }
    }
}
