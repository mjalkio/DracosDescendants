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
#endregion

namespace DracosD.Models
{
    class GaseousPlanet : PlanetaryObject
    {
        private const float DEFAULT_DENSITY = 2.0f;
        private const float DEFAULT_FRICTION = 0.0f;
        private const float DEFAULT_RESTITUTION = 0.0f;
        private const int COOLDOWN = 40; //in ticks
        private Texture2D flame_texture;
        private bool onFire;
        private int currCooldown;

        public Texture2D Flame_Texture
        {
            get { return flame_texture; }
            set { flame_texture = value; }
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

        public GaseousPlanet(Texture2D texture, Vector2 pos, float radius, Texture2D fireTexture) :
            base(texture, pos, radius, DEFAULT_DENSITY, DEFAULT_FRICTION, DEFAULT_RESTITUTION)
        {
            flame_texture = fireTexture;
            onFire = false;
            currCooldown = 0;
        }

        #region GameLoop (update & draw)
        public override void Update(float dt)
        {
            Torch(true);
            //TODO: If we give gaseous planets animation, add it here
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
                view.DrawSprite(flame_texture, Color.White, Position, scale * 2.0f, Rotation);
            }
            else
            {
                view.DrawSprite(base.Texture, Color.White, Position, scale * 2.0f, Rotation);
            }
        }
        #endregion
    }
}
