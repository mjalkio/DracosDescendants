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
using System;
#endregion

namespace DracosD.Models
{
    class Gate : SensorObject
    {
        #region Fields

        private const int NUM_FRAMES = 9;
        private PlanetaryObject planet1;
        private PlanetaryObject planet2;

        private const int FRAME_DELAY = 4;
        private int delayCount;
        private int animationFrame = 0;
        private bool framedirection = true;
        private bool hasBeenHit;
        #endregion

        #region Properties (READ-WRITE)
        public int Frame{
            get { return animationFrame; }
            set { animationFrame = value; }
        }

        public bool Hit
        {
            get { return hasBeenHit; }
            set { hasBeenHit = value; }
        }

        public int AnimationFrame
        {
            get { return animationFrame; }
            set { animationFrame = value; }
        }

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
        public Gate(Texture2D texture, PlanetaryObject p1, PlanetaryObject p2) :
            this(texture, ((p1.Position - p2.Position) / 2.0f) + p2.Position, new Vector2(1.0f, (p1.Position - p2.Position).Length()), p1, p2, (p2.Position - p1.Position)) { }

        /// <summary>
        /// Creates the Gate object between two planets
        /// </summary>
        /// <param name="texture">Gate Texture</param>
        /// <param name="points">Points defining the position of the gate</param>
        /// <param name="scale">The scale of the world (use the default)</param>
        /// <param name="p1">The first planet anchor of this gate</param>
        /// <param name="p2">The second planet anchor of this gate</param>
        public Gate(Texture2D texture, Vector2 pos, Vector2 dimension, PlanetaryObject p1, PlanetaryObject p2, Vector2 angleVector) :
            base(texture, pos, dimension)
        {
            planet1 = p1;
            planet2 = p2;
            Rotation = (float)Math.Atan2(angleVector.X, -angleVector.Y);
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

        public override void Update(float dt)
        {
            if (!hasBeenHit)
            {
                if (animationFrame > 3) animationFrame = 2;
                if (animationFrame < 0) animationFrame = 1;
                if (delayCount == FRAME_DELAY)
                {
                    if (framedirection)
                    {
                        animationFrame++;
                        if (animationFrame == 3) framedirection = false;
                    }
                    else
                    {
                        animationFrame--;
                        if (animationFrame == 0) framedirection = true;
                    }
                    delayCount = 0;
                }
                delayCount++;
            }
        
            base.Update(dt);
        }

        public override void Draw(GameView canvas)
        {
            Vector2 drawScale = scale;
            drawScale.X = drawScale.X * 100;
            if (animationFrame <= NUM_FRAMES)
            {
                canvas.DrawSprite(texture, Color.White, Position, drawScale, Rotation, animationFrame, NUM_FRAMES, SpriteEffects.None);
            }
            //base.Draw(canvas);
        }
        #endregion

        public void incrementFrame()
        {
            hasBeenHit = true;
            if (delayCount == FRAME_DELAY){
                animationFrame++;
                delayCount =0;
            }
            delayCount++;
        }

    }
}