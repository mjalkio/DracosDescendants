using System;
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

namespace DracosD.Controllers
{
    class WorldController
    {

        #region Constants
        public const float DEFAULT_SCALE = 10.0f;
        // Dimensions of the game world
        private const float WIDTH = 80.0f;
        private const float HEIGHT = 60.0f;
        private const float GRAVITY = 6.0f;

        // Physics constants for initialization
        private const float BASIC_DENSITY = 0.0f;
        private const float BASIC_FRICTION = 0.1f;
        private const float BASIC_RESTITION = 0.1f;
        #endregion

        #region Graphics Resources
        private Texture2D regularPlanetTexture;
        private Texture2D lavaPlanetTexture;
        private Texture2D lavaProjTexture;

        /// <summary>
        /// Load images for this class
        /// </summary>
        /// <param name="content">Content manager to access pipeline</param>
        public void LoadContent(ContentManager content)
        {
            regularPlanetTexture = content.Load<Texture2D>("earthtile");
            lavaPlanetTexture = content.Load<Texture2D>("lava planet");
            lavaProjTexture = content.Load<Texture2D>("lava projectile");
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
        private int lapNum;
        private int playerLap;

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

        //has the reset button been pressed?
        private bool toReset;

        protected Dragon[] dragons;
        protected List<PlanetaryObject> planets = new List<PlanetaryObject>();
        private Random rand;


        public Dragon[] Dragon
        {
            get { return dragons; }
        }

        // Controller to move the dragon
        protected ForceController forceController;
        // Game specific player input

        protected PlayerInputController playerInput;

        protected AIController[] AIControllers;
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

        public bool isToReset
        {
            get { return toReset; }
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
        public WorldController(Vector2 gravity, LevelController thisLevel, ContentManager content) :
            this(thisLevel.Dimensions / DEFAULT_SCALE, new Vector2(0, 0), new Vector2(DEFAULT_SCALE, DEFAULT_SCALE))
        {
            playerInput = new PlayerInputController();
            
            currentGates = new Dictionary<Dragon, int>();
            dragons = new Dragon[4];
            AIControllers = new AIController[3];
            level = thisLevel;
            LoadContent(content);
            PopulateLevel();

            lapNum = 1;

            succeeded = false;

            //level is populated so initialize and populate the current gates for each racer
            initializeGates(currentGates, level.Racers);

            // Attach the force controller to the dragons.
            foreach (Dragon drag in dragons)
            {
                forceController = new ForceController(drag, planets, Width);
                world.AddController(forceController);
            }

            world.ContactManager.BeginContact += ContactAdded;

            for (int i = 1; i < dragons.Length; i++)
            {
                AIControllers[i-1] = new AIController(dragons[i], level, currentGates);
            }
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
            //Create a bounding box around the level (for now, will add wraparound later)
            /*PhysicsObject obj;

            Vector2[] points = { new Vector2(0, 0), new Vector2(50, 0), new Vector2(50, .01f), new Vector2(0, .01f) };
            obj = new PolygonObject(regularPlanetTexture, points, Scale);
            obj.BodyType = BodyType.Static;
            obj.Density = BASIC_DENSITY;
            obj.Restitution = BASIC_RESTITION;
            //AddObject(obj);*/
            Vector2[] points = { new Vector2(0f, 0.0f), new Vector2(Width, 0f), new Vector2(Width+1.0f, 0.01f), new Vector2(0f, 0.01f) };
            PhysicsObject obj = new PolygonObject(regularPlanetTexture, points, Scale);
            obj.BodyType = BodyType.Static;
            obj.Density = BASIC_DENSITY;
            obj.Restitution = BASIC_RESTITION;
            AddObject(obj);

            Vector2[] points2 = { new Vector2(0f, Height), new Vector2(0f, Height-0.1f), new Vector2(Width, Height-.1f), new Vector2(Width + 1.0f, Height),};
            obj = new PolygonObject(regularPlanetTexture, points2, Scale);
            obj.BodyType = BodyType.Static;
            obj.Density = BASIC_DENSITY;
            obj.Restitution = BASIC_RESTITION;
            AddObject(obj);

            Debug.Print("COUNT OF LEVEL: " + level.Racers.Count);
            Debug.Print("COUNT OF DRAGONS: " + dragons.Length);
            for (int i = 0; i < level.Racers.Count; i++)
            {
                Debug.Print("COUNT: " + i);
                if (level.Racers[i] != null)
                {
                    dragons[i] = level.Racers[i];
                    AddObject(dragons[i]);
                }
            }

            foreach (Gate gate in level.Gates)
            {
                AddObject(gate);
            }

            foreach (PlanetaryObject planet in level.Planets)
            {
                planets.Add(planet);
                AddObject(planet);
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
                        //If you pass the last gate, you win
                        if (currentGates[drag] == level.Gates.Count - 1 && playerLap == 3)
                        {
                            Succeeded = true;
                            currentGates[drag]++;
                        }
                        else if (currentGates[drag] == level.Gates.Count - 1)
                        {
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
            /*Debug.Print("VIEW: " + view.LevelWidth);
            Debug.Print("WORLD: " + Width);
            Debug.Print("" + Dragon.Position);
            Debug.Print("Dragon: " + dragon.X + ", " + dragon.Y);*/

            // view.Scale = scale;
            foreach (PhysicsObject obj in Objects)
            {
                //draw only the current gate and all other objects that are not gates
                if (!(obj is Gate) || (obj is Gate && currentGates[dragons[0]] < level.Gates.Count && level.Gates[currentGates[dragons[0]]].Equals((Gate)obj)))
                {
                    Dragon drawDrag = null;
                    drawDrag = obj as Dragon;
                    if (drawDrag != null)
                    {
                        if (drawDrag.IsBreathing)
                        {
                            EndPass(view, state);
                            state = DrawState.PolygonPass;
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
                }

            }
            EndPass(view, state);

            BeginTextPass(view, state);
            view.DrawText("Lap: " + playerLap, Color.White, new Vector2(0.0f, 0.0f));
            EndPass(view, state);
            /*state = DrawState.SpritePass;
            BeginTextPass(view, state);
            

            view.DrawText("Q/A Restitution: " + planets[0].Restitution, Color.White, new Vector2(0.0f, 0.0f));
            view.DrawText("W/S Gravity Constant: " + forceController.Gravity, Color.White, new Vector2(0.0f, 0.5f));
            view.DrawText("E/D Dragon Thrust: " + dragon.Thrust, Color.White, new Vector2(0.0f, 1.0f));
            view.DrawText("R/F Dampening Factor: " + dragon.Dampen, Color.White, new Vector2(0.0f, 1.5f));
            view.DrawText("T/G Top Speed: " + dragon.DampenThreshold, Color.White, new Vector2(0.0f, 2.0f));
            view.DrawText("Current Speed: " + dragon.LinearVelocity.Length(), Color.White, new Vector2(0.0f, 2.5f));
            EndPass(view, state);*/
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
                    view.BeginBackgroundPass(BlendState.AlphaBlend, dragons[0].Position + new Vector2((lapNum)*Width,0)); //begin drawing the background
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
            // Debug.Print("" + dragon.Thrust);
            
            // Read input and assign actions to rocket
            playerInput.ReadInput();

            //reset the level when 'R' pressed
            if (playerInput.reset)
            {
                toReset = true;
            }

            else
            {
                //NEED TO EDIT THIS SO THAT EACH DRAGON FLAPS
                //if arrow key is pressed, then flap the dragon
                if (playerInput.Horizontal !=0 || playerInput.Vertical != 0) dragons[0].IsFlapping = true;
                else
                {
                    dragons[0].IsFlapping = false;
                }

            
                //control dragon breath
                if (playerInput.Breathing)
                {
                    dragons[0].breathFire();
                    dragons[0].Breath.ActivatePhysics(world);
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
                        if (obje == dragons[0])
                        {
                            lapNum++;
                        }
                    }
                    if (obje.Position.X < 0)
                    {
                        Vector2 currentPosition = obje.Position;
                        obje.X = currentPosition.X + Width;
                        obje.Y = currentPosition.Y;
                        if (obje == dragons[0])
                        {
                            lapNum--;
                        }
                    }
                

                    if (lapNum > playerLap && playerLap < 3) playerLap = lapNum;


                    if (obje is GaseousPlanet)
                    {
                        GaseousPlanet gp = (GaseousPlanet)obje;
                        if (gp.Burned)
                        {
                            gp.OnFire = true;
                        }
                        Vector2 p1 = gp.Position;
                        Vector2 p2 = new Vector2(gp.Position.X, gp.Position.Y + gp.Radius);
                        Vector2 p3 = new Vector2(gp.Position.X, gp.Position.Y - gp.Radius);
                        Vector2 p4 = new Vector2(gp.Position.X - gp.Radius, gp.Position.Y);
                        Vector2 p5 = new Vector2(gp.Position.X - (float)(gp.Radius / Math.Sqrt(2)), gp.Position.Y - (float)(gp.Radius / Math.Sqrt(2)));
                        Vector2 p6 = new Vector2(gp.Position.X - (float)(gp.Radius / Math.Sqrt(2)), gp.Position.Y + (float)(gp.Radius / Math.Sqrt(2)));
                        //if dragon is breathing, detect overlap with gas planet and breath..generalize this to all dragons breathing
                        if (dragons[0].IsBreathing)
                        {
                            foreach (Fixture fix in dragons[0].Breath.Fixtures)
                            {
                                if ((fix.TestPoint(ref p1) || fix.TestPoint(ref p2) || fix.TestPoint(ref p3) || fix.TestPoint(ref p4) || fix.TestPoint(ref p5) || fix.TestPoint(ref p6))
                                    && !gp.OnFire)
                                {
                                    gp.Torch(false);
                                    break;
                                }
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
                Vector2 dir = AIControllers[i - 1].GetAction(gametime, currentGates);
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
            }


            //Debug.Print("" + dragon.Position);
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

        private void createLavaProjectile(LavaPlanet planet)
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
        #endregion
    }
}
