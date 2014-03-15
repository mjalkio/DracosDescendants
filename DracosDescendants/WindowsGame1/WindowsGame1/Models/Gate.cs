#region File Description
//-----------------------------------------------------------------------------
// Gate.cs
//
// Defines a gate between two planets that racers need to pass through 
//
// Author: Justin Bard
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
    class Gate : PolygonObject
    {
        #region Fields
        private Texture2D gateTexture;
        private PlanetaryObject planet1;
        private PlanetaryObject planet2;
        #endregion

        #region Properties (READ-WRITE)
        public PlanetaryObject Planet1
        {
            get { return planet1; }
            set { planet1 = value; }
        }

        public PlanetaryObject Planet2
        {
            get { return planet2; }
            set { planet2 = value; }
        }
        #endregion

        #region Initialization
        public Gate(Texture2D texture, Vector2 scale, PlanetaryObject p1, PlanetaryObject p2) :
            this(texture, new Vector2[]
                    {
                        new Vector2(p1.Position.X-0.5f,p1.Position.Y), new Vector2(p1.Position.X+0.5f,p1.Position.Y),
                        new Vector2(p2.Position.X-0.5f,p2.Position.Y), new Vector2(p2.Position.X+0.5f,p2.Position.Y)
                    }, scale, p1, p2) { }

        /// <summary>
        /// Creates the Gate object between two planets
        /// </summary>
        /// <param name="texture">Gate Texture</param>
        /// <param name="points">Points defining the position of the gate</param>
        /// <param name="scale">The scale of the world (use the default)</param>
        /// <param name="p1">The first planet anchor of this gate</param>
        /// <param name="p2">The second planet anchor of this gate</param>
        public Gate(Texture2D texture, Vector2[] points, Vector2 scale, PlanetaryObject p1, PlanetaryObject p2) :
            base(texture, points, scale)
        {
            planet1 = p1;
            planet2 = p2;
            BodyType = BodyType.Static;
            Density = 0.0f;
            Friction = 0.0f;
            Restitution = 0.0f;
        }

        /// <summary>
        /// Creates the physics Body for this oject, adding it to the world
        /// </summary>
        /// <param name="world">Farseer world that stores body</param>
        /// <returns><c>true</c>if object allocation succeeded</returns>
        public override bool ActivatePhysics(World world)
        {
            bool success = base.ActivatePhysics(world);
            return success;
        }
        #endregion

        #region Game Loop (Draw)
        public override void Draw(GameView canvas)
        {
            base.Draw(canvas);
        }
        #endregion

    }
}
