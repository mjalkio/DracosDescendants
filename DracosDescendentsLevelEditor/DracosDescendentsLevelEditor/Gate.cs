using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DracosDescendentsLevelEditor
{
    public class Gate
    {
        public Planet planet1;
        public Planet planet2;

        public Gate(Planet planet1, Planet planet2)
        {
            this.planet1 = planet1;
            this.planet2 = planet2;
        }

        /// <summary>
        /// Used to display the gate's info for ComboBoxes
        /// </summary>
        public string Display
        {
            get
            {
                return "Gate between planet " + LevelEditorForm.planetList.IndexOf(planet1) + " and planet " + LevelEditorForm.planetList.IndexOf(planet2);
            }
        }
    }
}
