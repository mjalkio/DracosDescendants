﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DracosD.Objects;
using Microsoft.Xna.Framework.Content;
using DracosD.Controllers;
using FarseerPhysics.Controllers;
using DracosD.Models;
using FarseerPhysics.Dynamics.Contacts;
using DracosD.Views;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Audio;


namespace DracosD.Controllers
{
    class WorldController
    {

        #region Constants
        public const float DEFAULT_SCALE = 10.0f;
        // Dimensions of the game world
        private const float WIDTH = 80.0f;
        private const float HEIGHT = 60.0f;
        private const float GRAVITY = 5.0f;

        // Physics constants for initialization
        private const float BASIC_DENSITY = 0.0f;
        private const float BASIC_FRICTION = 0.1f;
        private const float BASIC_RESTITION = 0.1f;

        private const int LAP_NUM_FRAMES = 6;
        private const float DEFAULT_DRAGON_THRUST = 2000.0f;
        private const float DEFAULT_DRAGON_TOPSPEED = 40.0f;
        private const float PLACE_DIFFERENCE_MODIFIER = 1.1f;
        #endregion

        #region Graphics Resources
        private Texture2D regularPlanetTexture;
        private Texture2D lavaPlanetTexture;
        private Texture2D lavaProjTexture;
        private Texture2D lap2Texture;
        private Texture2D lap3Texture;

        private Texture2D lap1Counter;
        private Texture2D lap2Counter;
        private Texture2D lap3Counter;

        private int lapAnimationFrame;
        private int lapAnimationDelay = 3;
        private int lapDelayCounter;
        private int lapHesitationDelay = 25;
        private int lapHesitationCounter;


