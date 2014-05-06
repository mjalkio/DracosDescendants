
#region File Description
//-----------------------------------------------------------------------------
// CircularObject.cs
//
// Circular-shaped model to support collisions.
//
// Sometimes you want circles instead of boxes. This class gives it to you.
// Note that the shape must be circular, not Elliptical.  If you want to make
// an ellipse, you will need to make a new class (see the comments in Box.cs).
//
// Author: Walker M. White
// Based on original PhysicsDemo Lab by Don Holden, 2007
// MonoGame version, 2/14/2014
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DracosD.Views;
#endregion

/// <summary>
/// Subnamespace for Farseer-aware models.
/// </summary>
namespace DracosD.Objects {

    /// <summary>
    /// A circukar physics object.
    /// </summary>
    public class CircleObject : SimplePhysicsObject {

    #region Fields
        // buffer variables for creation.
        protected Fixture fixture;
        protected float radius = 1.0f;
        protected Vector2 scale;
    #endregion

    #region Properties (READ-WRITE)
        /// <summary>
        /// Radius of this circle
        /// </summary>
        /// <remarks>
        /// We track the difference between the radius and the texture size
        /// for drawing purposes.  All changes are buffered until Update is called.
        /// </remarks>
        public float Radius {
            get { return radius; }
            set {
                Debug.Assert(value > 0, "Radius must be > 0");
                isDirty = true;
                radius = value;
                scale.X = radius / texture.Width;
                scale.Y = radius / texture.Height;
            }
        }

        public Fixture Fixture
        {
            get { return fixture; }
        }

        /// <summary>
        /// Friction of this body
        /// </summary>
        /// <remarks>
        /// Unlike density, we can pass this value through to the fixture.
        /// </remarks>
        public override float Friction {
            get { return friction; }
            set {
                friction = value; // Always update the buffer
                if (fixture != null) {
                    fixture.Friction = value;
                }
            }
        }

        /// <summary>
        /// Restitution of this body
        /// </summary>
        /// <remarks>
        /// Unlike density, we can pass this value through to the fixture.
        /// </remarks>
        public override float Restitution {
            get { return restitution; }
            set {
                restitution = value; // Always update the buffer
                if (fixture != null) {
                    fixture.Restitution = value;
                }
            }
        }
    #endregion

    #region Physics Initialization
        /// <summary>
        /// Create a new circle at the origin.
        /// </summary>
        /// <param name="texture">Object texture</param>
        public CircleObject(Texture2D texture) : this(texture, Vector2.Zero, (float)texture.Width) { }

        /// <summary>
        /// Create a new circular object
        /// </summary>
        /// <param name="texture">Object texture</param>
        /// <param name="pos">Location in world coordinates</param>
        public CircleObject(Texture2D texture, Vector2 pos) : this(texture,pos,(float)texture.Width) {}

        /// <summary>
        /// Create a new circular object
        /// </summary>
        /// <param name="texture">Object texture</param>
        /// <param name="pos">Location in world coordinates</param>
        /// <param name="radius">Radius in world coordinates</param>
        public CircleObject(Texture2D texture, Vector2 pos, float radius) : 
            base(texture, pos) {
            // Initialize
            this.radius = radius;
            scale = new Vector2(radius/texture.Width,radius/texture.Height);
        }

        /// <summary>
        /// Create a new circular shape.
        /// </summary>
        /// <remarks>
        /// Note the usage of Factories to make fixtures easily.  Looking at this, you
        /// should be able to generalize this idea to an Ellipse or Triangle object. 
        /// Look at the class PolygonTools in Farseer to see shapes we omitted.
        /// </remarks>
        protected override void CreateShape() {
            if (fixture != null) {
                body.DestroyFixture(fixture);
            }
            CircleShape circleShape = new CircleShape(radius, density);
            fixture = body.CreateFixture(circleShape, this);
            fixture.Friction = friction;
            fixture.Restitution = restitution;
            isDirty = false;
        }
    #endregion

    #region Game Loop (Update and Draw)
        /// <summary>
        /// Draws the physics object.
        /// </summary>
        /// <param name="view">Drawing context</param>
        public override void Draw(GameView view) {
            view.DrawSprite(texture, Color.White, Position, scale, Rotation);
        }
    #endregion

    }
}