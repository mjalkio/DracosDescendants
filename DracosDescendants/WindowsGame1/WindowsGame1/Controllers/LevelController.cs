#region Using Statements
using DracosD.Models;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;
using System.Linq;
using System;
using System.Diagnostics;
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

        public Texture2D Background
        {
            get { return background; }
        }
        #endregion

        #region Initialization
        public LevelController()
        {
            racerList = new List<Dragon>();
            gateList = new List<Gate>();
            planetList = new List<PlanetaryObject>();
            levelHeight = 0;
            levelWidth = 0;
        }

        public LevelController(string fileName, Dictionary<string,Texture2D> textures) : this()
        {
            var xml = XDocument.Load(fileName);

            var height = xml.Root.Element("levelheight").Value ;
            var width = xml.Root.Element("levelwidth").Value;
            levelHeight = Convert.ToInt32(height);
            levelWidth = Convert.ToInt32(width);
            background = textures["background"];

            Dragon playerDragon = new Dragon(textures["player"], new Vector2(5.0f, levelHeight / 2));
            racerList.Add(playerDragon);
            
            var planets = from p in xml.Root.Descendants("planet") select new {
                Type=p.Element("type").Value,
                Radius=p.Element("radius").Value,
                X = p.Element("x").Value,
                Y = p.Element("y").Value
            };

            foreach (var planet in planets)
            {
                PlanetaryObject newPlanet;
                Vector2 pos = new Vector2(Convert.ToInt32(planet.X), Convert.ToInt32(planet.Y));
                float radius = Convert.ToSingle(planet.Radius);
                if (planet.Type == "gaseous")
                {
                    newPlanet = new GaseousPlanet(textures["gaseous"], pos, radius);
                }
                else if (planet.Type == "lava")
                {
                    newPlanet = new LavaPlanet(textures["lava"], pos, radius);
                }
                else
                {
                    newPlanet = new RegularPlanet(textures["regular"], pos, radius);
                }
                planetList.Add(newPlanet);
            }

            var gates = from g in xml.Root.Descendants("gate") select new
                          {
                              Planet1 = g.Element("planet1").Value,
                              Planet2 = g.Element("planet2").Value
                          };

            foreach (var gate in gates)
            {
                //Debug.Print(planetList[Convert.ToInt32(gate.Planet1)].Position.X + "," + planetList[Convert.ToInt32(gate.Planet1)].Position.Y);
                Gate newGate = new Gate(textures["gate"], new Vector2(50.0f,50.0f), planetList[Convert.ToInt32(gate.Planet1)], planetList[Convert.ToInt32(gate.Planet2)]);
                gateList.Add(newGate);
            }
        }
        #endregion

    }
}
