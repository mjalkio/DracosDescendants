#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;
using DracosD.Controllers;
using DracosD.Views;
using DracosD.Models;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
#endregion

namespace DracosD
{
    class GameEngine : Microsoft.Xna.Framework.Game
    {
        #region Fields
        // Used to load the sounds and graphics (CONTROLLER CLASS)
        protected ContentManager content;

        // Parses XML files to create Level object (CONTROLLER CLASSES)
        protected LevelController gameLevelController;

        // Gets inputs from the AI and PlayerInput controllers and applies them (CONTROLLER CLASS)
        protected WorldController currentWorld;

        // Draws the current menu if required, based on the Menu model (VIEW CLASS)
        protected MenuView gameMenuView;

        // draws information defined by level (camera) and HUD based elements
        protected GameView gameView;

        private Texture2D victory;
        #endregion

        #region Initialization
        /// <summary>
        /// Create a new instance of our game. 
        /// </summary>
        public GameEngine()
        {
            // Tell the program to load all files relative to the "Content" directory.
            content = new ContentManager(Services);
            content.RootDirectory = "Content";
            gameMenuView = new MenuView();
            gameView = new GameView(this);
            gameLevelController = new LevelController();
        }

        /// <summary>
        /// Preform any initialization necessary before running the game.  This
        /// includes both (preliminary) model and view creation
        /// </summary>
        protected override void Initialize()
        {
            gameView.Initialize(this);
            base.Initialize();
        }

        /// <summary>
        /// Load all graphic and audio content.
        /// </summary>
        protected override void LoadContent()
        {
            gameView.LoadContent(content);
            //gameLevelController.LoadContent(content, "..\\..\\..\\..\\WindowsGame1Content\\techPrototypeLevel.xml");
            gameLevelController.LoadContent(content, "..\\..\\..\\..\\WindowsGame1Content\\alphaLevel.xml");
            gameView.LevelWidth = (int) (gameLevelController.Width / WorldController.DEFAULT_SCALE);
            gameView.LevelHeight = (int)(gameLevelController.Height / WorldController.DEFAULT_SCALE);

            victory = content.Load<Texture2D>("victory");
            currentWorld = new WorldController(new Vector2(0, 0), gameLevelController,content);
        }

        /// <summary>
        /// Unload all graphic and audio content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Content managers make this step easy.
            content.Unload();
        }
        #endregion



        #region Game Loop
        /// <summary>
        /// Read user input, calculate physics, and update the models.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            gameView.Scale = currentWorld.Scale;
            if (currentWorld.isToReset)
            {
                gameLevelController = new LevelController();
                base.Initialize();
            }
            else
            {
                currentWorld.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            gameView.Reset();
            // World specific drawing
            currentWorld.Draw(gameView);
            // gameMenuView.Draw();

            // Final message
            if (currentWorld.Succeeded)
            {
                gameView.BeginSpritePass(BlendState.AlphaBlend);
                gameView.DrawOverlay(victory, Color.White, false); 
                gameView.EndSpritePass();
            }
            base.Draw(gameTime);
        }
        #endregion
    }
}
