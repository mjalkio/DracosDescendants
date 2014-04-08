using System;
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
        protected Texture2D m_background;
        protected Texture2D f_background;

        // Track the current drawing pass. 
        protected DrawState state;

        // For onscreen messages
        protected SpriteFont font;

        // Private variable for property IsFullscreen.
        protected bool fullscreen;

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
            //load background
            background = content.Load<Texture2D>("stars");
            m_background = content.Load<Texture2D>("stars-parallax middle");
            f_background = content.Load<Texture2D>("stars-parallax front");
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

            if (position.X < 50.0f)
            {
                //Debug.Print("HERE IN THE FLESH");
                spriteBatch.Draw(image, new Vector2(position.X+(float)levelWidth,position.Y), null, tint, angle, origin, scale, SpriteEffects.None, 0);
            }

            if (position.X > levelWidth-50.0f)
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
        public void DrawSprite(Texture2D image, Color tint, Vector2 position, Vector2 scale, float angle, SpriteEffects effects) {
            // Enforce invariants.
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");

            // Get the texture center.
            Vector2 origin = new Vector2(image.Width / 2, image.Height / 2);

            // Draw it.
            spriteBatch.Draw(image, position, null, tint, angle, origin, scale, effects, 0);
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
        public void DrawSprite(Texture2D image, Color tint, Vector2 position, Vector2 scale, float angle, int frame, int framesize) {
            // Enforce invariants.
            Debug.Assert(state == DrawState.SpritePass, "Drawing state is invalid (expected SpritePass)");

            // Pick out the right frame
            int width = image.Width / framesize;
            int height = image.Height;

            // Compute frame position assuming only 1 row of frames.
            Rectangle src  = new Rectangle(frame * width, 0, width, height);
            Vector2 origin = new Vector2(width / 2, height / 2);

            // Draw it.
            spriteBatch.Draw(image, position, src, tint, angle, origin, scale, SpriteEffects.None, 0);
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
            DrawPolygons(vertices, texture, Vector2.Zero, 0.0f, 1.0f,BlendState.AlphaBlend);
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
            DrawPolygons(vertices, texture, position, angle, scale, BlendState.AlphaBlend);
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
                                 BlendState blendMode) {
            // Check the drawing state invariants.
            Debug.Assert(state == DrawState.PolygonPass, "Drawing state is invalid (expected PolygonPass)");

            camera = new Camera(graphics.GraphicsDevice.Viewport, levelWidth, levelHeight, 10.0f);

            camera.BPos = position;

            // Create translation matrix
            effect.World = Matrix.CreateRotationZ(angle) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(new Vector3(position, 0)) * camera.GetBreathTransformation();
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
            spriteBatch.Draw(background, -position/4, Color.White);
            spriteBatch.Draw(m_background, -position/3, Color.White);
            spriteBatch.Draw(f_background, -position/2, Color.White);
 
        }

        public void EndBackgroundPass()
        {
            // Check the drawing state invariants.
            Debug.Assert(state == DrawState.BackgroundPass, "Drawing state is invalid (expected SpritePass)");
            state = DrawState.Inactive;
            spriteBatch.End();
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
                    if (_pos.Y >= topBarrier +1.7f)
                        _pos.Y = topBarrier + 1.7f;
                    if (_pos.Y <= bottomBarrier +1.7f)
                        _pos.Y = bottomBarrier + 1.7f;
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

            public Matrix GetBreathTransformation()
            {
                _transform =
                Matrix.CreateTranslation(new Vector3(-_pos.X + 4.8f, -_pos.Y + 1.7f, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(_viewportWidth * 0.5f,
                    _viewportHeight * 0.5f, 0));

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
