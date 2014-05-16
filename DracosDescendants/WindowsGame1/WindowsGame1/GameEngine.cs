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
        protected const int NUM_LEVELS = 9;

        protected readonly string[] levelLoadLocations = {"..\\..\\..\\..\\WindowsGame1Content\\tutorialLevel.xml", "..\\..\\..\\..\\WindowsGame1Content\\level1a.xml",
                                                      "..\\..\\..\\..\\WindowsGame1Content\\level1.xml", "..\\..\\..\\..\\WindowsGame1Content\\level2.xml",
                                                      "..\\..\\..\\..\\WindowsGame1Content\\level4.xml","..\\..\\..\\..\\WindowsGame1Content\\level3.xml",
                                                         "..\\..\\..\\..\\WindowsGame1Content\\level5.xml", "..\\..\\..\\..\\WindowsGame1Content\\level6.xml",
                                                         "..\\..\\..\\..\\WindowsGame1Content\\WillLevel.xml"};
        /*protected readonly string[] levelLoadLocations = {"Content//tutorialLevel.xml", "Content//level1.xml","Content//level2.xml", "Content//level3.xml",
                                                      "Content//level1a.xml","Content//level4.xml","Content//level5.xml", "Content//level6.xml","Content//WillLevel.xml"};*/



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
        private Texture2D firstplace;
        private Texture2D secondplace;
        private Texture2D thirdplace;
        private Texture2D fourthplace;

        private Texture2D failure;
        private Texture2D menuBackground;
        private Texture2D levelSelect;
        private Texture2D pause;
        private Texture2D countdown3;
        private Texture2D countdown2;
        private Texture2D countdown1;
        private Texture2D countdownFilmstrip;
        private Texture2D level1Unlocked;
        private Texture2D level2Unlocked;
        private Texture2D level3Unlocked;
        private Texture2D level4Unlocked;
        private Texture2D level5Unlocked;
        private Texture2D level6Unlocked;
        private Texture2D level7Unlocked;
        private Texture2D level8Unlocked;
        private Texture2D tutorialUnlocked;

        private Texture2D pauseMenuResume;
        private Texture2D pauseMenuRestart;
        private Texture2D pauseMenuLevelSelect;

        private Texture2D selectionSparkleFilmstrip;
        // Contstants decided from level select texture to place sparkle
        private int startX = 423;
        private int incrementX = 180;
        private int startY = 365;
        private int incrementY = 170;

        // For sparkling animation
        private int animationFrame = 0;
        private int numSparkleFrames = 4;
        private int sparkleDelay = 4;
        private int sparkleTimer;
        private int countdownAnimationFrame;

        private int countdown;
        private float countdownTimer;
        private const int COUNTDOWN_FRAMES= 13;

        // To give time after victory before returning to level select
        private int successCountdown;

        private GameState gameState;

        // Fields necessary for the level select menu
        private int optionSelected;
        private int pauseOptionSelected;
        private PlayerInputController playerInput;
        private int levelCompleted = -1;

        //Puttin in dat music yo'
        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;
        private Cue raceCue;
        private Cue menuCue;

        private SoundEffect dragonFireSound;
        private SoundEffect gasPlanetSound;
        private SoundEffectInstance gasSound;
        private SoundEffectInstance dragonSound;
        private SoundEffect onFireSound;
        private SoundEffect gateSound;

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
            countdownFilmstrip = content.Load<Texture2D>("Countdown");
            firstplace = content.Load<Texture2D>("1st");
            secondplace = content.Load<Texture2D>("2nd");
            thirdplace = content.Load<Texture2D>("3rd");
            fourthplace = content.Load<Texture2D>("4th");

            pauseMenuResume = content.Load<Texture2D>("MidGameMenuResume");
            pauseMenuRestart = content.Load<Texture2D>("MidGameMenuRestart");
            pauseMenuLevelSelect = content.Load<Texture2D>("MidGameMenuSelectLevel");

            level1Unlocked = content.Load<Texture2D>("Level1Unlocked");
            level2Unlocked = content.Load<Texture2D>("Level2Unlocked");
            level3Unlocked = content.Load<Texture2D>("Level3Unlocked");
            level4Unlocked = content.Load<Texture2D>("Level4Unlocked");
            level5Unlocked = content.Load<Texture2D>("Level5Unlocked");
            level6Unlocked = content.Load<Texture2D>("Level6Unlocked");
            level7Unlocked = content.Load<Texture2D>("Level7Unlocked");
            level8Unlocked = content.Load<Texture2D>("Level8Unlocked");
            tutorialUnlocked = content.Load<Texture2D>("TutorialUnlocked");

            selectionSparkleFilmstrip = content.Load<Texture2D>("SelectionSparkle");
            //currentWorld = new WorldController(new Vector2(0, 0), gameLevelControllers[0],content,playerInput);

            //Umf umf umf umf
            audioEngine = new AudioEngine("Content/DracoMusic.xgs");
            waveBank = new WaveBank(audioEngine, "Content/Waves.xwb");
            soundBank = new SoundBank(audioEngine, "Content/Sounds.xsb");
            menuCue = soundBank.GetCue("menu_music");

            gasPlanetSound = content.Load<SoundEffect>("gas_planet_sound");
            gasSound = gasPlanetSound.CreateInstance();
            gasSound.Dispose();
            dragonFireSound = content.Load<SoundEffect>("fire_breath_sound");
            onFireSound = content.Load<SoundEffect>("on_fire_sound");
            gateSound = content.Load<SoundEffect>("gate_sound");
            dragonSound = null;
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

            sparkleTimer++;
            if (sparkleTimer == sparkleDelay)
            {
                sparkleTimer = 0;
                animationFrame++;
                if (animationFrame == numSparkleFrames) animationFrame = 0;
            }

            if (!(currentWorld == null) && currentWorld.Succeeded) successCountdown--;

            if (successCountdown == 0 && gameState == GameState.Game)
            {
                resetGame();
                raceCue.Stop(AudioStopOptions.Immediate);
                gameState = GameState.ChooseLevel;
                menuCue.Resume();
            }

            if (gameState == GameState.Pause)
            {
                if (playerInput.start || playerInput.Confirm)
                {
                    if (pauseOptionSelected == 0)
                    {
                        gameState = GameState.Game;
                        menuCue.Pause();
                        raceCue.Resume();
                    }
                    else if (pauseOptionSelected == 1)
                    {
                        menuCue.Stop(AudioStopOptions.Immediate);
                        resetGame();
                        raceCue.Stop(AudioStopOptions.Immediate);
                        currentWorld = new WorldController(new Vector2(0, 0), gameLevelControllers[optionSelected], content, playerInput);
                        gameView.LevelHeight = (int)currentWorld.Height;
                        gameView.LevelWidth = (int)currentWorld.Width;
                        countdown = 3;
                        countdownTimer = 0.25f;
                        countdownAnimationFrame = 0;
                        successCountdown = 180;
                        raceCue = soundBank.GetCue("race_music");
                        raceCue.Play();
                        gameState = GameState.RaceBegin;
                    }
                    else if (pauseOptionSelected == 2)
                    {
                        raceCue.Stop(AudioStopOptions.Immediate);
                        optionSelected = 0;
                        menuCue.Stop(AudioStopOptions.Immediate);
                        resetGame();
                        gameState = GameState.ChooseLevel;
                        menuCue.Resume();
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
                if (!menuCue.IsPlaying)
                {
                    menuCue.Play();
                }

                if (playerInput.start)
                {
                    optionSelected = 0;
                    gameState = GameState.ChooseLevel;
                }
            }
            else if (gameState == GameState.ChooseLevel){
                if (playerInput.start || playerInput.Confirm)
                {
                    if (optionSelected - 1 > levelCompleted)
                    {

                    }
                    else
                    {
                        menuCue.Pause();
                        currentWorld = new WorldController(new Vector2(0, 0), gameLevelControllers[optionSelected], content, playerInput);
                        gameView.LevelHeight = (int)currentWorld.Height;
                        gameView.LevelWidth = (int)currentWorld.Width;
                        countdown = 3;
                        countdownAnimationFrame = 0;
                        countdownTimer = 0.25f;
                        successCountdown = 180;
                        gameState = GameState.RaceBegin;
                        raceCue = soundBank.GetCue("race_music");
                        raceCue.Play();
                    }

                }
                else if (playerInput.Right)
                {
                    if (optionSelected < NUM_LEVELS - 1)
                    {
                        if (optionSelected <= levelCompleted)
                        {
                            optionSelected++;
                        }
                    }
                }
                //no need to change
                else if (playerInput.Left)
                {
                    if (optionSelected > 0) optionSelected--;
                }
                else if (playerInput.Down)
                {
                    if (optionSelected + 3 < NUM_LEVELS)
                    {
                        if (optionSelected + 1 < levelCompleted)
                        {
                            optionSelected += 3;
                        }
                    }
                }
                //no need to change
                else if (playerInput.Up)
                {
                    if (optionSelected - 3 >= 0) optionSelected -= 3;
                }
                //currentWorld.Update((float)gameTime.ElapsedGameTime.TotalSeconds, gameTime);
                base.Update(gameTime);
            }
            else if (gameState == GameState.RaceBegin) {
                if (countdownAnimationFrame == COUNTDOWN_FRAMES - 2)
                {
                    countdownAnimationFrame++;
                    countdownTimer = 1.0f;
                    gameState = GameState.Game;
                }
                else
                {
                    if (countdownTimer > 0) countdownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else
                    {
                        countdownAnimationFrame++;
                        countdownTimer = 0.25f;
                        //if (countdownAnimationFrame == 15) countdownTimer = 0.0f;
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
                    raceCue.Pause();
                    menuCue.Resume();
                }
                /*else if (playerInput.reset)
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
                }*/
                else
                {
                    if (countdownTimer > 0) countdownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    currentWorld.Update((float)gameTime.ElapsedGameTime.TotalSeconds, gameTime);

                    if (currentWorld.ShouldPlayGasSound && gasSound.IsDisposed)
                    {
                        gasSound = gasPlanetSound.CreateInstance();
                        gasPlanetSound.Play();
                    }
                    else if (!currentWorld.ShouldPlayGasSound && !gasSound.IsDisposed)
                    {
                        gasSound.Dispose();
                    }
                    if (currentWorld.ShouldPlayFireSound && dragonSound == null)
                    {
                        dragonSound = dragonFireSound.CreateInstance();
                        dragonSound.Play();
                    }
                    else if (!currentWorld.ShouldPlayFireSound && dragonSound != null)
                    {
                        dragonSound.Stop();
                        dragonSound = null;
                    }
                    if (currentWorld.ShouldPlayOnFireSound)
                    {
                        onFireSound.Play();
                    }
                    if (currentWorld.ShouldPlayGateSound)
                    {
                        gateSound.Play();
                    }

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
            //levelCompleted = 8;
            if (gameState == GameState.Start)
            {
                gameView.BeginSpritePass(BlendState.AlphaBlend);
                gameView.DrawOverlay(menuBackground, Color.White, true);
                gameView.EndSpritePass();
            }
            else if (gameState == GameState.ChooseLevel)
            {
                Texture2D levelUnlockedSpecific;
                levelUnlockedSpecific = tutorialUnlocked;
                if (levelCompleted == 0)
                {
                    levelUnlockedSpecific = level1Unlocked;
                }
                if (levelCompleted == 1)
                {
                    levelUnlockedSpecific = level2Unlocked;
                }
                if (levelCompleted == 2)
                {
                    levelUnlockedSpecific = level3Unlocked;
                }
                if (levelCompleted == 3)
                {
                    levelUnlockedSpecific = level4Unlocked;
                }
                if (levelCompleted == 4)
                {
                    levelUnlockedSpecific = level5Unlocked;
                }
                if (levelCompleted == 5)
                {
                    levelUnlockedSpecific = level6Unlocked;
                }
                if (levelCompleted == 6)
                {
                    levelUnlockedSpecific = level7Unlocked;
                }
                if (levelCompleted == 7)
                {
                    levelUnlockedSpecific = level8Unlocked;
                }
                if (levelCompleted == 8)
                {
                    levelUnlockedSpecific = level8Unlocked;
                }

                gameView.BeginSpritePass(BlendState.AlphaBlend);
                gameView.DrawOverlay(levelUnlockedSpecific, Color.White, false);
                gameView.EndSpritePass();

                int gridX = optionSelected%3;
                int gridY = optionSelected/3;
                
                gameView.BeginSpritePass(BlendState.AlphaBlend);
                gameView.DrawOverlay(selectionSparkleFilmstrip,Color.White,new Vector2(startX+gridX*incrementX,startY+gridY*incrementX),
                    new Vector2(0.85f, 0.85f), 0.0f, animationFrame, 4, SpriteEffects.None);
                gameView.EndSpritePass();
                
            }
            else if (gameState == GameState.RaceBegin)
            {
                currentWorld.Draw(gameView);
                Vector2 pos   = new Vector2(gameView.Width,gameView.Height)/2.0f;
                Vector2 scale = new Vector2(1, 1); // To counter global scale
                int lapFrameSize = (int)(countdownFilmstrip.Width/COUNTDOWN_FRAMES);
                gameView.BeginSpritePass(BlendState.AlphaBlend);
                gameView.DrawOverlay(countdownFilmstrip, Color.White, pos, scale, 0.0f, countdownAnimationFrame, COUNTDOWN_FRAMES, SpriteEffects.None);
                gameView.EndSpritePass();
                /*if (countdown == 3)
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
                }*/
            }
            else if (gameState == GameState.Game)
            {
                // World specific drawing
                currentWorld.Draw(gameView);

                if (countdownTimer > 0)
                {
                    Vector2 pos = new Vector2(gameView.Width, gameView.Height) / 2.0f;
                    Vector2 scale = new Vector2(1, 1); // To counter global scale
                    int lapFrameSize = (int)(countdownFilmstrip.Width / COUNTDOWN_FRAMES);
                    gameView.BeginSpritePass(BlendState.AlphaBlend);
                    gameView.DrawOverlay(countdownFilmstrip, Color.White, pos, scale, 0.0f, countdownAnimationFrame, COUNTDOWN_FRAMES, SpriteEffects.None);
                    gameView.EndSpritePass();
                }

                // Final message
                if (currentWorld.Succeeded)
                {
                    switch (currentWorld.FinishingPlace)
                    {
                        case 1:
                            gameView.BeginSpritePass(BlendState.AlphaBlend);
                            gameView.DrawOverlay(firstplace, Color.White, false);
                            gameView.EndSpritePass();
                            break;
                        case 2:
                            gameView.BeginSpritePass(BlendState.AlphaBlend);
                            gameView.DrawOverlay(secondplace, Color.White, false);
                            gameView.EndSpritePass();
                            break;
                        case 3:
                            gameView.BeginSpritePass(BlendState.AlphaBlend);
                            gameView.DrawOverlay(thirdplace, Color.White, false);
                            gameView.EndSpritePass();
                            break;
                        case 4:
                            gameView.BeginSpritePass(BlendState.AlphaBlend);
                            gameView.DrawOverlay(fourthplace, Color.White, false);
                            gameView.EndSpritePass();
                            break;
                    }
                    if (optionSelected > levelCompleted)
                    {
                        levelCompleted = optionSelected;
                    }
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
                if (pauseOptionSelected == 0) gameView.DrawOverlay(pauseMenuResume, Color.White, false);
                else if (pauseOptionSelected == 1) gameView.DrawOverlay(pauseMenuRestart, Color.White, false);
                else if (pauseOptionSelected == 2) gameView.DrawOverlay(pauseMenuLevelSelect, Color.White, false);
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
            gameLevelControllers = new LevelController[NUM_LEVELS];
            for (int i = 0; i < NUM_LEVELS; i++)
            {
                gameLevelControllers[i] = new LevelController();
            }
            // gameState = GameState.ChooseLevel;
             base.Initialize();
            //base.Update(gameTime);
             menuCue = soundBank.GetCue("menu_music");
             menuCue.Play();
             menuCue.Pause();
        }
    }
}
