using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DracosDescendentsLevelEditor
{
    public class Dragon
    {
        public float x;
        public float y;
        public bool isPlayer;

        public Dragon(float x, float y, bool isPlayer)
        {
            this.x = x;
            this.y = y;
            this.isPlayer = isPlayer;
        }

        /// <summary>
        /// Used to display the dragon's info for ComboBoxes
        /// </summary>
        public string Display
        {
            get
            {
                if (isPlayer) { return "Player Dragon at (" + x + ", " + y + ")"; }
                else { return "NPC Dragon at (" + x + ", " + y + ")"; }
            }
        }
    }
}
