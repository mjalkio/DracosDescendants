﻿#region File Description
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
        private const float DEFAULT_THRUST = 3000.0f;
        private const int NUM_FRAMES = 9;
        #endregion

        #region Fields
        private Vector2 force;
        private bool isOnFire;

        // texture for dragon
        private Texture2D flapEffect;

        // To animate the rocket flames
        private int animationFrame = 0;
        private bool frameDirection = true;

        //delayed time for animation frame
        private int delay = 1;
        private int elapsed;
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
        public Dragon(Texture2D effect, Vector2 pos) :
            base(effect, pos, new Vector2((float)effect.Width / (NUM_FRAMES * 2), (float)effect.Height) * .1f) 
        {
            BodyType = BodyType.Dynamic;
            flapEffect = effect;
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
            bool success = base.ActivatePhysics(world);
            body.FixedRotation = true;
            return success;
        }
        #endregion

        #region Game Loop (Update and Draw)
        /// <summary>
        /// Updates the object AFTER collisions are resolved. Primarily for animation.
        /// </summary>
        /// <param name="dt">Timing values from parent loop</param>
        public override void Update(float dt) {
            if (base.LinearVelocity.Length() > 3.0f)
            {
                base.LinearVelocity = base.LinearVelocity * 0.95f;
            }


            // Picks which frame of the dragon animation effect
            if (LinearVelocity.Y < -1)
            {
                // Turn on the flames and go back and forth
                if (animationFrame == 0)
                {
                    frameDirection = true;
                }
                else if (animationFrame == (NUM_FRAMES - 1))
                {
                    frameDirection = false;
                }

                // Increment 
                if (frameDirection)
                {
                    elapsed++;
                    if (elapsed > delay)
                    {
                        animationFrame++;
                        elapsed = 0;
                    }
                }
                else
                {
                    elapsed++;
                    if (elapsed > delay)
                    {
                        animationFrame = 0;
                        elapsed = 0;
                    }
                }
            }
            else
            {
                // Turn off the flames
                animationFrame = 0;
            }

            base.Update(dt);


        }

        /// <summary>
        /// Draws the physics object.
        /// </summary>
        /// <param name="view">Drawing context</param>
        public override void Draw(GameView view)
        {
            view.DrawSprite(flapEffect, Color.White, Position, new Vector2(scale.X * NUM_FRAMES * 2, scale.Y), Rotation, animationFrame, NUM_FRAMES);
        }
        #endregion
    }
}
