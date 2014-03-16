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

        // Used to play the sounds (Techically, VIEW CLASS)
        protected SoundBank soundBank;
        protected AudioEngine audioEngine;
        protected WaveBank waveBank;

        //stores the current level
        protected Level currLevel;
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
        }

        /// <summary>
        /// Preform any initialization necessary before running the game.  This
        /// includes both (preliminary) model and view creation
        /// </summary>
        protected override void Initialize()
        {
            gameLevelController = new LevelController();
            currLevel = gameLevelController.parse(parse the XML file here);
            currentWorld = new WorldController(new Vector2(0, 0), currLevel);
            // gameMenuView.Initialize();
            gameView.Initialize(this);
            base.Initialize();
        }

        /// <summary>
        /// Load all graphic and audio content.
        /// </summary>
        protected override void LoadContent()
        {
            // load all graphic contents for the views
            // gameMenuView.LoadContent(content);
            gameView.LoadContent(content);

            // Sound banks allow us to play a sound "on top of itself"
            audioEngine = new AudioEngine("Content\\sounds\\torpedo40.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\sounds\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\sounds\\Sound Bank.xsb");
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
            currentWorld.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // World specific drawing
            currentWorld.Draw(gameView);
            // gameMenuView.Draw();

            /*/ Final message
            if (currentWorld.Succeeded && !currentWorld.Failed)
            {
                gameView.BeginSpritePass(BlendState.AlphaBlend);
                gameView.DrawOverlay(victory, Color.White, false); 
                gameView.EndSpritePass();
            }
            else if (currentWorld.Failed)
            {
                gameView.BeginSpritePass(BlendState.AlphaBlend);
                gameView.DrawOverlay(failure, Color.White, false);
                gameView.EndSpritePass();
            }*/
            base.Draw(gameTime);
        }
        #endregion
    }
}
