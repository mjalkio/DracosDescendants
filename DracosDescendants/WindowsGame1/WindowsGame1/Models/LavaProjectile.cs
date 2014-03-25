using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DracosD.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace DracosD.Models
{
    public class LavaProjectile : CircleObject
    {
        #region Fields
        // Access through property, not constant
        //private const float BULLET_SPEED = 10.0f;
        #endregion

        #region Properties (READ-ONLY)
        /*/// <summary>
        /// Speed of this bullet when fired.
        /// </summary>
        public float Speed
        {
            get { return BULLET_SPEED; }
        }*/
        #endregion

        #region Initialization
        /// <summary>
        /// Create a new bullet at the origin.
        /// </summary>
        /// <param name="texture">Object texture</param>
        public LavaProjectile(Texture2D texture) :
            base(texture) {
                Density = 1.0f;
                Friction = 0.0f;
                Restitution = 0.0f;
                //body.IsSensor = true;
            }

        /// <summary>
        /// Create a new bullet object
        /// </summary>
        /// <param name="texture">Object texture</param>
        /// <param name="pos">Location in world coordinates</param>
        public LavaProjectile(Texture2D texture, Vector2 pos) :
            base(texture, pos) { }

        /// <summary>
        /// Create a new bullet object
        /// </summary>
        /// <param name="texture">Object texture</param>
        /// <param name="pos">Location in world coordinates</param>
        /// <param name="radius">Radius in world coordinates</param>
        public LavaProjectile(Texture2D texture, Vector2 pos, float radius) : 
            base(texture, pos, radius) { }

        /// <summary>
        /// Creates the physics Body for this object, adding it to the world.
        /// </summary>
        /// <remarks>
        /// Override base to ignore gravity.
        /// </remarks>
        /// <param name="world">Farseer world that stores body</param>
        /// <returns><c>true</c> if object allocation succeeded</returns>
        public override bool ActivatePhysics(World world) {
            bool success = base.ActivatePhysics(world);
            //body.IsBullet = true;
            body.IsSensor = true;
            return success;
        }

        
    #endregion

    }
}
