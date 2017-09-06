using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace EclipsePhase
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class GameWorld : Game
    {
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public SFX SFX { get; set; }
        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }
        public int WorldWidth { get; set; }
        public int WorldHeight { get; set; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private static GameWorld instance;
        public bool GameOver { get; set; }

        public GameObjectPool gameObjectPool;
        public Random rnd;

        public KeyboardState Keystate { get; set; }

        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }

        private GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            WorldWidth = 1280;
            WorldHeight = 720;
            WindowWidth = 1280;
            WindowHeight = 720;
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            graphics.ApplyChanges();
            this.Window.AllowUserResizing = false;
            this.IsMouseVisible = true;
            SpriteWidth = 400;
            SpriteHeight = 400;

            SFX = new SFX();

            Keystate = Keyboard.GetState();

            //Initializes the gameObjectPool
            gameObjectPool = new GameObjectPool();

            gameObjectPool.AddToActive();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Places the player
            gameObjectPool.CreatePlayer(new Vector2(500, 100));

            //Places test environment
            //With scale 0.5f the dimensions are 200x200
            gameObjectPool.CreateEnvironment(new Vector2(100, 400), 0.5f);
            gameObjectPool.CreateEnvironment(new Vector2(300, 400), 0.5f);
            gameObjectPool.CreateEnvironment(new Vector2(500, 400), 0.5f);
            gameObjectPool.CreateEnvironment(new Vector2(650, 450), 0.25f);
            gameObjectPool.CreateEnvironment(new Vector2(750, 450), 0.25f);
            gameObjectPool.CreateEnvironment(new Vector2(850, 450), 0.25f);

            gameObjectPool.CreateEnvironment(new Vector2(850, 300), 0.25f);
            gameObjectPool.CreateEnvironment(new Vector2(850, 150), 0.25f);
            gameObjectPool.CreateEnvironment(new Vector2(750, 150), 0.25f);
            gameObjectPool.CreateEnvironment(new Vector2(650, 150), 0.25f);
            gameObjectPool.CreateEnvironment(new Vector2(550, 150), 0.25f);



            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            //Update the keyboard state
            Keystate = Keyboard.GetState();
            //Updates the mouse state
            MouseInputManager.Update();

            //Updates all gameObjects
            gameObjectPool.Update(gameTime);

            //Adds and removes GameObjects from the game
            gameObjectPool.RemoveFromActive();
            gameObjectPool.AddToActive();
            gameObjectPool.CollisionListForEnemy();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //Draws all gameObjects
            gameObjectPool.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
