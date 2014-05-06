using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DracosD.Objects;
using DracosD.Views;
using System.Drawing;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;
using FarseerPhysics.Dynamics;
using System.Diagnostics;

namespace DracosD.Models
{
    class FloatingText : PhysicsObject
    {

        #region Fields

        private String text;
        private int startX;
        private int endX;

        #endregion

        #region Properties

        public String Text
        {
            get { return text; }
            set { text = value; }
        }

        public int Start
        {
            get { return startX; }
            set { startX = value; }
        }

        public int End
        {
            get { return endX; }
            set { endX = value; }
        }

        #endregion

        public FloatingText(String initialText, int start, int end)
        {
            text = initialText;
            startX = start;
            endX = end; 
            this.position = position;
            drawState = DrawState.SpritePass;
            isActive = true;
        }

        public override void Draw(GameView view)
        {
            view.DrawText(text, Color.PaleGoldenrod, new Vector2(300,400), true);
        }

        public override void Update(float dt)
        {
            
        }

        public override bool ActivatePhysics(World world)
        {
            return true;
        }

        public override void DeactivatePhysics(World world) {
            // Should be good for most (simple) applications.
            if (isActive) {
                isActive = false;
            }
        }
    }
}
