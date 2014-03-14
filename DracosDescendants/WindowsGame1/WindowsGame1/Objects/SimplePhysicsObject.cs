#region File Description
//-----------------------------------------------------------------------------
// SimplePhysicsObject.cs
//
// Base model class to support collisions.
//
// This class is a subclass of PhysicsObject that supports only one Body.
// it is the prime subclass of most models in the game.
//
// This class does not provide Shape information, and cannot be instantiated
// directly.
//
// Author: Walker M. White
// Based on original PhysicsDemo Lab by Don Holden, 2007
// MonoGame version, 2/14/2014
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

/// <summary>
/// Subnamespace for Farseer-aware models.
/// </summary>
namespace DracosD.Objects {

    /// <summary>
    /// Instance of a Physics object with just one body.
    /// </summary>
    /// <remarks>
    /// SimplePhysicsObject instances do not have joints. They are the primary type of 
    /// physics object. This class does not provide Shape information, and cannot be
    /// instantiated directly.
    /// </remarks>
    public abstract class SimplePhysicsObject : PhysicsObject {

    #region Fields
        // The geometric data.
        protected Body body;

        // The texture for the shape.
        protected Texture2D texture;
    #endregion

    #region Properties (READ-WRITE)
        /// <summary>
        /// BodyType for Farseer physics
        /// </summary>
        /// <remarks>
        /// If you want to lock a body in place (e.g. a platform) set this value to STATIC.
        /// KINEMATIC allows the object to move and collide, but ignores external forces 
        /// (e.g. gravity). DYNAMIC makes this is a full-blown physics object.
        /// </remarks>
        public override BodyType BodyType {
            get { return (body != null ? body.BodyType : bodyType); }
            set {
                bodyType = value; // Always update the buffer.
                if (body != null) {
                    body.BodyType = value;
                }
            }
        }

        /// <summary>
        /// Current position for this physics body
        /// </summary>
        public override Vector2 Position {
            get { return (body != null ? body.Position : position); }
            set {
                position = value; // Always update the buffer.
                if (body != null) {
                    body.Position = value;
                }
            }
        }

        /// <summary>
        /// Linear velocity for this physics body
        /// </summary>
        public override Vector2 LinearVelocity {
            get { return (body != null ? body.LinearVelocity : linearVelocity); }
            set {
                linearVelocity = value; // Always update the buffer.
                if (body != null) {
                    body.LinearVelocity = value;
                }
            }
        }

        /// <summary>
        /// Angle of rotation for this body (about the center).
        /// </summary>
        public override float Rotation {
            get { return (body != null ? body.Rotation : rotation); }
            set {
                rotation = value; // Always update the buffer.
                if (body != null) {
                    body.Rotation = value;
                }
            }
        }

        /// <summary>
        /// Object texture for drawing purposes.
        /// </summary>
        public Texture2D Texture {
            get { return texture; }
            set { texture = value; }
        }
    #endregion

    #region Properties (READ-ONLY)
        /// <summary>
        /// The Farseer body for this object.
        /// </summary>
        /// <remarks>
        /// Use this body to add joints and apply forces.
        /// </remarks>
        public override Body Body {
            get { return body; }
        }
    #endregion

    #region Physics Initialization

        /// <summary>
        /// Create a new simple physics object at the origin.
        /// </summary>
        /// <param name="texture">Object texture</param>
        protected SimplePhysicsObject(Texture2D texture)
            : this(texture, Vector2.Zero) { }

        /// <summary>
        /// Create a new simple physics object
        /// </summary>
        /// <param name="texture">Object texture</param>
        /// <param name="pos">Location of object in world coordinates</param>
        protected SimplePhysicsObject(Texture2D texture, Vector2 pos)
            : base(pos) {
            this.texture = texture;
            body = null;
        }
        
        /// <summary>
        /// Creates the physics Body for this object, adding it to the world.
        /// </summary>
        /// <remarks>
        /// This method depends on the internal method CreateShape() for
        /// the specific body allocation. You should override that method,
        /// not this one, for specific physics objects.
        /// </remarks>
        /// <param name="world">Farseer world that stores body</param>
        /// <returns><c>true</c> if object allocation succeeded</returns>
        public override bool ActivatePhysics(World world) {
            // Make a body, if possible
            body = BodyFactory.CreateBody(world, this);

            // Only initialize if a body was created.
            if (body != null) {
                body.BodyType = bodyType;
                body.Position = position;
                body.LinearVelocity = linearVelocity;
                body.Rotation = rotation;
                CreateShape();
                isActive = true;
            }
            return isActive;
        }

        /// <summary>
        /// Destroys the physics Body of this object if applicable,
        /// removing it from the world.
        /// </summary>
        /// <param name="world">Farseer world that stores body</param>
        public override void DeactivatePhysics(World world) {
            // Should be good for most (simple) applications.
            if (isActive) {
                world.RemoveBody(body);
                body = null;
                isActive = false;
            }
        }

        /// <summary>
        /// Create a Fixture for this body, defining the shape
        /// </summary>
        /// <remarks>
        /// This is the primary method to override for custom physics objects
        /// </remarks>
        protected abstract void CreateShape();
    #endregion

    #region Game Loop (Update and Draw)
        /// <summary>
        /// Updates the object's physics state (NOT GAME LOGIC).
        /// </summary>
        /// <remarks>
        /// This method is called AFTER the collision resolution
        /// state.  Therefore, it should not be used to process
        /// actions or any other gameplay information.  Its primary
        /// purpose is to adjust changes to the fixture, which
        /// have to take place after collision.
        /// </remarks>
        public override void Update(float dt) {
            // Recreate the fixture object if dimensions changed.
            if (isDirty) {
                CreateShape();
            }
        }
    #endregion

    }


}
