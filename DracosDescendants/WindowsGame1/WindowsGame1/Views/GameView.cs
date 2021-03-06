﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace DracosD.Views
{

    #region Enum
    /// <summary>
    /// Draw states to keep track of our current drawing pass
    /// </summary>
    /// <remarks>
    /// There is no order on drawing passes for this view, but the view
    /// can only be in one drawing pass at a time.  If you wish to change
    /// the current drawing state, you must ifrst end any pass that is 
    /// currently active.  In particular, the sprite pass is not drawn to
    /// the screen until you end it.
    /// </remarks>
    public enum DrawState
    {
        Inactive,       // Not in the middle of any pass.
        SpritePass,     // In the middle of a SpriteBatch pass.
        PolygonPass,     // In the middle of a Polygon pass.
        BackgroundPass
    };
    #endregion


    public class GameView
    {
        
    #region Constants
        // Default window size.  This belongs in the view, not the game engine.
        private const int GAME_WIDTH = 1200;
        private const int GAME_HEIGHT = 900;
    #endregion

    #region Fields
        // Used to track window properties
        protected GraphicsDeviceManager graphics;
        protected GameWindow window;
        protected Rectangle bounds;

        // For drawing sprites
        protected SpriteBatch spriteBatch;

        // For drawing polygons
        protected BasicEffect effect;

        //the background of the game
        protected Texture2D background;
        protected Texture2D f_background;

        //the HUD of the game
        protected Texture2D progressTexture;
        protected Texture2D firebarTexture;
        protected Texture2D innerfirebarTexture;
        protected Texture2D dragonheadTexture;
        protected Texture2D dragonheadTexture2; //the blaze head
        protected Texture2D dragonheadTexture3;
        protected Texture2D dragonheadTexture4;
        protected Texture2D arrowTexture;

        // Track the current drawing pass. 
        protected DrawState state;

        // For onscreen messages
        protected SpriteFont font;
        protected SpriteFont fancyFont;

        // Private variable for property IsFullscreen.
        protected bool fullscreen;

        //if a gate is missed, used for HUD display
        public bool[] gateMissed = new bool[4];
        public float[] prevGates = new float[4];
        public float[] stoppedPosition = new float[4];

        // Attributes to rescale the image
        protected Matrix transform;
        protected Vector2 worldScale;

        //camera for scrolling
        private Camera camera;
        //camera for drawing background
        private Camera backcamera;

        private int levelWidth;
        private int levelHeight;
    #endregion

    #region Properties (READ-WRITE)
        /// <summary>
        /// Whether this instance is fullscreen.
        /// </summary>
        /// <remarks>
        /// This value cannot be reset during an active drawing pass.
        /// </remarks>
        public bool IsFullscreen {
            get { return fullscreen; }
            set {
                Debug.Assert(state == DrawState.Inactive, "Cannot reset property while actively drawing");
                if (fullscreen != value && graphics != null) {
                    graphics.IsFullScreen = value;
                    graphics.ApplyChanges();
                }
                fullscreen = value;
            }
        }

        /// <summary>
        /// The rectangular bounds of the drawing Window.
        /// </summary>
        /// <remarks>
        /// This code has some VERY Windows specific code in it that is
        /// incompatible with MonoGame. If you wish to use MonoGame, you 
        /// need to use the Gameview version for that engine.
        /// 
        /// This value cannot be reset during an active drawing pass.
        /// </remarks>
        public Rectangle Bounds {
            get {
                // Update bounds to the current state
                if (graphics != null) {
                    bounds.X = window.ClientBounds.X;
                    bounds.Y = window.ClientBounds.Y;
                    bounds.Width = graphics.PreferredBackBufferWidth;
                    bounds.Height = graphics.PreferredBackBufferHeight;
                }
                return bounds;
            }
            set {
                Debug.Assert(state == DrawState.Inactive, "Cannot reset property while actively drawing");
                bounds = value;
                if (graphics != null) {
                    // Code to change window size
                    graphics.PreferredBackBufferWidth = bounds.Width;
                    graphics.PreferredBackBufferHeight = bounds.Height;
                    graphics.ApplyChanges();

                    // Code to change window position
                    System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(window.Handle);
                    form.Location = new System.Drawing.Point(bounds.X, bounds.Y);
                }
            }
        }

        /// <summary>
        /// The left side of this game window
        /// </summary>
        /// <remarks>
        /// This code has some VERY Windows specific code in it that is
        /// incompatible with MonoGame. If you wish to use MonoGame, you 
        /// need to use the Gameview version for that engine.
        /// 
        /// This value cannot be reset during an active drawing pass.
        /// </remarks>
        public int X {
            get {
                if (window != null && state == DrawState.Inactive) {
                    bounds.X = window.ClientBounds.X;
                }
                return bounds.X;
            }
            set {
                Debug.Assert(state == DrawState.Inactive, "Cannot reset property while actively drawing");
                bounds.X = value;
                if (window != null) {
                    System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(window.Handle);
                    form.Location = new System.Drawing.Point(bounds.X, bounds.Y);
                }
            }
        }

        /// <summary>
        /// The upper side of this game window.
        /// </summary>
        /// <remarks>
        /// This code has some VERY Windows specific code in it that is
        /// incompatible with MonoGame. If you wish to use MonoGame, you 
        /// need to use the Gameview version for that engine.
        /// 
        /// This value cannot be reset during an active drawing pass.
        /// </remarks>
        public int Y {
            get {
                if (window != null && state == DrawState.Inactive) {
                    bounds.Y = window.ClientBounds.Y;
                }
                return bounds.Y;
            }
            set {
                Debug.Assert(state == DrawState.Inactive, "Cannot reset property while actively drawing");
                bounds.Y = value;
                if (window != null) {
                    System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(window.Handle);
                    form.Location = new System.Drawing.Point(bounds.X, bounds.Y);
                }
            }
        }

        /// <summary>
        /// The width of this drawing view
        /// </summary>
        /// <remarks>
        /// This value cannot be reset during an active drawing pass.
        /// </remarks>
        public int Width {
            get {
                if (graphics != null && state == DrawState.Inactive) {
                    bounds.Width = graphics.PreferredBackBufferWidth;
                }
                return bounds.Width;
            }
            set {
                Debug.Assert(state == DrawState.Inactive, "Cannot reset property while actively drawing");
                bounds.Width = value;
                if (graphics != null && state == DrawState.Inactive) {
                    graphics.PreferredBackBufferWidth = value;
                    graphics.ApplyChanges();
                }
            }
        }

        /// <summary>
        /// The height of this drawing view
        /// </summary>
        /// <remarks>
        /// This value cannot be reset during an active drawing pass.
        /// </remarks>
        public int Height {
            get {
                if (graphics != null) {
                    bounds.Height = graphics.PreferredBackBufferHeight;
                }
                return bounds.Height;
            }
            set {
                Debug.Assert(state == DrawState.Inactive, "Cannot reset property while actively drawing");
                bounds.Height = value;
                if (graphics != null) {
                    graphics.PreferredBackBufferWidth = value;
                    graphics.ApplyChanges();
                }
            }
        }

        /// <summary>
        /// The global scale of this drawing view
        /// </summary>
        /// <remarks>
        /// Altering this value will zoom in or out of the view (anchored at 
        /// the bottom left corner).
        /// 
        /// This value cannot be reset during an active drawing pass.
        /// </remarks>
        public Vector2 Scale {
            get { return worldScale; }
            set {
                Debug.Assert(state == DrawState.Inactive, "Cannot reset property while actively drawing");
                Debug.Assert(value.X > 0 && value.Y > 0, "Scale attributes must be positive");
                worldScale = value;
                transform = Matrix.CreateScale(worldScale.X,worldScale.Y,1.0f);
            }
        }

        /// <summary>
        /// The global X-axis scale of this drawing view
        /// </summary>
        /// <remarks>
        /// This value cannot be reset during an active drawing pass.
        /// </remarks>
        public float SX {
            get { return worldScale.X; }
            set {
                Debug.Assert(state == DrawState.Inactive, "Cannot reset property while actively drawing");
                Debug.Assert(value > 0, "Scale attributes must be positive");
                worldScale.X = value;
                transform = Matrix.CreateScale(worldScale.X, worldScale.Y, 1.0f);
            }
        }

        /// <summary>
        /// The global Y-axis scale of this drawing view
        /// </summary>
        /// <remarks>
        /// This value cannot be reset during an active drawing pass.
        /// </remarks>
        public float SY {
            get { return worldScale.Y; }
            set {
                Debug.Assert(state == DrawState.Inactive, "Cannot reset property while actively drawing");
                Debug.Assert(value > 0, "Scale attributes must be positive");
                worldScale.Y = value;
                transform = Matrix.CreateScale(worldScale.X, worldScale.Y, 1.0f);
            }
        }

        /// <summary>
        /// The font for displaying messages.
        /// </summary>
        public SpriteFont Font {
            get { return font; }
            set { font = value; }
        }

        public int LevelWidth
        {
            set { levelWidth = value; }
            get { return levelWidth; }
        }

        public int LevelHeight
        {
            set { levelHeight = value; }
            get { return levelHeight; }
        }
    #endregion

    #region Initialization
        /// <summary>
        /// Constructor to create a new instance of our view.
        /// </summary>
        /// <remarks>
        /// Note that we initialize the graphics device manager as soon
        /// as this view is constructed. However, we do NOT create a
        /// new SpriteBatch yet.  We have to wait for the graphics manager 
        /// to initialize before we do that. 
        /// </remarks>
        /// <param name="game">The root game engine for this view</param>
        public GameView(Game game) {
            // Create a new graphics manager.
            
            fullscreen = false;
            graphics = new GraphicsDeviceManager(game);
            graphics.IsFullScreen = fullscreen;

            bounds.Width = GAME_WIDTH;
            bounds.Height = GAME_HEIGHT;

            Scale = Vector2.One;
        }

        /// <summary>
        /// Initialize the SpriteBatch for this view and prepare for drawing.
        /// </summary>
        /// <remarks>
        /// This method is called by the Initialize method of the game engine,
        /// after all of the game has finished all necessary off-screen
        /// initialization.
        /// </remarks>
        /// <param name="game">The root game engine for this view</param>
        public void Initialize(Game game) {
            // Override window position at start-up
            window = game.Window;
            if (!fullscreen) {
                graphics.PreferredBackBufferWidth = bounds.Width;
                graphics.PreferredBackBufferHeight = bounds.Height;
                graphics.ApplyChanges();
            }

            // Set up Sprite Batch properites
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            // Set up polygon properties
            effect = new BasicEffect(game.GraphicsDevice);
            effect.View = Matrix.Identity;
            effect.Projection = Matrix.CreateOrthographicOffCenter(0, bounds.Width, bounds.Height, 0, 0, 1);
            effect.TextureEnabled = true;
            effect.LightingEnabled = false;

            // We are not actively drawing
            state = DrawState.Inactive;
        }

        /// <summary>
        /// Load all default graphics resources for the view
        /// </summary>
        /// <param name='content'>
        /// Reference to global content manager.
        /// </param>
        public void LoadContent(ContentManager content) {
            // Load sprite font
            font = content.Load<SpriteFont>("PhysicsFont");
            fancyFont = content.Load<SpriteFont>("WillFont");
            //load background
            background = content.Load<Texture2D>("stars");
            f_background = content.Load<Texture2D>("stars-parallax front");
            progressTexture = content.Load<Texture2D>("progressbar");
            firebarTexture = content.Load<Texture2D>("FireBar");
            innerfirebarTexture = content.Load<Texture2D>("InnerFireBar");
            dragonheadTexture = content.Load<Texture2D>("dragonhead");
            dragonheadTexture2 = content.Load<Texture2D>("blaze_head");
            dragonheadTexture3 = content.Load<Texture2D>("dragonhead3");
            dragonheadTexture4 = content.Load<Texture2D>("dragonhead4");
            arrowTexture = content.Load<Texture2D>("arrow");
        }


        /// <summary>
        /// Eliminate any resources that prevent garbage collection.
        /// </summary>
        public void Dispose() {
            graphics = null;
            window = null;
            effect.Dispose();
        }
    #endregion

    #region Drawing Methods
        /// <summary>
        /// Clear the view and reset the drawing state for a new animation frame.
        /// </summary>
        public void Reset() {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Allow either pass to follow.
            state = DrawState.Inactive;
        }

        #region Sprite Pass

        /// <summary>
        /// Start a new pass to draw sprites
        /// </summary>
        /// <remarks>
        /// Once a pass has begin, you cannot change attributes or draw polygons
        /// until it has ended.
        /// </remarks>
        /// <param name="blend">Blending mode for combining sprites</param>
        public void BeginSpritePass(BlendState blend, Vector2 position) {
            // Check that state invariant is satisfied.
            Debug.Assert(state == DrawState.Inactive, "Drawing state is invalid (expected Inactive)");
            state = DrawState.SpritePass;

            //Define the Camera
            camera = new Camera(graphics.GraphicsDevice.Viewport, levelWidth, levelHeight,10.0f);

            camera.Pos = position;
            //Console.WriteLine("here");
            // Set up the drawing view to use the appropriate blending.
            // Deferred sorting guarantees Sprites are drawn in order given.
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, null, null, null, null, camera.GetTransformation());
        }

        public void BeginSpritePass(BlendState blend)
        {
            // Check that state invariant is satisfied.
            Debug.Assert(state == DrawState.Inactive, "Drawing state is invalid (expected Inactive)");
            state = DrawState.SpritePass;

            //Console.WriteLine("here");
            // Set up the drawing view to use the appropriate blending.
            // Deferred sorting guarantees Sprites are drawn in order given.
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, null, null, null, null, transform);
        }

        /// <summary>
        /// Start a new pass to draw text sprites specifically
        /// </summary>
        /// <remarks>
        /// Once a pass has begin, you cannot change attributes or draw polygons
        /// until it has ended.
        /// </remarks>
        /// <param name="blend">Blending mode for combining sprites</param>
        public void BeginTextSpritePass(BlendState blend, Vector2 position)
        {
            // Check that state invariant is satisfied.
            Debug.Assert(state == DrawState.Inactive, "Drawing state is invalid (expected Inactive)");
            state = DrawState.SpritePass;

            //Define the Camera
            //camera = new Camera(graphics.GraphicsDevice.Viewport, 100, 100, 20.0f);

            // camera.Pos = position;
            //Console.WriteLine("here");
            // Set up the drawing view to use the appropriate blending.
            // Deferred sorting guarantees Sprites are drawn in order given.
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, null, null, null, null);

        }


        /// <summary>
        /// Draw a sprite on this drawing view.
        /// </summary>
        /// <remarks>
        /// The image is scaled according to the view Scale attribute.
        /// </remarks>
        /// <param name="image">Sprite to draw</param>
        /// <param name="tint">Color to tint sprite</param>
        /// <param name="position">Location to draw image on view</param>
        public void DrawSprite(Texture2D image, Color tint, Vector2 position) {
            // Enforce pass invariants.
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");

            // Re-center the object.
            position.X -= image.Width / 2;
            position.Y -= image.Height / 2;

            // Draw it.
            spriteBatch.Draw(image, position, tint);
        }

        /// <summary>
        /// Draw a sprite on this drawing view.
        /// </summary>
        /// <remarks>
        /// The image is scaled according to the view Scale attribute.
        /// </remarks>
        /// <param name="image">Sprite to draw</param>
        /// <param name="tint">Color to tint sprite</param>
        /// <param name="position">Location to draw image on view</param>
        /// <param name="scale">Amount to scale image (in addition to global scale)</param>
        /// <param name="angle">Amount to rotate image in radians</param>
        public void DrawSprite(Texture2D image, Color tint, Vector2 position, Vector2 scale, float angle) {
            // Enforce invariants.
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");

            // Get the texture center.
            Vector2 origin = new Vector2(image.Width / 2, image.Height / 2);

            // Draw it.
            spriteBatch.Draw(image, position, null, tint, angle, origin, scale, SpriteEffects.None, 0);

            if (position.X < 100.0f)
            {
                spriteBatch.Draw(image, new Vector2(position.X+(float)levelWidth,position.Y), null, tint, angle, origin, scale, SpriteEffects.None, 0);
            }

            if (position.X > levelWidth-100.0f)
            {
                spriteBatch.Draw(image, new Vector2(position.X - (float)levelWidth, position.Y), null, tint, angle, origin, scale, SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// Draw a sprite on this drawing view.
        /// </summary>
        /// <remarks>
        /// The image is scaled according to the view Scale attribute.
        /// </remarks>
        /// <param name="image">Sprite to draw</param>
        /// <param name="tint">Color to tint sprite</param>
        /// <param name="position">Location to draw image on view</param>
        /// <param name="scale">Amount to scale image (in addition to global scale)</param>
        /// <param name="angle">Amount to rotate image in radians</param>
        /// <param name="effects">Sprite effect to flip image</param>
        /*public void DrawSprite(Texture2D image, Color tint, Vector2 position, Vector2 scale, float angle, SpriteEffects effects) {
            // Enforce invariants.
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");

            // Get the texture center.
            Vector2 origin = new Vector2(image.Width / 2, image.Height / 2);

            // Draw it.
            spriteBatch.Draw(image, position, null, tint, angle, origin, scale, effects, 0);
        }*/

        public void DrawSprite(Texture2D image, Color tint, Vector2 position, Vector2 scale, float angle, SpriteEffects effect)
        {
            // Enforce invariants.
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");

            // Get the texture center.
            Vector2 origin = new Vector2(image.Width / 2, image.Height / 2);

            // Draw it.
            spriteBatch.Draw(image, position, null, tint, angle, origin, scale, effect, 0);

            if (position.X < 100.0f)
            {
                spriteBatch.Draw(image, new Vector2(position.X + (float)levelWidth, position.Y), null, tint, angle, origin, scale, effect, 0);
            }

            if (position.X > levelWidth - 100.0f)
            {
                spriteBatch.Draw(image, new Vector2(position.X - (float)levelWidth, position.Y), null, tint, angle, origin, scale, effect, 0);
            }
        }

        /// <summary>
        /// Animate a sprite on this drawing view.
        /// </summary>
        /// <remarks>
        /// This version of the drawing method will animate an image over a 
        /// filmstrip. It assumes that the filmstrip is a SINGLE LINE of 
        /// images.  You must modify the code if this is not the case.
        /// </remarks>
        /// <param name="image">Sprite to draw</param>
        /// <param name="tint">Color to tint sprite</param>
        /// <param name="position">Location to draw image on view</param>
        /// <param name="scale">Amount to scale image (in addition to global scale)</param>
        /// <param name="angle">Amount to rotate image in radians</param>
        /// <param name="frame">Current animation frame</param>
        /// <param name="framesize">Number of frames in filmstrip</param>
        public void DrawSprite(Texture2D image, Color tint, Vector2 position, Vector2 scale, float angle, int frame, int framesize, SpriteEffects effect) {
            // Enforce invariants.
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");

            // Pick out the right frame
            int width = image.Width / framesize;
            int height = image.Height;

            // Compute frame position assuming only 1 row of frames.
            Rectangle src  = new Rectangle(frame * width, 0, width, height);
            Vector2 origin = new Vector2(width / 2, height / 2);

            // Draw it.
            spriteBatch.Draw(image, position, src, tint, angle, origin, scale, effect, 0);

            if (position.X < 100.0f)
            {
                spriteBatch.Draw(image, new Vector2(position.X + (float)levelWidth, position.Y), src, tint, angle, origin, scale, effect, 0);
            }

            if (position.X > levelWidth - 100.0f)
            {
                spriteBatch.Draw(image, new Vector2(position.X - (float)levelWidth, position.Y), src, tint, angle, origin, scale, effect, 0);
            }
        }

        /// <summary>
        /// Draw an unscaled overlay image.
        /// </summary>
        /// <remarks>
        /// An overlay image is one that is not scaled according to the current zoom.
        /// This is ideal for backgrounds, foregrounds and uniform HUDs that do not
        /// track the camera. Images are put in the center of the window.
        /// </remarks>
        /// <param name="image">Sprite to draw</param>
        /// <param name="tint">Color to tint sprite</param>
        /// <param name="fill">Whether to stretch the image to fill the window</param>
        public void DrawOverlay(Texture2D image, Color tint, bool fill) {
            // Enforce invariants.
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");

            Vector2 pos   = new Vector2(Width,Height)/(2*Scale);
            Vector2 orig  = new Vector2(image.Width/2, image.Height/2);
            Vector2 scale = new Vector2(1/SX, 1/SY); // To counter global scale
            if (fill) {
                scale.X = Width / (image.Width*SX);
                scale.Y = Height / (image.Height*SY);
            }

            // Draw this unscaled
            spriteBatch.Draw(image, pos, null, tint, 0.0f, orig, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw an unscaled overlay image.
        /// </summary>
        /// <remarks>
        /// An overlay image is one that is not scaled according to the current zoom.
        /// This is ideal for backgrounds, foregrounds and uniform HUDs that do not
        /// track the camera.
        /// </remarks>
        /// <param name="image">Sprite to draw</param>
        /// <param name="tint">Color to tint sprite</param>
        /// <param name="pos">Position to draw in WINDOW COORDS</param>
        public void DrawOverlay(Texture2D image, Color tint, Vector2 pos) {
            // Enforce invariants.
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");
           
            // Rescale position to align
            pos = pos / Scale;
            Vector2 orig = new Vector2(image.Width / 2, image.Height / 2);
            Vector2 scale = new Vector2(1 / SX, 1 / SY); // To counter global scale

            // Draw this unscaled
            spriteBatch.Draw(image, pos, null, tint, 0.0f, orig, scale, SpriteEffects.None, 0);
        }

        public void DrawOverlay(Texture2D image, Color tint, Vector2 pos, Vector2 scale, float angle, int frame, int framesize, SpriteEffects effect)
        {
            // Enforce invariants.
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");

            // Pick out the right frame
            int width = image.Width / framesize;
            int height = image.Height;

            // Rescale position to align
            pos = pos / Scale;
            scale = new Vector2(scale.X / SX, scale.Y / SY); // To counter global scale

            // Compute frame position assuming only 1 row of frames.
            Rectangle src = new Rectangle(frame * width, 0, width, height);
            Vector2 origin = new Vector2(width / 2, height / 2);

            // Draw it.
            spriteBatch.Draw(image, pos, src, tint, angle, origin, scale, effect, 0);

        }

        /// <summary>
        /// Draw text to the screen.
        /// </summary>
        /// <remarks>
        /// Text is scaled by the global scale factor, just like any other image.
        /// Text is drawn with the built-in view font.
        /// </remarks>
        /// <param name="text">Text to draw</param>
        /// <param name="tint">Text color</param>
        /// <param name="position">Location to draw text</param>
        public void DrawText(String text, Color tint, Vector2 position) {
            // Enforce invariants.
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");
            spriteBatch.DrawString(font, text, position*Scale, tint);
        }

        /// <summary>
        /// Draw text to the screen.
        /// </summary>
        /// <remarks>
        /// Text is scaled by the global scale factor, just like any other image.
        /// </remarks>
        /// <param name="text">Text to draw</param>
        /// <param name="tint">Text color</param>
        /// <param name="position">Location to draw text</param>
        /// <param name="font">Alternate font to use</param>
        public void DrawText(String text, Color tint, Vector2 position, SpriteFont font) {
            // Enforce invariants.
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");
            spriteBatch.DrawString(font, text, position * Scale, tint);
        }

        public void DrawText(String text, Color tint, Vector2 position, bool fancy)
        {
            // Enforce invariants.
            //Vector2 pos = position / Scale;
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");
            if (fancy)
            {
                spriteBatch.DrawString(fancyFont, text, position, tint);
            }
            else spriteBatch.DrawString(font, text, position * Scale, tint);
        }

        /// <summary>
        /// End the sprite pass, flushing all graphics to the screen.
        /// </summary>
        public void EndSpritePass() {
            // Check the drawing state invariants.
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");
            state = DrawState.Inactive;
            spriteBatch.End();
        }

        #endregion

        #region Polygon Pass

        /// <summary>
        /// Start a new pass to draw polygons
        /// </summary>
        /// <remarks>
        /// Once a pass has begin, you cannot change attributes or draw sprites
        /// until it has ended.
        /// </remarks>
        public void BeginPolygonPass() {
            // Check that state invariant is satisfied.
            Debug.Assert(state == DrawState.Inactive, "Drawing state is invalid (expected Inactive)");
            state = DrawState.PolygonPass;
        }

        /// <summary>
        /// Draw the given textured polygon.
        /// </summary>
        /// <remarks>
        /// The polygon is scaled according to the view Scale attribute.
        /// </remarks>
        /// <param name="vertices">Vertices with texture mapping</param>
        /// <param name="texture">Texture to apply to polygon</param>
        public void DrawPolygons(VertexPositionTexture[] vertices, Texture2D texture) {
            DrawPolygons(vertices, texture, Vector2.Zero, 0.0f, 1.0f,BlendState.AlphaBlend,false,Vector2.Zero);
        }

        /// <summary>
        /// Draw the given textured polygon.
        /// </summary>
        /// <remarks>
        /// The polygon is scaled according to the view Scale attribute.
        /// </remarks>
        /// <param name="vertices">Vertices with texture mapping</param>
        /// <param name="texture">Texture to apply to polygon</param>
        /// <param name="position">Position to offset polygon</param>
        /// <param name="angle">Angle to rotate polygon</param>
        /// <param name="scale">Amount to scale polygon</param>
        public void DrawPolygons(VertexPositionTexture[] vertices, Texture2D texture,
                                 Vector2 position, float angle, float scale) {
            DrawPolygons(vertices, texture, position, angle, scale, BlendState.AlphaBlend,false,Vector2.Zero);
        }

        /// <summary>
        /// Draw the given textured polygon.
        /// </summary>
        /// <remarks>
        /// The polygon is scaled according to the view Scale attribute.
        /// </remarks>
        /// <param name="vertices">Vertices with texture mapping</param>
        /// <param name="texture">Texture to apply to polygon</param>
        /// <param name="position">Position to offset polygon</param>
        /// <param name="angle">Angle to rotate polygon</param>
        /// <param name="scale">Amount to scale polygon</param>
        /// <param name="blendMode">Blend mode to combine textures</param>
        public void DrawPolygons(VertexPositionTexture[] vertices, Texture2D texture,
                                 Vector2 position, float angle, float scale,
                                 BlendState blendMode, bool ai, Vector2 dragPos) {
            // Check the drawing state invariants.
            Debug.Assert(state == DrawState.PolygonPass, "Drawing state is invalid (expected PolygonPass)");

            if (angle == (float)Math.PI)
            {
                camera = new Camera(graphics.GraphicsDevice.Viewport, levelWidth, levelHeight, 10.0f);

                camera.BPos = position;

                // Create translation matrix
                effect.World = Matrix.CreateRotationZ(angle) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(new Vector3(position, 0)) * camera.GetFlippedBreathTransformation();
            }
            else if(!ai)
            {
                camera = new Camera(graphics.GraphicsDevice.Viewport, levelWidth, levelHeight, 10.0f);

                camera.BPos = position;

                // Create translation matrix
                effect.World = Matrix.CreateRotationZ(angle) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(new Vector3(position, 0)) * camera.GetBreathTransformation();
            }
            else if (ai)
            {
                Camera cam = new Camera(graphics.GraphicsDevice.Viewport, levelWidth, levelHeight, 10.0f);

                //cam.AIBPos = position;
                cam.AIBPos = dragPos;
                //Debug.Print("" + position);
                effect.World = Matrix.CreateTranslation(new Vector3(position,0));
                //effect.World = Matrix.CreateRotationZ(angle) * cam.GetAIBreathTransformation() * Matrix.CreateTranslation(new Vector3(dragPos, 0));
                //effect.World = Matrix.CreateRotationZ(angle) * Matrix.CreateScale(10.0f) * Matrix.CreateTranslation(new Vector3(dragPos, 0)); //* cam.GetBreathTransformation();
                //effect.World = Matrix.CreateScale(10.0f) * Matrix.CreateTranslation(new Vector3(levelWidth, levelHeight, 0)) * Matrix.CreateTranslation(new Vector3(position, 0));
                //Debug.Print("" + Matrix.CreateScale(10.0f) * Matrix.CreateTranslation(new Vector3(levelWidth, levelHeight, 0)) * Matrix.CreateTranslation(new Vector3(position, 0)));
            }

            effect.Texture = texture;

            // Prepare device for drawing.
            GraphicsDevice device = graphics.GraphicsDevice;
            device.BlendState = blendMode;

            SamplerState s = new SamplerState();
            s.AddressU = TextureAddressMode.Wrap;
            s.AddressV = TextureAddressMode.Wrap;
            device.SamplerStates[0] = s;

            // Draw the polygon
            foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                pass.Apply();
                device.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);
            }
        }

        /// <summary>
        /// End the polygon pass.
        /// </summary>
        public void EndPolygonPass() {
            // Check that state invariant is satisfied.
            Debug.Assert(state == DrawState.PolygonPass, "Drawing state is invalid (expected PolygonPass)");
            state = DrawState.Inactive;
        }

        #endregion


        #region Background Pass

        public void BeginBackgroundPass(BlendState blend, Vector2 position)
        {
            // Check that state invariant is satisfied.
            Debug.Assert(state == DrawState.Inactive, "Drawing state is invalid (expected Inactive)");
            state = DrawState.BackgroundPass;

            backcamera = new Camera(graphics.GraphicsDevice.Viewport, levelWidth, levelHeight, 1.0f);

            backcamera.Pos = position;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null,backcamera.GetTransformation2(0.1f));
            spriteBatch.Draw(background, -position * 2, new Rectangle(0, 0, background.Width * 100, background.Height * 10), Color.White);
            spriteBatch.Draw(f_background, -position * 3, new Rectangle(0, 0, background.Width * 100, background.Height * 10), Color.White);
 
        }

        public void EndBackgroundPass()
        {
            // Check the drawing state invariants.
            Debug.Assert(state == DrawState.BackgroundPass, "Drawing state is invalid (expected SpritePass)");
            state = DrawState.Inactive;
            spriteBatch.End();
        }

        #endregion

        #region HUD pass
        public void BeginArrowPass(Vector2 positionGate, float playerXCorr, float playerYCorr, int player_ID, float width, float levelWidth)
        {
            //draw the arrow
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null);
            if (player_ID == 0)
            {
                float distance = positionGate.X - playerXCorr;
                float distance2;
                //if gate is in front of player
                if (positionGate.X > playerXCorr)
                {

                    distance2 = (positionGate.X - width - playerXCorr);
                }
                else
                {
                    distance2 = (positionGate.X + width - playerXCorr);
                }
                if (Math.Abs(distance2) < Math.Abs(distance))
                {
                    distance = distance2;
                }
                if (levelWidth == 21000)
                {

                    if (distance > (width / 30))
                    {
                        spriteBatch.Draw(arrowTexture, new Vector2(1080, positionGate.Y * 10 - playerYCorr * 2), Color.White);
                    }
                    if (-distance > (width / 30))
                    {
                        spriteBatch.Draw(arrowTexture, new Vector2(65, positionGate.Y * 10 - playerYCorr * 2), null, Color.White, 0, new Vector2(arrowTexture.Width / 2, arrowTexture.Height / 2), 1, SpriteEffects.FlipHorizontally, 0);

                    }
                }
                else
                {
                    if (distance > (width / 10))
                    {
                        spriteBatch.Draw(arrowTexture, new Vector2(1080, positionGate.Y * 10 - playerYCorr * 2), Color.White);
                    }
                    if (-distance > (width / 10))
                    {
                        spriteBatch.Draw(arrowTexture, new Vector2(65, positionGate.Y * 10 - playerYCorr * 2), null, Color.White, 0, new Vector2(arrowTexture.Width / 2, arrowTexture.Height / 2), 1, SpriteEffects.FlipHorizontally, 0);

                    }

                }
            }
        }


        //Draw the hud and dragon head on the progress bar
        //Draw the hud and dragon head on the progress bar
        public void BeginHUDPassPlayer(Vector2 relativeDragonPosition, Vector2 positionDragon, Vector2 positionGate, int lapNum, int playerLap, float width)
        {
            Texture2D drawingTexture = dragonheadTexture;
            float drawRatio = width * 3 / progressTexture.Width;


            //if the dragon missed a gate, then the progress bar shouldn't move anymore
            if (relativeDragonPosition.X > positionGate.X)
            {
                if (playerLap > lapNum)
                {
                    gateMissed[0] = false;
                }
                else
                {
                    gateMissed[0] = true;
                }
            }
            //if relative position is smaller than the gate position
            else
            {
                //if we have previously missed a gate
                if (gateMissed[0] == true)
                {
                    //it is the same gate we have been keep making
                    if (positionGate.X == prevGates[0])
                    {
                        gateMissed[0] = true;
                    }
                    //it is not the same gate, so we need to update
                    else
                    {
                        gateMissed[0] = false;
                    }
                }
                //if we have not previously missed a gate
                else
                {
                    gateMissed[0] = false;
                }
            }

            if (gateMissed[0])
            {
                spriteBatch.Draw(drawingTexture, new Vector2(stoppedPosition[0], 50), Color.White);
            }
            else
            {
                if (playerLap > lapNum)
                {
                    spriteBatch.Draw(drawingTexture, new Vector2((relativeDragonPosition.X + (playerLap - 2) * width) / drawRatio + 140, 50), Color.White);
                    stoppedPosition[0] = (relativeDragonPosition.X + (playerLap - 1) * width) / drawRatio + 140;
                }
                else
                {
                    spriteBatch.Draw(drawingTexture, new Vector2((relativeDragonPosition.X + (playerLap - 1) * width) / drawRatio + 140, 50), Color.White);
                    stoppedPosition[0] = (relativeDragonPosition.X + (playerLap - 1) * width) / drawRatio + 140;
                }
            }


            prevGates[0] = positionGate.X;
        }

        //Draw the hud and dragon head on the progress bar
        public void BeginHUDPassAI(Vector2 relativeDragonPosition, Vector2 positionDragon, Vector2 positionGate, int lapNum, int playerLap, int d_id, float width, float levelWidth)
        {
            float drawRatio = width * 3 / progressTexture.Width;
            Texture2D drawingTexture = dragonheadTexture;
            if (d_id == 1)
            {
                drawingTexture = dragonheadTexture2;
            }
            else if (d_id == 2)
            {
                drawingTexture = dragonheadTexture3;
            }
            else
            {
                drawingTexture = dragonheadTexture4;
            }

            Debug.Print(levelWidth.ToString());
            if (levelWidth == 21000)
            {
                if (d_id == 1)
                {
                    spriteBatch.Draw(drawingTexture, new Vector2(140, 40), Color.White);
                }
                else
                {
                    spriteBatch.Draw(drawingTexture, new Vector2(140, 50), Color.White);
                }
            }

            else
            {
                //if the dragon missed a gate, then the progress bar shouldn't move anymore
                if (relativeDragonPosition.X > positionGate.X)
                {
                    if (playerLap > lapNum)
                    {
                        gateMissed[d_id] = false;
                    }
                    else
                    {
                        gateMissed[d_id] = true;
                    }
                }
                //if relative position is smaller than the gate position
                else
                {
                    //if we have previously missed a gate
                    if (gateMissed[d_id] == true)
                    {
                        //it is the same gate we have been keep making
                        if (positionGate.X == prevGates[d_id])
                        {
                            gateMissed[d_id] = true;
                        }
                        //it is not the same gate, so we need to update
                        else
                        {
                            gateMissed[d_id] = false;
                        }
                    }
                    //if we have not previously missed a gate
                    else
                    {
                        gateMissed[d_id] = false;
                    }
                }

                if (d_id == 1)
                {
                    if (gateMissed[d_id])
                    {
                        spriteBatch.Draw(drawingTexture, new Vector2(stoppedPosition[d_id], 40), Color.White);
                    }
                    else
                    {
                        if (playerLap > lapNum)
                        {
                            spriteBatch.Draw(drawingTexture, new Vector2((relativeDragonPosition.X + (playerLap - 2) * width) / drawRatio + 140, 40), Color.White);
                            stoppedPosition[d_id] = (relativeDragonPosition.X + (playerLap - 1) * width) / drawRatio + 140;
                        }
                        else
                        {
                            spriteBatch.Draw(drawingTexture, new Vector2((relativeDragonPosition.X + (playerLap - 1) * width) / drawRatio + 140, 40), Color.White);
                            stoppedPosition[d_id] = (relativeDragonPosition.X + (playerLap - 1) * width) / drawRatio + 140;
                        }
                    }
                }
                else
                {
                    if (gateMissed[d_id])
                    {
                        spriteBatch.Draw(drawingTexture, new Vector2(stoppedPosition[d_id], 50), Color.White);
                    }
                    else
                    {
                        if (playerLap > lapNum)
                        {
                            spriteBatch.Draw(drawingTexture, new Vector2((relativeDragonPosition.X + (playerLap - 2) * width) / drawRatio + 140, 50), Color.White);
                            stoppedPosition[d_id] = (relativeDragonPosition.X + (playerLap - 1) * width) / drawRatio + 140;
                        }
                        else
                        {
                            spriteBatch.Draw(drawingTexture, new Vector2((relativeDragonPosition.X + (playerLap - 1) * width) / drawRatio + 140, 50), Color.White);
                            stoppedPosition[d_id] = (relativeDragonPosition.X + (playerLap - 1) * width) / drawRatio + 140;
                        }
                    }

                }
            }
            prevGates[d_id] = positionGate.X;
        }


        public void BeginHUDPass2()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null);
            spriteBatch.Draw(progressTexture, new Vector2(150, 50), Color.White);
        }

        public void EndHUDPass()
        {
            spriteBatch.End();
        }

        // Draw dragon fire gauge
        public void BeginHUDPass3(float fireLevel)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null);
            spriteBatch.Draw(firebarTexture, new Vector2(50, 200), Color.White);
            spriteBatch.Draw(innerfirebarTexture, new Vector2(50+firebarTexture.Width*.15f, 200+firebarTexture.Height*(1-.97f*fireLevel-.015f)), null, Color.White, 0.0f, new Vector2(0,0), new Vector2(.7f,.97f*fireLevel),SpriteEffects.None,0);
        }

        #endregion

    #endregion


        private class Camera
        {
            private const float zoomUpperLimit = 1.5f;
            private const float zoomLowerLimit = .5f;

            private float _zoom;
            private Matrix _transform;
            private Vector2 _pos;
            private float _rotation;
            private int _viewportWidth;
            private int _viewportHeight;
            private int _worldWidth;
            private int _worldHeight;

            public Camera(Viewport viewport, int worldWidth,
                int worldHeight, float initialZoom)
            {
                _zoom = initialZoom;
                _rotation = 0.0f;
                _pos = Vector2.Zero;
                _viewportWidth = viewport.Width;
                _viewportHeight = viewport.Height;
                _worldWidth = worldWidth;
                _worldHeight = worldHeight;
            }

            #region Properties

            public float Zoom
            {
                get { return _zoom; }
                set
                {
                    _zoom = value;
                    if (_zoom < zoomLowerLimit)
                        _zoom = zoomLowerLimit;
                    if (_zoom > zoomUpperLimit)
                        _zoom = zoomUpperLimit;
                }
            }

            public float Rotation
            {
                get { return _rotation; }
                set { _rotation = value; }
            }

            public void Move(Vector2 amount)
            {
                _pos += amount;
            }

            public Vector2 Pos
            {
                get { return _pos; }
                set
                {
                    float leftBarrier = 0;/* (float)_viewportWidth *
                             .5f / _zoom;*/
                    float rightBarrier = _worldWidth; /* -
                            (float)_viewportWidth * .5f / _zoom;*/
                    float topBarrier = _worldHeight -
                            (float)_viewportHeight * .5f / _zoom;
                    float bottomBarrier = (float)_viewportHeight *
                            .5f / _zoom;
                    _pos = value;
                    if (_pos.X < leftBarrier)
                        _pos.X = leftBarrier;
                    if (_pos.X > rightBarrier)
                        _pos.X = rightBarrier;
                    if (_pos.Y > topBarrier)
                        _pos.Y = topBarrier;
                    if (_pos.Y < bottomBarrier)
                        _pos.Y = bottomBarrier;
                }
            }

            public Vector2 AIBPos
            {
                get { return _pos; }
                set { _pos = value; }
            }

            public Vector2 BPos
            {
                get { return _pos; }
                set
                {
                    float leftBarrier = 0;/* (float)_viewportWidth *
                             .5f / _zoom;*/
                    float rightBarrier = _worldWidth; /* -
                            (float)_viewportWidth * .5f / _zoom;*/
                    float topBarrier = _worldHeight -
                            (float)_viewportHeight * .5f / _zoom;
                    float bottomBarrier = (float)_viewportHeight *
                            .5f / _zoom;
                    _pos = value;
                    /*if (_pos.X <= leftBarrier)
                        _pos.X = leftBarrier + 5.0f;
                    if (_pos.X >= rightBarrier)
                        _pos.X = rightBarrier + 5.0f;*/
                    if (_pos.Y >= topBarrier +2.1f)
                        _pos.Y = topBarrier + 2.1f;
                    if (_pos.Y <= bottomBarrier +2.1f)
                        _pos.Y = bottomBarrier + 2.1f;
                }
            }

            #endregion

            public Matrix GetTransformation()
            {
                _transform =
                Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(_viewportWidth * 0.5f,
                    _viewportHeight * 0.5f, 0));

                return _transform;
            }

            public Matrix GetFlippedBreathTransformation()
            {
                _transform =
                Matrix.CreateTranslation(new Vector3(-_pos.X - 6.5f, -_pos.Y + 2.1f, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(_viewportWidth * 0.5f,
                    _viewportHeight * 0.5f, 0));

                return _transform;
            }

            public Matrix GetBreathTransformation()
            {
                _transform =
                Matrix.CreateTranslation(new Vector3(-_pos.X + 6.5f, -_pos.Y + 2.1f, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(_viewportWidth * 0.5f,
                    _viewportHeight * 0.5f, 0));

                return _transform;
            }

            public Matrix GetAIBreathTransformation()
            {
                _transform =
                Matrix.CreateTranslation(new Vector3(-_pos.X + 6.5f, -_pos.Y + 2.1f, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));

                return _transform;
            }

            public Matrix GetTransformation2(float paralax)
            {
                return
                Matrix.CreateTranslation(new Vector3(-_pos.X * paralax, -_pos.Y * paralax, 0)) *
                Matrix.CreateTranslation(new Vector3(-_viewportWidth * 0.5f,
                    -_viewportHeight * 0.5f, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(_viewportWidth * 0.5f,
                    _viewportHeight * 0.5f, 0));
            }



        }
    }
}
