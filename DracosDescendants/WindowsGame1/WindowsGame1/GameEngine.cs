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
    public enum GameState
    {
        Start,
        ChooseLevel,
        RaceBegin,
        Game,
        Pause
    }


    class GameEngine : Microsoft.Xna.Framework.Game
    {
        protected const int NUM_LEVELS = 5;

        protected readonly string[] levelLoadLocations = {"..\\..\\..\\..\\WindowsGame1Content\\tutorialLevel.xml", "..\\..\\..\\..\\WindowsGame1Content\\level1.xml",
                                                      "..\\..\\..\\..\\WindowsGame1Content\\level2.xml", "..\\..\\..\\..\\WindowsGame1Content\\level3.xml",
                                                      "..\\..\\..\\..\\WindowsGame1Content\\level3.xml"};


        #region Fields
        // Used to load the sounds and graphics (CONTROLLER CLASS)
        protected ContentManager content;

        // Parses XML files to create Level object (CONTROLLER CLASSES)
        protected LevelController[] gameLevelControllers;

        // Gets inputs from the AI and PlayerInput controllers and applies them (CONTROLLER CLASS)
        protected WorldController currentWorld;

        // Draws the current menu if required, based on the Menu model (VIEW CLASS)
        protected MenuView gameMenuView;

        // draws information defined by level (camera) and HUD based elements
        protected GameView gameView;

        private Texture2D victory;
        private Texture2D failure;
        private Texture2D menuBackground;
        private Texture2D levelSelect;
        private Texture2D pause;
        private Texture2D countdown3;
        private Texture2D countdown2;
        private Texture2D countdown1;

        private int countdown;
        private float countdownTimer;

        private GameState gameState;

        // Fields necessary for the level select menu
        private int optionSelected;
        private int pauseOptionSelected;
        private PlayerInputController playerInput;


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
            playerInput = new PlayerInputController();
            gameLevelControllers = new LevelController[NUM_LEVELS];
            for (int i=0;i<NUM_LEVELS;i++){
                gameLevelControllers[i] = new LevelController();
            }
            //currentWorld = new WorldController(new Vector2(0, 0), gameLevelController, content);

            gameState = new GameState();
            gameState = GameState.Start;
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

            for (int i = 0; i < NUM_LEVELS; i++)
            {
                gameLevelControllers[i].LoadContent(content, levelLoadLocations[i]);
            }

            //gameLevelController.LoadContent(content, "..\\..\\..\\..\\WindowsGame1Content\\alphaLevel.xml");
            //gameLevelController.LoadContent(content, "..\\..\\..\\..\\WindowsGame1Content\\betaLevel.xml");
            //gameView.LevelWidth = (int) (gameLevelController.Width / WorldController.DEFAULT_SCALE);
            //gameView.LevelHeight = (int)(gameLevelController.Height / WorldController.DEFAULT_SCALE);

            victory = content.Load<Texture2D>("victory");
            failure = content.Load<Texture2D>("failure");
            menuBackground = content.Load<Texture2D>("badStartScreen");
            levelSelect = content.Load<Texture2D>("roughLevelSelect");
            pause = content.Load<Texture2D>("paused");
            countdown3 = content.Load<Texture2D>("countdown3");
            countdown2 = content.Load<Texture2D>("countdown2");
            countdown1 = content.Load<Texture2D>("countdown1");
            //currentWorld = new WorldController(new Vector2(0, 0), gameLevelControllers[0],content,playerInput);
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
            playerInput.ReadInput();
            if (gameState == GameState.Pause)
            {
                if (playerInput.start)
                {
                    if (pauseOptionSelected == 0) gameState = GameState.Game;
                    else if (pauseOptionSelected == 1) {
                        resetGame();
                        currentWorld = new WorldController(new Vector2(0, 0), gameLevelControllers[optionSelected], content, playerInput);
                        gameView.LevelHeight = (int)currentWorld.Height;
                        gameView.LevelWidth = (int)currentWorld.Width;
                        countdown = 3;
                        countdownTimer = 1.0f;
                        gameState = GameState.RaceBegin;
                    }
                    else if (pauseOptionSelected == 2)
                    {
                        optionSelected = 0;
                        resetGame();
                        gameState = GameState.ChooseLevel;
                    }
                }
                else if (playerInput.Down)
                {
                    if (pauseOptionSelected < 2) pauseOptionSelected++;
                }
                else if (playerInput.Up)
                {
                    if (pauseOptionSelected > 0) pauseOptionSelected--;
                }
            }
            else if (gameState == GameState.Start){
                if (playerInput.start)
                {
                    optionSelected = 0;
                    gameState = GameState.ChooseLevel;
                }
            }
            else if (gameState == GameState.ChooseLevel){    
                if (playerInput.start)
                {
                    currentWorld = new WorldController(new Vector2(0, 0), gameLevelControllers[optionSelected],content,playerInput);
                    gameView.LevelHeight = (int)currentWorld.Height;
                    gameView.LevelWidth = (int)currentWorld.Width;
                    countdown = 3;
                    countdownTimer = 1.0f;
                    gameState = GameState.RaceBegin;
                }
                else if (playerInput.Down)
                {
                    //Debug.Print("DOWN WAS PRESSED");
                    if (optionSelected < NUM_LEVELS-1) optionSelected++;
                }
                else if (playerInput.Up)
                {
                    //Debug.Print("UP WAS PRESSED");
                    if (optionSelected > 0) optionSelected--;
                }
                //currentWorld.Update((float)gameTime.ElapsedGameTime.TotalSeconds, gameTime);
                base.Update(gameTime);
            }
            else if (gameState == GameState.RaceBegin) {
                if (countdown == 0) gameState = GameState.Game;
                else
                {
                    if (countdownTimer > 0) countdownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else
                    {
                        countdown--;
                        countdownTimer = 1.0f;
                    }
                }
            }
            else if (gameState == GameState.Game)
            {
                gameView.Scale = currentWorld.Scale;
                if (playerInput.start)
                {
                    pauseOptionSelected = 0;
                    gameState = GameState.Pause;
                }
                else if (playerInput.reset)
                {
                    currentWorld.Reset = false;
                    //gameState = GameState.Start;
                    gameLevelControllers = new LevelController[NUM_LEVELS];
                    for (int i = 0; i < NUM_LEVELS; i++)
                    {
                        gameLevelControllers[i] = new LevelController();
                    }
                    gameView.gateMissed = new bool[4];
                    gameView.prevGates = new float[4];
                    gameView.stoppedPosition = 0.0f;
                    base.Initialize();
                    base.Update(gameTime);
                    gameState = GameState.ChooseLevel;
                }
                else
                {
                    currentWorld.Update((float)gameTime.ElapsedGameTime.TotalSeconds, gameTime);
                    base.Update(gameTime);
                }
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            gameView.Reset();

            if (gameState == GameState.Start)
            {
                gameView.BeginSpritePass(BlendState.AlphaBlend);
                gameView.DrawOverlay(menuBackground, Color.White, false);
                gameView.EndSpritePass();
            }
            else if (gameState == GameState.ChooseLevel)
            {
                gameView.BeginSpritePass(BlendState.AlphaBlend);
                gameView.DrawOverlay(levelSelect, Color.White, false);
                gameView.EndSpritePass();
                gameView.BeginSpritePass(BlendState.AlphaBlend);
                // GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                // For some reason this drawtext does not work after a reset...
                gameView.DrawText("Current Level Selected: " + (optionSelected + 1), Color.DarkCyan, new Vector2(500, 700), true);
                gameView.EndSpritePass();
            }
            else if (gameState == GameState.RaceBegin)
            {
                currentWorld.Draw(gameView);
                if (countdown == 3)
                {
                    gameView.BeginSpritePass(BlendState.AlphaBlend);
                    gameView.DrawOverlay(countdown3, Color.White, false);
                    gameView.EndSpritePass();
                }
                else if (countdown == 2)
                {
                    gameView.BeginSpritePass(BlendState.AlphaBlend);
                    gameView.DrawOverlay(countdown2, Color.White, false);
                    gameView.EndSpritePass();
                }
                else if (countdown == 1)
                {
                    gameView.BeginSpritePass(BlendState.AlphaBlend);
                    gameView.DrawOverlay(countdown1, Color.White, false);
                    gameView.EndSpritePass();
                }
            }
            else if (gameState == GameState.Game)
            {
                // World specific drawing
                currentWorld.Draw(gameView);

                // Final message
                if (currentWorld.Succeeded)
                {
                    gameView.BeginSpritePass(BlendState.AlphaBlend);
                    gameView.DrawOverlay(victory, Color.White, false);
                    gameView.EndSpritePass();
                }
                if (currentWorld.Failed)
                {
                    gameView.BeginSpritePass(BlendState.AlphaBlend);
                    gameView.DrawOverlay(failure, Color.White, false);
                    gameView.EndSpritePass();
                }
            }
            else if (gameState == GameState.Pause)
            {
                currentWorld.Draw(gameView);
                gameView.BeginSpritePass(BlendState.AlphaBlend);
                //gameView.DrawOverlay(pause, Color.White, false);
                if (pauseOptionSelected == 0) gameView.DrawText("Resume", Color.White, new Vector2(0, 3));
                else if (pauseOptionSelected == 1) gameView.DrawText("Restart Race", Color.White, new Vector2(0, 3));
                else if (pauseOptionSelected == 2) gameView.DrawText("Level Select", Color.White, new Vector2(0, 3));
                gameView.EndSpritePass();
            }

            base.Draw(gameTime);
        }
        #endregion

        private void resetGame()
        {
            currentWorld.Reset = false;
            //gameState = GameState.Start;
            gameView.gateMissed = new bool[4];
            gameView.prevGates = new float[4];
            gameLevelControllers = new LevelController[5];
            for (int i = 0; i < NUM_LEVELS; i++)
            {
                gameLevelControllers[i] = new LevelController();
            }
            // gameState = GameState.ChooseLevel;
             base.Initialize();
            //base.Update(gameTime);
             
        }
    }
}
