using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SurvivalCraft
{
    public class World
    {
        public int TileCountX { get; }
        public int TileCountY { get; }
        public int TileSize { get; }

        private readonly Tile[,] _tiles;

        public World(int width, int height, Texture2D grassTex, Texture2D dirtTex)
        {
            TileCountX = width;
            TileCountY = height;
            TileSize = 32; // size in pixels of each tile, match your art

            _tiles = new Tile[TileCountX, TileCountY];
            GenerateTiles(grassTex, dirtTex);
        }

        private void GenerateTiles(Texture2D grassTex, Texture2D dirtTex)
        {
            var rnd = new Random();
            for (int x = 0; x < TileCountX; x++)
                for (int y = 0; y < TileCountY; y++)
                {
                    var type = rnd.Next(0, 2) == 0 ? TileType.Grass : TileType.Dirt;
                    var tex = type == TileType.Grass ? grassTex : dirtTex;
                    var bounds = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
                    _tiles[x, y] = new Tile(type, tex, bounds);
                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in _tiles)
                tile.Draw(spriteBatch);
        }
    }
}