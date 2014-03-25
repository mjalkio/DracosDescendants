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
    }
}