        /// <summary>
        /// Load images for this class
        /// </summary>
        /// <param name="content">Content manager to access pipeline</param>
        public void LoadContent(ContentManager content)
        {
            regularPlanetTexture = content.Load<Texture2D>("earthtile");
            lavaPlanetTexture = content.Load<Texture2D>("lava planet");
            lavaProjTexture = content.Load<Texture2D>("lava projectile");
            lap2Texture = content.Load<Texture2D>("Lap 2 Filmstrip");
            lap3Texture = content.Load<Texture2D>("Final Lap FIlmstrip");

            lap1Counter = content.Load<Texture2D>("lap1counter");
            lap2Counter = content.Load<Texture2D>("lap2counter");
            lap3Counter = content.Load<Texture2D>("lap3counter");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public static void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #endregion

        #region Fields (Game Geography)
        // Locations of all the boxes
        private static Vector2[] planetLocations = new Vector2[] { };

        // Other game objects
        private static Vector2 rocketPos = new Vector2(2, 10);
        #endregion

        #region Fields
        // All the objects in the world
        private LevelController level;
        private bool tutorial = false;

        // lapNum used to count 
        private Dictionary<Dragon, int> lapNum;
        private Dictionary<Dragon,int> playerLap;

        private Dictionary<Dragon, int> placeCounter;

        private LinkedList<PhysicsObject> objects = new LinkedList<PhysicsObject>();

        // Queue for adding objects
        private Queue<PhysicsObject> addQueue = new Queue<PhysicsObject>();

        //Dictionary to keep track of which index (i.e. which gate) of the gate list a dragon racer is on
        private Dictionary<Dragon, int> currentGates;

        // The Box2D world
        protected World world;
        protected Vector4 bounds;
        protected Vector2 scale;

        // Have we won yet?
        private bool succeeded;
        private bool failed;

        private const int DRAW_LAP_DELAY = 200;
        private bool drawlap2 = false;
        private int drawlap2delay;
        private bool drawlap3 = false;
        private int drawlap3delay;

        //dictionary to check if last gate is passed
        private Dictionary<Dragon, bool> lastGate;

        //has the reset button been pressed?
        private bool toReset;

        protected Dragon[] dragons;
        protected List<PlanetaryObject> planets = new List<PlanetaryObject>();
        private Random rand;


        public Dragon[] Dragon
        {
            get { return dragons; }
        }

        public int FinishingPlace
        {
            get { return placeCounter[dragons[0]]; }
        }

        // Controller to move the dragon
        protected ForceController forceController;
        // Game specific player input

        protected PlayerInputController playerInput;

        protected AIController[] AIControllers;

        //the HUD controller
        protected HUDController hud;

        //Stupid sound triggers
        protected bool fireSound;
        protected bool planetGoBoom;
        protected bool thisDragIsOnFire;
        protected bool gateSound;

        #endregion

        #region Properties (Read-Write)
        /// <summary>
        /// Whether the player has succeeded at this world (e.g. finished the race)
        /// </summary>
        public bool Succeeded
        {
            get { return succeeded; }
            set { succeeded = value; }
        }

        public bool Reset
        {
            get { return toReset; }
            set { toReset = value; }
        }

        public LinkedList<PhysicsObject> Objects
        {
            get { return objects; }
        }

        /// <summary>
        /// Whether the player has failed at this world (e.g. died)
        /// </summary>
        public bool Failed
        {
            get { return failed; }
            set { failed = value; }
        }
        #endregion

        #region Properties (Read-Only)
        /// <summary>
        /// Reference to Farseer World for collision detection.
        /// </summary>
        public World World
        {
            get { return world; }
        }

        /// <summary>
        /// Bounds of this Farseer world. 
        /// </summary>
        /// <remarks>
        /// The bounds are a 4-vector with the left-hand side, top boundary,
        /// right-hand side, and bottom-boundary of the window.
        /// </remarks>
        public Vector4 Bounds
        {
            get { return bounds; }
        }

        /// <summary>
        /// Left-hand side of this world
        /// </summary>
        public float X
        {
            get { return bounds.X; }
        }

        /// <summary>
        /// Bottom edge of this world
        /// </summary>
        public float Y
        {
            get { return bounds.Y; }
        }

        /// <summary>
        /// Width of this world
        /// </summary>
        public float Width
        {
            get { return bounds.Z - bounds.X; }
        }

        /// <summary>
        /// Height of this world
        /// </summary>
        public float Height
        {
            get { return bounds.W - bounds.Y; }
        }

        /// <summary>
        /// Scale of this Farseer world.
        /// </summary>
        /// <remarks>
        /// The world scale and view scale should always agree.
        /// </remarks>
        public Vector2 Scale
        {
            get { return scale; }
        }

        /// <summary>
        /// X-coordinate of the world scale
        /// </summary>
        /// <remarks>
        /// The world scale and view scale should always agree.
        /// </remarks>
        public float SX
        {
            get { return scale.X; }
        }

        /// <summary>
        /// Y-coordinate of the world scale
        /// </summary>
        /// <remarks>
        /// The world scale and view scale should always agree.
        /// </remarks>
        public float SY
        {
            get { return scale.Y; }
        }

        public bool ShouldPlayFireSound
        {
            get { return fireSound; }
        }

        public bool ShouldPlayGasSound
        {
            get { return planetGoBoom; }
        }

        public bool ShouldPlayOnFireSound
        {
            get { return thisDragIsOnFire; }
        }

        public bool ShouldPlayGateSound
        {
            get { return gateSound; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Create a new game world.
        /// </summary>
        /// <remarks>
        /// The bounds are a 4-vector with the left-hand side, top boundary,
        /// right-hand side, and bottom-boundary of the window.
        /// </remarks>
        /// <param name="bounds">Object boundary for this world</param>
        /// <param name="gravity">Global gravity constant</param>
        public WorldController(Vector2 gravity, LevelController thisLevel, ContentManager content, PlayerInputController inputController) :
            this(thisLevel.Dimensions / DEFAULT_SCALE, new Vector2(0, 0), new Vector2(DEFAULT_SCALE, DEFAULT_SCALE))
        {
            playerInput = inputController;
            
            currentGates = new Dictionary<Dragon, int>();
            placeCounter = new Dictionary<Dragon, int>();
            dragons = new Dragon[4];
            AIControllers = new AIController[3];
            level = thisLevel;
            LoadContent(content);
            // Only the tutorial has a power level of OVER 9000!!!!!!!!!!!
            tutorial = level.Width > 9000;
            PopulateLevel();

            succeeded = false;

            //level is populated so initialize and populate the current gates for each racer
            initializeGates(currentGates, level.Racers);

            lapNum = new Dictionary<Dragon, int>();
            playerLap = new Dictionary<Dragon, int>();
            lastGate = new Dictionary<Dragon, bool>();
            // Attach the force controller to the dragons.
            foreach (Dragon drag in dragons)
            {
                lapNum[drag] = 1;
                playerLap[drag] = 1;
                lastGate[drag] = false;
                forceController = new ForceController(drag, planets, Width);
                world.AddController(forceController);
                placeCounter[drag] = 3;
            }

            world.ContactManager.BeginContact += ContactAdded;

            //ai 0,1
            for (int i = 1; i < dragons.Length; i++)
            {
                
                List<List<Vector2>> aiLists = new List<List<Vector2>>();
                foreach (List<Vector2> ais in level.AIs)
                {
                    List<Vector2> newAI = new List<Vector2>();
                    if (ais.Count != 0)
                    {
                        foreach (Vector2 v in ais)
                        {
                            newAI.Add(v / 10);
                        }
                    }
                    aiLists.Add(newAI);
                }
                AIControllers[i-1] = new AIController(dragons[i], level, currentGates, aiLists[i-1]);
            }

            //create a new HUD controller
            hud = new HUDController(level, dragons[0], currentGates);
        }


        /// <summary>
        /// Create a new game world.
        /// </summary>
        /// <remarks>
        /// The bounds are a 4-vector with the left-hand side, top boundary,
        /// right-hand side, and bottom-boundary of the window.
        /// </remarks>
        /// <param name="bounds">Object boundary for this world</param>
        /// <param name="gravity">Global gravity constant</param>
        /// <param name="scale">Global scaling attribute</param>
        protected WorldController(Vector4 bounds, Vector2 gravity, Vector2 scale)
        {
            world = new World(new Vector2(0, 0));
            this.bounds = bounds;
            this.scale = scale;
            rand = new Random();
            succeeded = failed = false;
        }

        /// <summary>
        /// This method takes in a list of dragon racers, and for each racer, it adds it to the map
        /// with the first gate as the starting gate
        /// </summary>
        /// <param name="gates">Dictionary of racers the index of the gate they are on</param>
        /// <param name="racers">List of all dragon racers in the level</param>
        private void initializeGates(Dictionary<Dragon, int> gates, List<Dragon> racers)
        {
            foreach (Dragon drag in racers)
            {
                gates.Add(drag, 0);
            }
        }

        private void PopulateLevel()
        {
            //Create a bounding box around the level 

            Vector2[] points = { new Vector2(0f, 0.0f), new Vector2(Width, 0f), new Vector2(Width+1.0f, 0.01f), new Vector2(0f, 0.01f) };
            //PhysicsObject obj = new BoxObject(regularPlanetTexture, new Vector2(0, 0), new Vector2(Width, .01f));
            PhysicsObject obj = new PolygonObject(regularPlanetTexture, points, Scale);
            obj.BodyType = BodyType.Static;
            obj.Density = BASIC_DENSITY;
            obj.Restitution = BASIC_RESTITION;
            AddObject(obj);

            Vector2[] points2 = { new Vector2(0f, Height), new Vector2(0f, Height-0.1f), new Vector2(Width, Height-.1f), new Vector2(Width + 1.0f, Height),};
            obj = new PolygonObject(regularPlanetTexture, points2, Scale);
            //obj = new BoxObject(regularPlanetTexture, new Vector2(Height, 0), new Vector2(Width, .01f));
            obj.BodyType = BodyType.Static;
            obj.Density = BASIC_DENSITY;
            obj.Restitution = BASIC_RESTITION;
            AddObject(obj);

            //obj = new FloatingText("\n\n\n\n\n\n\n\n RIP Bithor", 110,111);
            //AddObject(obj);


            for (int i = 0; i < level.Racers.Count; i++)
            {
                if (level.Racers[i] != null)
                {
                    dragons[i] = level.Racers[i];
                    AddObject(dragons[i]);
                }
            }

            level.Gates[level.Gates.Count - 1].AnimationFrame = 100;
            foreach (Gate gate in level.Gates)
            {
                AddObject(gate);
            }
            

            foreach (PlanetaryObject planet in level.Planets)
            {
                planets.Add(planet);
                AddObject(planet);
            }

            if (tutorial)
            {
                foreach (FloatingText t in level.TextList)
                {
                    AddObject(t);
                }

            }

        }

        /// <summary>
        /// Adds physics object in to the add queue.
        /// </summary>
        /// <remarks>
        /// Objects on the queue are added just before collision
        /// processing.  We do this to control object creation.
        /// </remarks>
        /// <param name="obj">Object to add</param>
        public void AddQueuedObject(PhysicsObject obj)
        {
            Debug.Assert(InBounds(obj), "Object is not in bounds");
            addQueue.Enqueue(obj);
        }

        /// <summary>
        /// Immediately adds the object to the physics world
        /// </summary>
        /// <param name="obj">Object to add</param>
        protected void AddObject(PhysicsObject obj)
        {
            Debug.Assert(InBounds(obj), "Object is not in bounds");
            objects.AddLast(obj);
            obj.ActivatePhysics(world);
        }

        /// <summary>
        /// Checks if an object is in in-bounds (for debugging purposes)
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True, if object is in world bounds</returns>
        public bool InBounds(PhysicsObject obj)
        {
            bool horiz = (bounds.X <= obj.X && obj.X <= bounds.Z);
            bool vert = (bounds.Y <= obj.Y && obj.Y <= bounds.W);
            return horiz && vert;
        }
        #endregion

        #region Game Loop

        /// <summary>
        /// Callback method for collisions
        /// </summary>
        /// <remarks>
        /// This method is called when we get a collision between two objects.  We use 
        /// this method to test if it is the "right" kind of collision.
        /// </remarks>
        /// <param name="contact">The two bodies that collided</param>
        /// <returns><c>true</c> if we want the collision recognized</returns>
        private bool ContactAdded(Contact contact)
        {
            Body body1 = contact.FixtureA.Body;
            Body body2 = contact.FixtureB.Body;

            //handle the racer to gate collisions
            foreach (Dragon drag in dragons)
            {
                if (currentGates[drag] < level.Gates.Count)
                {
                    Gate currGate = level.Gates[currentGates[drag]];
                    //if the racer passes through the current gate hes on...
                    if ((body1.UserData == drag && body2.UserData == currGate) ||
                        (body1.UserData == currGate && body2.UserData == drag))
                    {
                        if (drag == dragons[0])
                        {
                            level.Gates[(currentGates[drag] - 1 + level.Gates.Count) % level.Gates.Count].Frame = 0;
                            level.Gates[(currentGates[drag] + 1 + level.Gates.Count) % level.Gates.Count].Hit = false;
                            gateSound = true;
                        }

                        // Debug.Print("Dragon Gate: " + currentGates[drag] + "\nGate Count: " + level.Gates.Count);
                        
                        //CHECK FOR TUTORIAL - ONLY ONE LAP
                        //If you pass the last gate, you win
                        if (currentGates[drag] == level.Gates.Count - 1 && playerLap[drag] == 1 && tutorial)
                        {
                            if (drag == dragons[0] && !Failed)
                            {
                                Succeeded = true;
                            }
                        }

                        //If you pass the last gate, you win
                        if (currentGates[drag] == level.Gates.Count - 1 && playerLap[drag] == 3)
                        {
                            if (drag == dragons[0])
                            {
                                Succeeded = true;
                            }
                            currentGates[drag]++;
                        }
                        else if (currentGates[drag] == level.Gates.Count - 1)
                        {
                            lastGate[drag] = true;
                            currentGates[drag] = 0;
                        }
                        else
                        {
                            currentGates[drag]++;
                        }

                    }
                }
            }
            
            //handle the racer to onFire gas planet collision
            Dragon curr_drag = null;
            GaseousPlanet gp = null;
            if (body1.UserData is Dragon && body2.UserData is GaseousPlanet)
            {
                curr_drag = body1.UserData as Dragon;
                gp = body2.UserData as GaseousPlanet;

            }
            else if (body1.UserData is GaseousPlanet && body2.UserData is Dragon)
            {
                curr_drag = body2.UserData as Dragon;
                gp = body1.UserData as GaseousPlanet;
            }
            if (curr_drag != null && gp != null)
            {
                if (gp.OnFire)
                {
                    if (curr_drag == dragons[0] && curr_drag.CanBeOnFire)
                    {
                        thisDragIsOnFire = true;
                    }
                    curr_drag.Burn(false); //set the cooldown for the dragon to not be able to enter input
                }
            }

            LavaProjectile fireObject = null;
            if (body1.UserData is Dragon && body2.UserData is LavaProjectile)
            {
                curr_drag = body1.UserData as Dragon;
                fireObject = body2.UserData as LavaProjectile;

            }
            else if (body1.UserData is LavaProjectile && body2.UserData is Dragon)
            {
                curr_drag = body2.UserData as Dragon;
                fireObject = body1.UserData as LavaProjectile;
            }

            if (curr_drag != null && fireObject != null)
            {
                if (curr_drag == dragons[0] && curr_drag.CanBeOnFire)
                {
                    thisDragIsOnFire = true;
                }
                curr_drag.Burn(false); //set the cooldown for the dragon to not be able to enter input
            }

            LavaPlanet lavaObject = null;
            if (body1.UserData is Dragon && body2.UserData is LavaPlanet)
            {
                curr_drag = body1.UserData as Dragon;
                lavaObject = body2.UserData as LavaPlanet;
            } 
            else if (body1.UserData is LavaPlanet && body2.UserData is Dragon)
            {
                curr_drag = body2.UserData as Dragon;
                lavaObject = body1.UserData as LavaPlanet;
            }

            if (curr_drag != null && lavaObject != null)
            {
                if (curr_drag == dragons[0] && curr_drag.CanBeOnFire)
                {
                    thisDragIsOnFire = true;
                }
                curr_drag.Burn(false); //set the cooldown for the dragon to not be able to enter input
            }

            return true;
        }

        /// <summary>
        /// Draw the physics objects to the view
        /// </summary>
        /// <remarks>
        /// For simple worlds, this method is enough by itself.  It will need
        /// to be overriden if the world needs fancy backgrounds or the like.
        /// 
        /// The method draws all objects in the order that they were added.
        /// To keep from combining passes, it checks the draw type of each
        /// object, and switches the pass whenever necessary.
        /// </remarks>
        /// <param name="view">Drawing context</param>
        public virtual void Draw(GameView view)
        {
            DrawState state = DrawState.BackgroundPass;
            BeginPass(view, state);
            EndPass(view, state);
            state = DrawState.Inactive;

            // view.Scale = scale;
            foreach (PhysicsObject obj in Objects)
            {
                //draw only the current gate and all other objects that are not gates
                if (!(obj is Gate) || (obj is Gate && currentGates[dragons[0]] < level.Gates.Count && level.Gates[currentGates[dragons[0]]].Equals((Gate)obj))
                    || (/*(currentGates[dragons[0]]>0) &&*/ (obj is Gate && level.Gates[((currentGates[dragons[0]]-1+level.Gates.Count)%level.Gates.Count)].Equals((Gate)obj)))){
                    Dragon drawDrag = null;
                    drawDrag = obj as Dragon;
                    if (drawDrag != null)
                    {
                        if (drawDrag.IsBreathing)
                        {
                            EndPass(view, state);
                            state = DrawState.SpritePass;
                            BeginPass(view, state);
                            drawDrag.Breath.Draw(view);
                            //Debug.Print("here");
                        }
                    }
                    // Need to change the current drawing pass.
                    if (state != obj.DrawState)
                    {
                        EndPass(view, state);
                        state = obj.DrawState;
                        BeginPass(view, state);
                    }
                    obj.Draw(view);

                    if (obj is FloatingText)
                    {
                        FloatingText current = (FloatingText)obj;
                        if (current.Start < dragons[0].Position.X && current.End > dragons[0].Position.X)
                        {
                            EndPass(view, state);
                            BeginTextPass(view, state);
                            obj.Draw(view);
                        }
                    }

                    EndPass(view, state);
                    BeginPass(view, state);
                }

            }
            EndPass(view, state);

            view.BeginSpritePass(BlendState.AlphaBlend);
            switch (playerLap[dragons[0]])
            {
                case 1:
                    view.DrawOverlay(lap1Counter, Color.White, new Vector2(70, 70));
                    break;
                case 2:
                    view.DrawOverlay(lap2Counter, Color.White, new Vector2(70, 70));
                    break;
                case 3:
                    view.DrawOverlay(lap3Counter, Color.White, new Vector2(70, 70));
                    break;
                default:
                    view.DrawOverlay(lap3Counter, Color.White, new Vector2(70, 70));
                    break;
            }
            
            EndPass(view, state);

            //Lap overlay if necessary
            if (drawlap2)
            {
                Vector2 pos   = new Vector2(view.Width,view.Height)/2.0f;
                Vector2 scale = new Vector2(1, 1); // To counter global scale
                int lapFrameSize = (int)(lap2Texture.Width/LAP_NUM_FRAMES);
                view.BeginSpritePass(BlendState.AlphaBlend);
                view.DrawOverlay(lap2Texture, Color.White, pos, scale, 0.0f, lapAnimationFrame, LAP_NUM_FRAMES, SpriteEffects.None);
                view.EndSpritePass();
            }
            if (drawlap3)
            {
                Vector2 pos = new Vector2(view.Width, view.Height) / 2.0f;
                Vector2 scale = new Vector2(1, 1); // To counter global scale
                int lapFrameSize = (int)(lap2Texture.Width / LAP_NUM_FRAMES);
                view.BeginSpritePass(BlendState.AlphaBlend);
                view.DrawOverlay(lap3Texture, Color.White, pos, scale, 0.0f, lapAnimationFrame, LAP_NUM_FRAMES, SpriteEffects.None);
                view.EndSpritePass();
            }

            //draw the hud if the game is not finished
            if ((currentGates[dragons[0]] < level.Gates.Count) && (!Failed))
            {
                Gate goal0 = level.Gates[currentGates[dragons[0]]];
                Gate goal1 = level.Gates[0];
                Gate goal2 = level.Gates[0];
                Gate goal3 = level.Gates[0];
                if (currentGates[dragons[1]] < level.Gates.Count)
                {
                    goal1 = level.Gates[currentGates[dragons[1]]];
                }
                if (currentGates[dragons[2]] < level.Gates.Count)
                {
                    goal2 = level.Gates[currentGates[dragons[2]]];
                }
                if (currentGates[dragons[3]] < level.Gates.Count)
                {
                    goal3 = level.Gates[currentGates[dragons[3]]];
                }
                hud.Draw2(view);
                hud.Draw(view, dragons[1].Position, dragons[1].Position + new Vector2((lapNum[dragons[1]]) * Width, 0), lapNum[dragons[1]], playerLap[dragons[1]], goal1, 1, Width, level.Width);
                hud.Draw(view, dragons[2].Position, dragons[2].Position + new Vector2((lapNum[dragons[2]]) * Width, 0), lapNum[dragons[2]], playerLap[dragons[2]], goal2, 2, Width, level.Width);
                hud.Draw(view, dragons[3].Position, dragons[3].Position + new Vector2((lapNum[dragons[3]]) * Width, 0), lapNum[dragons[3]], playerLap[dragons[3]], goal3, 3, Width, level.Width);
                hud.Draw(view, dragons[0].Position, dragons[0].Position + new Vector2((lapNum[dragons[0]]) * Width, 0), lapNum[dragons[0]], playerLap[dragons[0]], goal0, 0, Width, level.Width);
            }
        }

        /// <summary>
        /// Helper method to begin a new drawing pass
        /// </summary>
        /// <param name="view">Drawing view</param>
        /// <param name="state">Pass to activate</param>
        private void BeginPass(GameView view, DrawState state)
        {
            switch (state)
            {
                case DrawState.PolygonPass:
                    view.BeginPolygonPass();
                    break;
                case DrawState.SpritePass:
                    view.BeginSpritePass(BlendState.AlphaBlend, dragons[0].Position);
                    break;
                case DrawState.BackgroundPass:
                    view.BeginBackgroundPass(BlendState.AlphaBlend, dragons[0].Position + new Vector2(lapNum[dragons[0]]*Width,0)); //begin drawing the background
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Helper method to begin a new drawing pass
        /// </summary>
        /// <param name="view">Drawing view</param>
        /// <param name="state">Pass to activate</param>
        private void BeginTextPass(GameView view, DrawState state)
        {
            switch (state)
            {
                case DrawState.PolygonPass:
                    view.BeginPolygonPass();
                    break;
                case DrawState.SpritePass:
                    view.BeginTextSpritePass(BlendState.AlphaBlend, dragons[0].Position);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Helper method to end a new drawing pass
        /// </summary>
        /// <param name="view">Drawing view</param>
        /// <param name="state">Pass to deactivate</param>
        private void EndPass(GameView view, DrawState state)
        {
            switch (state)
            {
                case DrawState.PolygonPass:
                    view.EndPolygonPass();
                    break;
                case DrawState.SpritePass:
                    view.EndSpritePass();
                    break;
                case DrawState.BackgroundPass:
                    view.EndBackgroundPass(); //begin drawing the background
                    break;
                default:
                    break;
            }
        }

        public bool PressedStart()
        {
            // Read input and assign actions to rocket
            playerInput.ReadInput();

            return playerInput.start;
        }

        public bool PressedDown()
        {
            playerInput.ReadInput();

            return playerInput.Down;
        }

        public bool PressedUp()
        {
            playerInput.ReadInput();

            return playerInput.Up;
        }

        /// <summary>
        /// The core update loop of any physics world.
        /// </summary>
        /// <remarks>
        /// As the update loop is game-specific, this loop is obviously incomplete.  
        /// This is the portion of the loop that processes physics, updates objects
        /// for one last time, and garbage collects objects.  It should be called
        /// last, or close-to-last, in any overriding method
        /// </remarks>
        /// <param name="dt">Timing values from parent loop</param>
        public void Update(float dt, GameTime gametime)
        {
            // Read input and assign actions to rocket
            playerInput.ReadInput();

            //reset the level when 'R' pressed
            if (playerInput.reset)
            {
                toReset = true;
            }

            else
            {
                thisDragIsOnFire = false;
                planetGoBoom = false;
                gateSound = false;

                foreach (Dragon speedracer in dragons)
                {
                    speedracer.Thrust = DEFAULT_DRAGON_THRUST;
                    speedracer.DampenThreshold = DEFAULT_DRAGON_TOPSPEED;
                }
                //Debug.Print("GreenThrust: " + dragons[1].Thrust + "\nGreenSpeed: " + dragons[1].DampenThreshold);
                updateThrusts();
                //Debug.Print("GreenThrust: " + dragons[1].Thrust + "\nGreenSpeed: " + dragons[1].DampenThreshold);
                //NEED TO EDIT THIS SO THAT EACH DRAGON FLAPS
                //if arrow key is pressed, then flap the dragon
                if (playerInput.Horizontal !=0 || playerInput.Vertical != 0) dragons[0].IsFlapping = true;
                else
                {
                    dragons[0].IsFlapping = false;
                }

            
                //control dragon breath
                fireSound = false;
                if (playerInput.Breathing)
                {
                    dragons[0].breathFire();
                    if (dragons[0].Breath != null)
                    {
                        dragons[0].Breath.ActivatePhysics(world);
                        fireSound = true;
                    }
                }
                else
                {
                    if (dragons[0].Breath != null)
                    {
                        dragons[0].Breath.DeactivatePhysics(world);
                    }
                    dragons[0].stopBreathing();
                }

                //Manage lap tracking and seamless wraparound
                foreach (PhysicsObject obje in objects)
                {
                    if (obje.Position.X > Width)
                    {
                        Vector2 currentPosition = obje.Position;
                        obje.X = currentPosition.X - Width;
                        obje.Y = currentPosition.Y;
                        if (obje is Dragon)
                        {
                            lapNum[(Dragon)obje]++;
                        }
                    }
                    if (obje.Position.X < 0)
                    {
                        Vector2 currentPosition = obje.Position;
                        obje.X = currentPosition.X + Width;
                        obje.Y = currentPosition.Y;
                        if (obje is Dragon)
                        {
                            lapNum[(Dragon)obje]--;
                        }
                    }


                    if (obje is Dragon)
                    {
                        Dragon drag = (Dragon)obje;
                        //Debug.Print("" + lastGate[drag]);
                        if (/*lapNum[drag] > playerLap[drag] && playerLap[drag] < 3 && */lastGate[drag])
                        {
                            playerLap[drag]++;
                            lastGate[drag] = false;
                            if (drag == dragons[0]) {
                                if(playerLap[dragons[0]] == 2 && !tutorial)
                                {
                                    drawlap2 = true;
                                    lapAnimationFrame = 0;
                                }
                                if(playerLap[dragons[0]] ==3){
                                    drawlap3 = true;
                                    lapAnimationFrame = 0;
                                }
                            }
                        }
                    }


                    if (obje is GaseousPlanet)
                    {
                        GaseousPlanet gp = (GaseousPlanet)obje;
                        if (gp.Burned)
                        {
                            gp.OnFire = true;
                            // ADD SHOCKWAVE
                            for (int i = 0; i < 8; i++)
                            {
                                createLavaProjectile(gp);
                            }
                        }
                        Vector2 p1 = gp.Position;
                        Vector2 p2 = new Vector2(gp.Position.X, gp.Position.Y + gp.Radius);
                        Vector2 p3 = new Vector2(gp.Position.X, gp.Position.Y - gp.Radius);
                        Vector2 p4 = new Vector2(gp.Position.X - gp.Radius, gp.Position.Y);
                        Vector2 p5 = new Vector2(gp.Position.X - (float)(gp.Radius / Math.Sqrt(2)), gp.Position.Y - (float)(gp.Radius / Math.Sqrt(2)));
                        Vector2 p6 = new Vector2(gp.Position.X - (float)(gp.Radius / Math.Sqrt(2)), gp.Position.Y + (float)(gp.Radius / Math.Sqrt(2)));
                        Vector2 p7 = new Vector2(gp.Position.X + (float)(gp.Radius / Math.Sqrt(2)), gp.Position.Y + (float)(gp.Radius / Math.Sqrt(2)));
                        Vector2 p8 = new Vector2(gp.Position.X + (float)(gp.Radius / Math.Sqrt(2)), gp.Position.Y - (float)(gp.Radius / Math.Sqrt(2)));
                        Vector2 p9 = new Vector2(gp.Position.X + gp.Radius, gp.Position.Y);
                        //if dragon is breathing, detect overlap with gas planet and breath..generalize this to all dragons breathing
                        foreach (Dragon dragon in dragons)
                        {
                            if (dragon.IsBreathing)
                            {
                                Fixture fix = dragon.Breath.Fix;
                                if ((fix.TestPoint(ref p1) || fix.TestPoint(ref p2) || fix.TestPoint(ref p3) || fix.TestPoint(ref p4) || fix.TestPoint(ref p5) || fix.TestPoint(ref p6) || fix.TestPoint(ref p7) || fix.TestPoint(ref p8) || fix.TestPoint(ref p9))
                                    && !gp.OnFire)
                                {
                                    planetGoBoom = true;
                                    gp.Torch(false);
                                    break;
                                }
                               
                                /*foreach (Fixture fix in dragon.Breath.Fixtures)
                                {
                                    if ((fix.TestPoint(ref p1) || fix.TestPoint(ref p2) || fix.TestPoint(ref p3) || fix.TestPoint(ref p4) || fix.TestPoint(ref p5) || fix.TestPoint(ref p6) || fix.TestPoint(ref p7) || fix.TestPoint(ref p8) || fix.TestPoint(ref p9))
                                        && !gp.OnFire)
                                    {
                                        gp.Torch(false);
                                        break;
                                    }
                                }*/


                            }
                        }
                    }
                    
                    //Light other dragons on fire
                    //TODO: GENERALIZE THIS SO ALL DRAGONS CAN BREATH
                    if (obje is Dragon)
                    {
                        Dragon gp = (Dragon)obje;
                        Vector2 p1 = gp.Position;
                        Vector2 p2 = new Vector2(gp.Position.X, gp.Position.Y + gp.Height/2);
                        Vector2 p3 = new Vector2(gp.Position.X, gp.Position.Y - gp.Height / 2);
                        Vector2 p4 = new Vector2(gp.Position.X - gp.Width/2, gp.Position.Y);
                        Vector2 p5 = new Vector2(gp.Position.X + gp.Width / 2, gp.Position.Y);
                        //if dragon is breathing, detect overlap with gas planet and breath..generalize this to all dragons breathing
                        //TODO: GENERALIZE THIS IF ALL DRAGONS ARE BREATHING
                        foreach (Dragon dragon in dragons)
                        {
                            if (dragon.IsBreathing && dragon.Id != gp.Id)
                            {
                                Fixture fix = dragon.Breath.Fix;
                                if ((fix.TestPoint(ref p1) || fix.TestPoint(ref p2) || fix.TestPoint(ref p3) || fix.TestPoint(ref p4) || fix.TestPoint(ref p5))
                                    && gp.CanMove)
                                {
                                    if (gp == dragons[0] && gp.CanBeOnFire)
                                    {
                                        thisDragIsOnFire = true;
                                    }
                                    gp.Burn(false);
                                    break;
                                }
                             
                                /*foreach (Fixture fix in dragon.Breath.Fixtures)
                                {
                                    if ((fix.TestPoint(ref p1) || fix.TestPoint(ref p2) || fix.TestPoint(ref p3) || fix.TestPoint(ref p4) || fix.TestPoint(ref p5))
                                        && gp.CanMove)
                                    {
                                        gp.Burn(false);
                                        break;
                                    }
                                }*/


                            }
                        }
                    }

                    if (obje is LavaPlanet)
                    {
                        LavaPlanet lavaplan = (LavaPlanet)obje;
                        if (((LavaPlanet)lavaplan).Fire)
                        {
                            createLavaProjectile((LavaPlanet)lavaplan);
                            lavaplan.Fire = false;
                        }
                    }
                }

            // Read from the input and add the force to the dragon model
            float distance = (float) Math.Sqrt(playerInput.Horizontal * playerInput.Horizontal + playerInput.Vertical * playerInput.Vertical);
            //To prevent division by zero
            if (distance == 0)
            {
                distance = 1;
            }

            Vector2 normalizedDirection = new Vector2(playerInput.Horizontal / distance, playerInput.Vertical / distance);
            dragons[0].Force = normalizedDirection *dragons[0].Thrust;
            //Vector2 dir = ai.GetAction(gametime, currentGates);
            //dragon.Force = dragon.Thrust * dir;
            // Read from the input and add the force to the rocket model
            // But DO NOT apply the force yet (look at RocketObject.cs).
            //float FY = playerInput.Vertical * dragons[0].Thrust;
            //float FX = playerInput.Horizontal * dragons[0].Thrust;
            //float FY = dir.Y * dragon.Thrust;
            //float FX = dir.X * dragon.Thrust;
            //ragons[0].Force = new Vector2(FX, FY);


            for (int i = 1; i < dragons.Length; i++)
            {
                Vector2 dir;
                //if (i == 1 || i==2)
                //{
                    dir = AIControllers[i - 1].GetAction(gametime, currentGates, true);
                    //if (i == 1 && dragons[i].Position.X > 600) Debug.Print("" + dragons[i].Position);
                    //control dragon breath
                    if (AIControllers[i - 1].shouldBreathFire(dragons))
                    {
                        dragons[i].breathFire();
                        if (dragons[i].Breath != null)
                        {
                            dragons[i].Breath.ActivatePhysics(world);
                        }
                    }
                    else
                    {
                        if (dragons[i].Breath != null)
                        {
                            dragons[i].Breath.DeactivatePhysics(world);
                        }
                        dragons[i].stopBreathing();
                    }
                //}
                /*else
                {
                    if (!tutorial)
                    {
                        dir = AIControllers[i - 1].GetAction(gametime, currentGates, false);
                    }
                    else
                    {
                        dir = Vector2.Zero;
                    }
                }*/
                //make them complete a lap
                    //Debug.Print("" + (level.Width - 50) / 10);
                if ((i == 1 || i==2) && dragons[i].Position.X >= (level.Width - 50)/10)
                {
                    dir = Vector2.Normalize(new Vector2(1.0f,0.0f));
                }
                if ((i == 3) && dragons[i].Position.X >= (level.Width - 25) / 10)
                {
                    //Debug.Print("here");
                    dir = Vector2.Normalize(new Vector2(1.0f, 0.0f));
                }
                if (i == 3 && tutorial)
                {
                    dir = Vector2.Zero;
                }
                dragons[i].Force = dragons[i].Thrust * dir;
                if (dir.X != 0 || dir.Y != 0) dragons[i].IsFlapping = true;
                else dragons[i].IsFlapping = false;
            }

            //DONE: MAKE THIS HAPPEN FOR ALL DRAGONS, NOT JUST PLAYER
            foreach (Dragon drag in dragons)
            {
                if (!drag.CanMove)
                {
                    drag.Force = new Vector2(0f, 0f);
                }

                // Also this handles logic for place detection
                int whichPlace = 1;
                foreach (Dragon enemydrag in dragons)
                {
                    if (enemydrag != drag)
                    {
                        if(placeCompare(enemydrag,drag)) whichPlace++;
                    }
                    if (!succeeded)
                    {
                        placeCounter[drag] = whichPlace;
                    }
                }

            }
            

            if (drawlap2) drawlap2delay++;
            if (drawlap2delay > DRAW_LAP_DELAY) drawlap2 = false;
            if (drawlap3) drawlap3delay++;
            if (drawlap3delay > DRAW_LAP_DELAY) drawlap3 = false;

            // animation updates if necessary
            level.Gates[(currentGates[dragons[0]] - 1 + level.Gates.Count) % level.Gates.Count].incrementFrame();

            
            if (lapAnimationFrame != 3)
            {
                lapDelayCounter++;
                if (lapDelayCounter == lapAnimationDelay)
                {
                    lapAnimationFrame++;
                    lapDelayCounter = 0;
                }
            }
            else
            {
                lapHesitationCounter++;
                if (lapHesitationCounter == lapHesitationDelay)
                {
                    lapDelayCounter++;
                    if (lapDelayCounter == lapAnimationDelay)
                    {
                        lapAnimationFrame++;
                        lapDelayCounter = 0;
                    }
                    lapHesitationCounter = 0;
                }
            }
            


            // Add any objects created by actions
            foreach (PhysicsObject o in addQueue)
            {
                AddObject(o);
            }
            addQueue.Clear();

            // Turn the physics engine crank.
            world.Step(dt);

            // Garbage collect the deleted objects.
            // Note how we use the linked list nodes to delete O(1) in place.
            // This is O(n) without copying.  
            LinkedListNode<PhysicsObject> node = objects.First;
            LinkedListNode<PhysicsObject> next;
            PhysicsObject obj;
            while (node != null)
            {
                obj = node.Value;
                next = node.Next;
                // Delete O(1) in place
                if (obj.Remove)
                {
                    obj.DeactivatePhysics(world);
                    objects.Remove(node);
                }
                else
                {
                    // Note that update is called last!
                    obj.Update(dt);
                }
                node = next;
            }
        }
    }


        #endregion

        #region Methods

        private void createLavaProjectile(PlanetaryObject planet)
        {
            const float BULLET_OFFSET = 0.5f;

            float radius = planet.Radius / 4.0f;
            LavaProjectile bullet = new LavaProjectile(lavaProjTexture, planet.Position, radius);
            bullet.Density = .5f;

            // Compute position and velocity
            float offset = (lavaProjTexture.Width + BULLET_OFFSET);
            float speed = rand.Next(7,13);
            float randomDirection = (float)(rand.NextDouble() * Math.PI * 2.0);
            float randomDirection2 = (float)(rand.NextDouble() * Math.PI * 2.0);
            Vector2 randomDirection2Vec = new Vector2((float)Math.Cos(randomDirection2), (float)Math.Sin(randomDirection2));
            randomDirection2Vec.Normalize();

            Vector2 bulletDirection =  new Vector2((float)Math.Cos(randomDirection), (float)Math.Sin(randomDirection));
            bulletDirection.Normalize();

            bullet.Position = planet.Position + bulletDirection;
            bullet.LinearVelocity = (speed*bulletDirection) + randomDirection2Vec;
            AddQueuedObject(bullet);
        }

        // Returns true if drag1 is ahead of drag2, false if the converse
        private bool placeCompare(Dragon drag1, Dragon drag2)
        {
            if (playerLap[drag1] != playerLap[drag2]) return playerLap[drag1]>playerLap[drag2];
            else if (currentGates[drag1] != currentGates[drag2])
            {
                return currentGates[drag1]>currentGates[drag2];
            }
            else return (drag1.Position.X > drag2.Position.X);
        }

        private void updateThrusts()
        {
            for (int i = 1; i <= 3; i++)
            {
                int placeDifference = placeCounter[dragons[i]] - placeCounter[dragons[0]];
                float xdifference = (playerLap[dragons[0]] * Width + dragons[0].Position.X) -
                    (playerLap[dragons[i]] * Width + dragons[i].Position.X);


                float differenceModifier = (xdifference / (Width * 3)) + 1.0f;


                differenceModifier += (float)Math.Pow(PLACE_DIFFERENCE_MODIFIER, placeDifference) - 1;
                if (differenceModifier > 1.3f) differenceModifier = 1.3f;
                if (differenceModifier < 1.0f / 1.3f) differenceModifier = 1.0f / 1.3f;
                dragons[i].Thrust = DEFAULT_DRAGON_THRUST * (float)Math.Pow(differenceModifier,.5);
                dragons[i].DampenThreshold = DEFAULT_DRAGON_TOPSPEED * differenceModifier;
            }
        }


        #endregion
    }
}
