#region File Description
//-----------------------------------------------------------------------------
// FireBreath.cs
//
// Defines a the firebreath of a Dragon
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
    class FireBreath : BoxObject
    {
        protected Vector2 rPos;
        protected bool flipText;
        SpriteEffects fliped;
        protected bool ai;
        protected Vector2 d;
        /*public FireBreath(Texture2D flameTexture, Vector2 center, Vector2 scale, bool flip, bool ai, Vector2 drag) :
            base(flameTexture, new Vector2[]
            {
                center, new Vector2(center.X+5.0f,center.Y-2.0f), new Vector2(center.X+5.0f,center.Y+2.0f)}, scale)
        {
            rPos = center;
            flipText = flip;
            this.ai = ai;
            d = drag;
            Density = 0.0f;
            Friction = 0.0f;
            Restitution = 0.0f;
        }*/
        public FireBreath(Texture2D flameTexture, Vector2 center, bool flip, bool ai, Vector2 drag) :
            base(flameTexture, center, new Vector2(flameTexture.Width / 10.0f, flameTexture.Height / 10.0f))
        {
            rPos = center;
            flipText = flip;
            this.ai = ai;
            d = drag;
            Density = 0.0f;
            Friction = 0.0f;
            Restitution = 0.0f;
        }

        #region GameLoop (update & draw)
        public override void Update(float dt)
        {
            //TODO: If we give flames animations, do it here
            base.Update(dt);
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
            /*if (flipText)
            {
                Rotation = (float)Math.PI;
            }
            else
            {
                Rotation = 0.0f;
            }
            view.DrawPolygons(vertices, Texture, Position, Rotation, 1.0f, BlendState.AlphaBlend,ai,d);*/

            fliped = flipText ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            
            //view.DrawSprite(Texture, Color.White, Position, new Vector2(scale.X, scale.Y*2.0f), Rotation, 1, 2, fliped);
            view.DrawSprite(Texture, Color.White, Position, scale, Rotation, fliped);
            //base.Draw(view);
        }
        #endregion
    }
}

