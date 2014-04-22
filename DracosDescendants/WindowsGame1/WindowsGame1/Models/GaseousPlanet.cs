#region File Description
//-----------------------------------------------------------------------------
// GaseousPlanet.cs
//
// Class is a model for a GaseousPlanet.  Gaseous planets can be lit on fire, 
// so this class needs to add that functionality to PlanetaryObject.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DracosD.Views;
using FarseerPhysics.Dynamics;
using System.Diagnostics;
#endregion

namespace DracosD.Models
{
    class GaseousPlanet : PlanetaryObject
    {
        private const float DEFAULT_DENSITY = 0.1f;
        private const float DEFAULT_FRICTION = 0.0f;
        private const float DEFAULT_RESTITUTION = 0.0f;
        private const int IGNITE_FRAMES = 5;
        private const int BURNING_FRAMES = 4;
        private const int COOLDOWN = 40; //in ticks
        private Texture2D flame_texture;
        private Texture2D ignite_texture;
        private bool onFire;
        private int currCooldown;

        // Animation fields
        private int currFrame = 0;
        private bool frameDirection;
        private int delay = 6;
        private int elapsed;

        public Texture2D Flame_Texture
        {
            get { return flame_texture; }
            set { flame_texture = value; }
        }

        public Texture2D Ignite_Texture
        {
            get { return ignite_texture; }
            set { ignite_texture = value; }
        }

        public bool OnFire
        {
            get { return onFire; }
            set { onFire = value; }
        }

        public bool Burned
        {
            get { return currCooldown == 1; }
        }

        public GaseousPlanet(Texture2D texture, Vector2 pos, float radius, Texture2D fireTexture, Texture2D igniteTexture) :
            base(texture, pos, radius, DEFAULT_DENSITY, DEFAULT_FRICTION, DEFAULT_RESTITUTION)
        {
            flame_texture = fireTexture;
            ignite_texture = igniteTexture;
            onFire = false;
            currCooldown = 0;
        }

        #region GameLoop (update & draw)
        public override void Update(float dt)
        {
            Torch(true);
            //TODO: If we give gaseous planets animation, add it here
            if (currFrame == 0)
            {
                frameDirection = true;
            }
            else if ((currFrame >= (IGNITE_FRAMES - 1) && currCooldown>1) || ((currFrame>=BURNING_FRAMES-1) && onFire))
            {
                frameDirection = false;
            }

            // Increment 
            if (frameDirection)
            {
                elapsed++;
                if (elapsed > delay)
                {
                    currFrame++;
                    elapsed = 0;
                }
            }
            else
            {
                elapsed++;
                if (elapsed > delay)
                {
                    currFrame = 0;
                    elapsed = 0;
                }
            }
            base.Update(dt);
        }

        /// <summary>
        /// Decrement or resets (to
        /// </summary>
        /// <param name="decr"></param>
        public void Torch(bool decr)
        {
            if (decr && currCooldown > 0)
            {
                currCooldown--;
            }
            else if (!decr)
            {
                currCooldown = COOLDOWN;
                currFrame = 0;
            }
        }

        public override bool ActivatePhysics(World world)
        {
            // Get the box body from our parent class
            bool success = base.ActivatePhysics(world);
            body.IsSensor = true;
            return success;
        }

        public override void Draw(GameView view)
        {
            if (onFire)
            {
                view.DrawSprite(flame_texture, Color.White, Position, new Vector2(scale.X * 2, scale.Y * 2), Rotation, currFrame, BURNING_FRAMES, SpriteEffects.None);
            }
            else if (currCooldown > 1)
            {
                view.DrawSprite(ignite_texture, Color.White, Position, new Vector2(scale.X * 2, scale.Y * 2), Rotation, currFrame, IGNITE_FRAMES, SpriteEffects.None);
            }
            else
            {
                base.Draw(view);
            }
       }

        
        #endregion
    }
}
