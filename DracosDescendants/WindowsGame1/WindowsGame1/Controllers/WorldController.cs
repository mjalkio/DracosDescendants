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

namespace DracosD.Controllers
{
    class WorldController {
        
        #region Constants
        public const float DEFAULT_SCALE = 50.0f;
        // Dimensions of the game world
        private const float WIDTH = 50.0f;
        private const float HEIGHT = 20.0f;
        private const float GRAVITY = 8.0f;

        // Physics constants for initialization
        private const float BASIC_DENSITY = 0.0f;
        private const float BASIC_FRICTION = 0.1f;
        private const float BASIC_RESTITION = 0.1f;
        #endregion



        #region Graphics Resources
        // Textures for all the game objects
        /*private static Texture2D dragonTexture;
        private static Texture2D regularPlanetTexture;
        private static Texture2D gateTexture;
        private static Dictionary<string, Texture2D> graphicsDictionary; */
        private Texture2D regularPlanetTexture;

        /// <summary>
        /// Load images for this class
        /// </summary>
        /// <param name="content">Content manager to access pipeline</param>
        public  void LoadContent(ContentManager content)
        {
            // Earth tiles are unique in each world
            /*dragonTexture = content.Load<Texture2D>("rocket");
            graphicsDictionary.Add("player", dragonTexture);
            regularPlanetTexture = content.Load<Texture2D>("venus-no-background");
            graphicsDictionary.Add("regular", regularPlanetTexture);
            graphicsDictionary.Add("lava", regularPlanetTexture);
            graphicsDictionary.Add("gaseous", regularPlanetTexture);
            gateTexture = content.Load<Texture2D>("barrier");
            graphicsDictionary.Add("gate", gateTexture);*/
            regularPlanetTexture = content.Load<Texture2D>("earthtile");
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
        private static Vector2[] planetLocations = new Vector2[]
        {  };

        // Other game objects
        private static Vector2 rocketPos = new Vector2(2, 10);
        #endregion

        #region Fields
        // All the objects in the world
        private LevelController level;

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

        protected Dragon dragon;
        protected List<PlanetaryObject> planets = new List<PlanetaryObject>();


        public Dragon Dragon
        {
            get { return dragon; }
        }

        // Controller to move the dragon
        protected ForceController forceController;
        // Game specific player input

        protected PlayerInputController playerInput;
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
        public WorldController(Vector2 gravity, LevelController thisLevel,ContentManager content) :
            this(thisLevel.Dimensions,new Vector2(0,0), new Vector2(DEFAULT_SCALE,DEFAULT_SCALE)){
                playerInput = new PlayerInputController();
                currentGates = new Dictionary<Dragon, int>();
                level = thisLevel;
                LoadContent(content);
                PopulateLevel();

                succeeded = false;

                //level is populated so initialize and populate the current gates for each racer
                initializeGates(currentGates, level.Racers);

                // Attach the force controller to the rocket.
                forceController = new ForceController(dragon, planets);
                world.AddController(forceController); 

                world.ContactManager.BeginContact += ContactAdded;
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
            foreach(Dragon drag in racers){
                gates.Add(drag, 0);
            }
        }

        private void PopulateLevel() {
            //Create a bounding box around the level (for now, will add wraparound later)
            PhysicsObject obj;

            Vector2[] points = { new Vector2(0, 0), new Vector2(50, 0), new Vector2(50, .01f), new Vector2(0, .01f) };
            obj = new PolygonObject(regularPlanetTexture, points, Scale);
            obj.BodyType = BodyType.Static;
            obj.Density = BASIC_DENSITY;
            obj.Restitution = BASIC_RESTITION;
            //AddObject(obj);

            dragon = level.Racers[0];
            AddObject(dragon);

            foreach(PhysicsObject planet in level.Planets){
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

            /*TODO if the body is dragon and the other is the current gate hes on,
             then increment the gate hes on and change the texture of the gate using gate model
             to be the passed texture...maybe also add in later once all gates on the level have been passed then display a win screen*/
            foreach (Dragon drag in level.Racers)
            {
                Gate currGate = level.Gates[currentGates[drag]];
                //if the racer passes through the current gate hes on...
                if ((body1.UserData == dragon && body2.UserData == currGate) ||
                    (body1.UserData == currGate && body2.UserData == dragon))
                {
                    //If you pass the last gate, you win
                    if (currentGates[drag] == level.Gates.Count - 1)
                    {
                        Succeeded = true;
                    }
                    else
                    {
                        currentGates[drag]++;
                    }

                }
            }
            /*if ((body1.UserData == dragon && body2.UserData == goalDoor) ||
                (body1.UserData == goalDoor && body2.UserData == dragon))
            {
                Succeeded = true;
            }*/

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
            DrawState state = DrawState.Inactive;
            foreach (PhysicsObject obj in Objects)
            {
                //draw only the current gate and all other objects that are not gates
                if (!(obj is Gate) || (obj is Gate && level.Gates[currentGates[dragon]].Equals((Gate)obj)))
                {
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
                    view.BeginSpritePass(BlendState.AlphaBlend, dragon.Position);
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
        public void Update(float dt)
        {
            // Read input and assign actions to rocket
            playerInput.ReadInput();

            // Read from the input and add the force to the rocket model
            // But DO NOT apply the force yet (look at RocketObject.cs).
            float FY = playerInput.Vertical * dragon.Thrust;
            float FX = playerInput.Horizontal * dragon.Thrust;
            dragon.Force = new Vector2(FX, FY);
            Debug.Print("" + dragon.Position);
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


        #endregion

    }
}
