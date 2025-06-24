using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SurvivalCraft.Core;
using SurvivalCraft.Entities;
using SurvivalCraft.Systems;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace SurvivalCraft
{
///test from github
    public class Game1 : Game
    {
        //Day&Night&Seasons&Weather&Evets
        private DayNightCycle _dayNight;
        private SeasonManager _seasons;
        private WeatherManager _weather;
        private SpecialEventManager _events;
        private WeatherParticleManager _weatherParticles;

        //items&inventory
        private List<SurvivalCraft.Entities.ResourceNode> _resourceNodes;
        private Texture2D _nodeTexture;    // A simple texture for drawing resource nodes.
        private SurvivalCraft.UI.InventoryUI _inventoryUI;


        //Game state
        private Core.Camera2D _camera;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private Texture2D _grassTexture;
        private Texture2D _dirtTexture;
        private World _world;

        //crafting
        private SurvivalCraft.UI.CraftingUI _craftingUI;
        private SurvivalCraft.Systems.CraftingManager _craftingManager;

        //map
        private const int MapWidthInTiles = 200;
        private const int MapHeightInTiles = 150;
        private const int TileSize = 32;

        //Day/Night cycle
       // Day/Night cycle is now entirely inside DayNightCycle
       private const float DayLength = 120f; // full cycle = 2 minutes

        //Player
        private Texture2D _playerTexture;
        private Player _player;

        //Keyboard
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        //HUD
        private Hud _hud;

        private Core.GameStateManager _stateManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _stateManager = new Core.GameStateManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //load overlay
            _dayNight = new DayNightCycle(GraphicsDevice, DayLength);
            _seasons = new SeasonManager(seasonLengthSeconds: 300f);
            _weather = new WeatherManager(_seasons);
            _events = new SpecialEventManager();
            _weatherParticles = new WeatherParticleManager(GraphicsDevice);

            // Load a texture for resource nodes (e.g. a simple square or use an asset)
            _nodeTexture = Content.Load<Texture2D>("Textures/Tulip");

            // Create a list of resource nodes for testing.
            _resourceNodes = new List<SurvivalCraft.Entities.ResourceNode>
            { // Place a node at (200,200) with a durability set to 20.
              new SurvivalCraft.Entities.ResourceNode("Tulip", 20f, new Vector2(200, 200)),
            // You can add more nodes as needed.
            };

            // Initialize Inventory UI with a SpriteFont (make sure you have a font in Content/Fonts)
            _inventoryUI = new SurvivalCraft.UI.InventoryUI(Content.Load<SpriteFont>("Fonts/DefaultFont"));


            // 1. Load font and player
            _font = Content.Load<SpriteFont>("Fonts/DefaultFont");
            _playerTexture = Content.Load<Texture2D>("Textures/player");
            _player = new Player(_playerTexture, new Vector2(100, 100), 150f);
            // (Optional) Initialize Crafting Manager
            _craftingManager = new SurvivalCraft.Systems.CraftingManager();

            // Now safely initialize `_craftingUI`
            _craftingUI = new SurvivalCraft.UI.CraftingUI(_font, _craftingManager, _player.PlayerInventory);

            //Load tile textures and create world
            _grassTexture = Content.Load<Texture2D>("Textures/grass");
            _dirtTexture = Content.Load<Texture2D>("Textures/dirt");
            //  _world = new World(width: 250, height: 150, grassTex: _grassTexture, dirtTex: _dirtTexture);
            _world = new World(
                  width: MapWidthInTiles,
                  height: MapHeightInTiles,
                  grassTex: _grassTexture,
                  dirtTex: _dirtTexture);

            // Compute world size in pixels
            int worldWidthPixels = _world.TileCountX * _world.TileSize;
            int worldHeightPixels = _world.TileCountY * _world.TileSize;

            //HUD
            _hud = new Hud(GraphicsDevice, _font);

            // Initialize the camera** after**world exists
            _camera = new Camera2D(
                GraphicsDevice.Viewport,
                worldWidth: worldWidthPixels,
                worldHeight: worldHeightPixels);
        }

        protected override void Update(GameTime gameTime)
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            InputManager.Update();
            if (InputManager.IsKeyPressed(Keys.Enter) && _stateManager.CurrentState == GameState.MainMenu)
                _stateManager.ChangeState(GameState.Playing);
            if (InputManager.IsKeyPressed(Keys.Escape))
                _stateManager.TogglePause();
            if (InputManager.IsKeyPressed(Keys.C))
            {
                System.Console.WriteLine("Crafting UI Toggled!");
                _craftingUI.ToggleVisibility();
            }
            if (InputManager.IsKeyPressed(Keys.E))
            {
                // Reference your player via _player which you already created.
                // Check each resource node to see if it’s near the player.
                for (int i = _resourceNodes.Count - 1; i >= 0; i--)
                {
                    var node = _resourceNodes[i];
                    // Here we use distance; adjust threshold as needed.
                    if (Vector2.Distance(node.Position, _player.Position) < 50f)
                    {
                        // Simulate harvesting damage, e.g., 10 units per press.
                        if (node.Harvest(10f))
                        {
                            // If true, the node is depleted.
                            _resourceNodes.RemoveAt(i);
                        }
                        // Add the resource to the player’s inventory.
                        _player.PlayerInventory.AddItem(new SurvivalCraft.Entities.Item { Name = node.ResourceType }, 5);
                        // (For testing, we add a fixed amount. Tweak as needed.)
                    }
                }
            }

            if (_stateManager.CurrentState == GameState.Playing)
            {
                /* _timeOfDay = (_timeOfDay + (float)gameTime.ElapsedGameTime.TotalSeconds) % DayLength;
                 _player.Update(gameTime);  // move player
                 _camera.Position = _player.Position;// Center camera on player*/
                // ─── Update Systems & Player ───────────────────────
                _dayNight.Update(gameTime);
                _seasons.Update(gameTime);
                _weather.Update(gameTime);
                _events.Update(gameTime);
                _weatherParticles.Update(gameTime, _weather.Current);


                // existing player & camera
                _player.Update(gameTime);
                _camera.Position = _player.Position;
            }

            Window.Title = _stateManager.CurrentState.ToString();
            base.Update(gameTime);
        }

        private bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
        }


        protected override void Draw(GameTime gameTime)
        {
            // 0) clear to a neutral color (we’ll overlay tint next)
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // 1) MAIN MENU
            if (_stateManager.CurrentState == GameState.MainMenu)
            {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(_font,
                    "Main Menu - Press ENTER to Start",
                    new Vector2(100, 100),
                    Color.White);
                _spriteBatch.End();

                base.Draw(gameTime);
                return;
            }

            // 2) GAME WORLD & PLAYER (camera space)
            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
            _world.Draw(_spriteBatch);
            _player.Draw(_spriteBatch);
            _spriteBatch.End();

            // Draw resource nodes (for simplicity, draw as rectangles using _nodeTexture)
            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
            foreach (var node in _resourceNodes)
            {
                // Draw each node at its world position; adjust the rectangle as needed
                _spriteBatch.Draw(_nodeTexture, new Rectangle((int)node.Position.X, (int)node.Position.Y, 32, 32), Color.Brown);
            }
            _spriteBatch.End();

            _weatherParticles.Draw(_spriteBatch);

            // 3) DAY/NIGHT TINT OVERLAY (screen space)
            //    get the four colors from the current season
            _seasons.GetSeasonColors(
                out var nightCol,
                out var morningCol,
                out var dayCol,
                out var eveningCol);

            //    compute the tint
            var tint = _dayNight.GetTint(nightCol, morningCol, dayCol, eveningCol);

            //    draw full-screen overlay
            _dayNight.DrawOverlay(
                _spriteBatch,
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight,
                tint);

            // 4) HUD + WEATHER + SPECIAL EVENTS (screen space)
            _spriteBatch.Begin();
            _hud.Draw(_spriteBatch, _player.Stats);

            // optional: show current weather state
            _spriteBatch.DrawString(
                _font,
                $"Weather: {_weather.Current}",
                new Vector2(10, 80),
                Color.White);

            // show special event if any
            if (_events.CurrentEvent != SurvivalCraft.Systems.SpecialEventType.None)
            {
                _spriteBatch.DrawString(
                    _font,
                    $"Special Event: {_events.CurrentEvent}",
                    new Vector2(10, 110),
                    Color.Yellow);
            }


            // paused overlay
            if (_stateManager.CurrentState == GameState.Paused)
            {
                _spriteBatch.DrawString(
                    _font,
                    "Paused - Press ESC to Resume",
                    new Vector2(100, 100),
                    Color.White);
            }

            // Draw the Inventory UI on top of the HUD
            _inventoryUI.Draw(_spriteBatch, _player.PlayerInventory);
            _craftingUI.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
