#region File Description
//-----------------------------------------------------------------------------
// BoxObject.cs
//
// Box-shaped model to support collisions.
//
// Given the name Box-2D, this is your primary model class.  Most of the time,
// unless it is a player controlled avatar, you do not even need to subclass
// BoxObject.  Look through the code and see how many times we use this class.
//
// Author: Walker M. White
// Based on original PhysicsDemo Lab by Don Holden, 2007
// MonoGame version, 2/14/2014
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using DracosD.Models;
using DracosD.Views; // Stupid Box2DX name collision!
#endregion

/// <summary>
/// Subnamespace for Farseer-aware models.
/// </summary>
namespace DracosD.Objects {

    /// <summary>
    /// A rectangular physics object.
    /// </summary>
    public class BoxObject : SimplePhysicsObject {

    #region Fields
        // buffer variables for creation.
        protected Fixture fixture;
        protected Vector2 dimension;
        protected Vector2 scale;
    #endregion

    #region Properties (READ-WRITE)
        /// <summary>
        /// Dimensions of this box
        /// </summary>
        /// <remarks>
        /// We track the difference between the box shape and the texture size
        /// for drawing purposes.  All changes are buffered until Update is called.
        /// </remarks>
        public Vector2 Dimension {
            get { return dimension; }
            set {
                Debug.Assert(value.X > 0 && value.Y > 0, "Dimension attributes must be > 0");
                isDirty = true;
                dimension = value;
                scale.X = dimension.X / texture.Width;
                scale.Y = dimension.Y / texture.Height;
            }
        }

        /// <summary>
        /// Box width
        /// </summary>
        /// <remarks>
        /// We track the difference between the box shape and the texture size
        /// for drawing purposes.  All changes are buffered until Update is called.
        /// </remarks>
        public float Width {
            get { return dimension.X; }
            set {
                Debug.Assert(value > 0, "Width must be > 0");
                isDirty = true;
                dimension.X = value;
                scale.X = dimension.X / texture.Width;
            }
        }

        /// <summary>
        /// Box height
        /// </summary>
        /// <remarks>
        /// We track the difference between the box shape and the texture size
        /// for drawing purposes.  All changes are buffered until Update is called.
        /// </remarks>
        public float Height {
            get { return dimension.Y; }
            set {
                Debug.Assert(value > 0, "Height must be > 0");
                isDirty = true;
                dimension.X = value;
                scale.Y = dimension.Y / texture.Height;
            }
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

        public Fixture Fix
        {
            get { return fixture; }
        }
    #endregion

    #region Physics Initialization
        /// <summary>
        /// Create a new box at the origin.
        /// </summary>
        /// <param name="texture">Object texture</param>
        public BoxObject(Texture2D texture) :
            this(texture, Vector2.Zero, new Vector2((float)texture.Width, (float)texture.Height)) { }

        /// <summary>
        /// Create a new box object
        /// </summary>
        /// <param name="texture">Object texture</param>
        /// <param name="pos">Location in world coordinates</param>
        public BoxObject(Texture2D texture, Vector2 pos) : 
            this(texture,pos,new Vector2((float)texture.Width, (float)texture.Height)) {}

        /// <summary>
        /// Create a new box object
        /// </summary>
        /// <param name="texture">Object texture</param>
        /// <param name="pos">Location in world coordinates</param>
        /// <param name="dimension">Dimensions in world coordinates</param>
        public BoxObject(Texture2D texture, Vector2 pos, Vector2 dimension) :
            base(texture, pos) {
            // Initialize
            this.dimension = dimension;
            scale = new Vector2(dimension.X/texture.Width, dimension.Y/texture.Height);
        }

        /// <summary>
        /// Create a new box shape.
        /// </summary>
        /// <remarks>
        /// Note the usage of Factories to make fixtures easily.  Looking at this, you
        /// should be able to generalize this idea to an Ellipse or Triangle object. 
        /// Look at the class PolygonTools in Farseer to see shapes we omitted.
        /// </remarks>
        protected override void CreateShape() {
            /*if (fixture != null) {
                body.DestroyFixture(fixture);
            }*/
            Vertices rectangleVertices = PolygonTools.CreateRectangle(dimension.X / 2, dimension.Y / 2);
            PolygonShape rectangleShape = new PolygonShape(rectangleVertices, density);
            fixture = body.CreateFixture(rectangleShape, this);
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
            view.DrawSprite(texture,Color.White, Position, scale, Rotation);
        }
    #endregion

    }
}