#region File Description
//-----------------------------------------------------------------------------
// SensorObject.cs
//
// Box-shaped model to act as a sensor
//
// A sensor is an object that notifies you about collisions, but which does 
// not prevent the actual collision.  This is to be used to test if the
// player goes into an area, or is touching something.
//
// Author: Walker M. White
// Based on original PhysicsDemo Lab by Don Holden, 2007
// MonoGame version, 2/14/2014
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

/// <summary>
/// Subnamespace for Farseer-aware models.
/// </summary>
namespace DracosD.Objects {

    /// <summary>
    /// A box-shaped sensor to detect, but not repel collisions.
    /// </summary>
    public class SensorObject : BoxObject {

    #region Physics Initialization
        /// <summary>
        /// Create a new sensor at the origin.
        /// </summary>
        /// <param name="texture">Object texture</param>
        public SensorObject(Texture2D texture) :
            this(texture,Vector2.Zero, new Vector2((float)texture.Width,(float)texture.Height)) {}

        /// <summary>
        /// Create a new sensor object
        /// </summary>
        /// <param name="texture">Object texture</param>
        /// <param name="pos">Location in world coordinates</param>
        public SensorObject(Texture2D texture, Vector2 pos) :
            this(texture, pos, new Vector2((float)texture.Width, (float)texture.Height)) { }

        /// <summary>
        /// Create a new sensor object
        /// </summary>
        /// <param name="texture">Object texture</param>
        /// <param name="pos">Location in world coordinates</param>
        /// <param name="dimension">Dimensions in world coordinates</param>
        public SensorObject(Texture2D texture, Vector2 pos, Vector2 dimension)
            : base(texture, pos, dimension) {
            BodyType = BodyType.Static;
            Density = 0.0f;
            Friction = 0.0f;
            Restitution = 0.0f;
        }

        /// <summary>
        /// Creates the physics Body(s) for this object, adding them to the world.
        /// </summary>
        /// <remarks>
        /// This method overrides the base to set the sensor property.
        /// </remarks>
        public override bool ActivatePhysics(World world) {
            // Get the box body from our parent class
            bool success = base.ActivatePhysics(world);
            body.IsSensor = true;
            return success;
        }
    #endregion

    }
}