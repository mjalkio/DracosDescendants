using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DracosDescendentsLevelEditor
{
    public class Planet
    {
        public string type;
        public float radius;
        public float x;
        public float y;

        public Planet(string type, float radius, float x, float y)
        {
            this.type = type;
            this.radius = radius;
            this.x = x;
            this.y = y;
        }

        public string Display
        {
            get { return type + " planet (" + x + ", " + y + ")"; }
        }
    }
}
