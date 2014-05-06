using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DracosDescendentsLevelEditor
{
    class Text
    {
        public int startX;
        public int endX;
        public string txt;

        public Text(int startX, int endX, string txt)
        {
            this.startX = startX;
            this.endX = endX;
            this.txt = txt;
        }

        /// <summary>
        /// Used to display the text for ComboBoxes
        /// </summary>
        public string Display
        {
            get
            {
                return txt;
            }
        }
    }
}
