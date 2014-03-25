#region File Description
//-----------------------------------------------------------------------------
// LavaPlanet.cs
//
// Class is a model for a LavaPlanet.  LavaPlanets can occasionally shoot 
// solar flares, so this class needs to track that.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DracosD.Views;
using System;
using System.Diagnostics;
#endregion

namespace DracosD.Models
{
    class LavaPlanet : PlanetaryObject
    {
        private const float DEFAULT_DENSITY = 5.0f;
        private const float DEFAULT_FRICTION = 0.0f;
        private const float DEFAULT_RESTITUTION = 0.0f;
        private const float maxTime = 2.0f;
        private double randomdouble;

        private float lastFlare;
        private bool fireProjectile;
        private Random rand;

        #region Properties
        public bool Fire
        {
            get { return fireProjectile; }
            set { fireProjectile = false; }
        }
        #endregion

        public LavaPlanet(Texture2D texture, Vector2 pos, float radius) :
            base(texture, pos, radius, DEFAULT_DENSITY, DEFAULT_FRICTION, DEFAULT_RESTITUTION)
        {
            lastFlare = 0;
            rand = new Random();
            randomdouble = rand.NextDouble();
        }


        #region Methods


        #endregion

        #region Game Loop (Update and Draw)
        /// <summary>
        /// Updates the object AFTER collisions are resolved. Primarily for animation.
        /// </summary>
        /// <param name="dt">Timing values from parent loop</param>
        public override void Update(float dt)
        {
            //fireProjectile = false;
            lastFlare += dt;
            Debug.Print(""+randomdouble);

            if ((lastFlare / maxTime) > (float)randomdouble)
            {
                fireProjectile = true;
                lastFlare = 0.0f;
                randomdouble = rand.NextDouble();
            }
            if (lastFlare > maxTime)
            {
                fireProjectile = true;
                lastFlare = 0.0f;
                randomdouble = rand.NextDouble();
            }

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
