#region File Description
//-----------------------------------------------------------------------------
// Dragon.cs
//
// A model which represents one of the dragon racers in Draco's Descendents.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using DracosD.Objects;
using DracosD.Views;
#endregion

namespace DracosD.Models
{
    class Dragon : BoxObject 
    {
        #region Constants
        // Default physics values
        private const float DEFAULT_DENSITY = 1.0f;
        private const float DEFAULT_FRICTION = 0.1f;
        private const float DEFAULT_RESTITUTION = 0.4f;

        // Thrust amount to convert player input into thrust
        private const float DEFAULT_THRUST = 30.0f;
        #endregion

        #region Fields
        private Vector2 force;
        private bool isOnFire;
        #endregion

        #region Properties (READ-WRITE)
        /// <summary>
        /// The dragon force, as determined by the input controller
        /// </summary>
        public Vector2 Force
        {
            get { return force; }
            set { force = value; }
        }

        /// <summary>
        /// Whether the dragon is currently on fire
        /// </summary>
        public bool IsOnFire
        {
            get { return isOnFire; }
            set { isOnFire = value; }
        }
        #endregion

        #region Properties (READ-ONLY)
        /// <summary>
        /// The amount of thrust this rocket has.  Multiply this by input to give force.
        /// </summary>
        public float Thrust
        {
            get { return DEFAULT_THRUST; }
        }
        #endregion

        #region Initialization
        public Dragon(Texture2D texture, Vector2 pos) : 
            base(texture, pos, new Vector2((float)texture.Width, (float)texture.Height)) 
        {
            BodyType = BodyType.Dynamic;
            Density  = DEFAULT_DENSITY;
            Friction = DEFAULT_FRICTION;
            Restitution = DEFAULT_RESTITUTION;
            isOnFire = false;
        }

        /// <summary>
        /// Creates the physics Body for this object, adding it to the world.
        /// </summary>
        /// <remarks>
        /// Override base method to prevent spinning.
        /// </remarks>
        /// <param name="world">Farseer world that stores body</param>
        /// <returns><c>true</c> if object allocation succeeded</returns>
        public override bool ActivatePhysics(World world)
        {
            // Get the box body from our parent class
            return base.ActivatePhysics(world);
        }
        #endregion

        #region Game Loop (Update and Draw)
        /// <summary>
        /// Updates the object AFTER collisions are resolved. Primarily for animation.
        /// </summary>
        /// <param name="dt">Timing values from parent loop</param>
        public override void Update(float dt) {
            base.Update(dt);
        }

        /// <summary>
        /// Draws the physics object.
        /// </summary>
        /// <param name="view">Drawing context</param>
        public override void Draw(GameView view)
        {
            view.DrawSprite(texture, Color.White, Position, scale, Rotation);
        }
        #endregion
    }
}
