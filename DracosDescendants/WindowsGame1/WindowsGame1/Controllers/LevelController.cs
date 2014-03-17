#region Using Statements
using DracosD.Models;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace DracosD.Controllers
{
    class LevelController
    {
        #region Fields
        private List<Dragon> racerList;
        private List<Gate> gateList;
        private List<PlanetaryObject> planetList;
        private int levelHeight;
        private int levelWidth;
        private Texture2D background;
        #endregion

        #region Properties (READ-ONLY)
        public List<Dragon> Racers
        {
            get { return racerList; }
        }

        public List<PlanetaryObject> Planets
        {
            get { return planetList; }
        }

        public List<Gate> Gates
        {
            get { return gateList; }
        }

        public Vector4 Dimensions
        {
            get { return new Vector4(0, 0, levelWidth, levelHeight); }
        }
        #endregion


    }
}
