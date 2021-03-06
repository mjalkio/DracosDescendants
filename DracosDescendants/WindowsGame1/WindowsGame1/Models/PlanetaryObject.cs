﻿#region File Description
//-----------------------------------------------------------------------------
// PlanetaryObject.cs
//
// Abstract class used to define planets in our game.
// Based off of a CircleObject rather than BoxObject.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DracosD.Objects;
using DracosD.Views;
#endregion

namespace DracosD.Models
{
    abstract class PlanetaryObject : CircleObject
    {
        #region Initialization
        public PlanetaryObject(Texture2D texture, Vector2 pos, float radius, float density, float friction, float restitution) :
            base(texture, pos, radius)
        {
            BodyType = BodyType.Static;
            Density = density;
            Friction = friction;
            Restitution = restitution;
        }
        #endregion

        #region Game Loop (Update and Draw)
        /// <summary>
        /// Updates the object AFTER collisions are resolved. Primarily for animation.
        /// </summary>
        /// <param name="dt">Timing values from parent loop</param>
        public override void Update(float dt)
        {
            base.Update(dt);

        }

        /// <summary>
        /// Draws the planetary object.
        /// </summary>
        /// <param name="view">Drawing context</param>
        public override void Draw(GameView view)
        {
            view.DrawSprite(texture, Color.White, Position, scale * 2.0f, Rotation);
        }
        #endregion
    }
}
