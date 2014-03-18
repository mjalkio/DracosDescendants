#region Using Statements
using DracosD.Models;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;
using System.Linq;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
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

        private static Texture2D dragonTexture;
        private static Texture2D regularPlanetTexture;
        private static Texture2D otherPlanetTexture;
        private static Texture2D gateTexture;
        private static Texture2D backgroundTexture;
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

        public int Width
        {
            get { return levelWidth; }
        }

        public int Height
        {
            get { return levelHeight; }
        }

        public Texture2D Background
        {
            get { return backgroundTexture; }
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

        public LevelController(string fileName, Dictionary<string, Texture2D> textures) : this() { }
        /// <summary>
        /// Load all default graphics resources and the XML information for the level
        /// </summary>
        /// <param name='content'>
        /// Reference to global content manager.
        /// </param>
        public void LoadContent(ContentManager content, string xmlFile)
        {
            dragonTexture = content.Load<Texture2D>("rocket");
            backgroundTexture = content.Load<Texture2D>("PrimaryBackground");
            regularPlanetTexture = content.Load<Texture2D>("planet");
            otherPlanetTexture = content.Load<Texture2D>("venus-no-background");
            gateTexture = content.Load<Texture2D>("earthtile");
            parseLevelFromXML(xmlFile);
        }

        public void parseLevelFromXML(string fileName)
        {
            var xml = XDocument.Load(fileName);

            var height = xml.Root.Element("levelheight").Value;
            var width = xml.Root.Element("levelwidth").Value;
            levelHeight = Convert.ToInt32(height);
            levelWidth = Convert.ToInt32(width);

            var dragons = from d in xml.Root.Descendants("dragon")
                          select new
                          {
                              X = d.Element("x").Value,
                              Y = d.Element("y").Value
                          };

            foreach (var dragon in dragons)
            {
                Dragon playerDragon = new Dragon(dragonTexture, new Vector2(Convert.ToInt32(dragon.X), Convert.ToInt32(dragon.Y)));
                racerList.Add(playerDragon);
            }

            var planets = from p in xml.Root.Descendants("planet")
                          select new
                          {
                              Type = p.Element("type").Value,
                              Radius = p.Element("radius").Value,
                              X = p.Element("x").Value,
                              Y = p.Element("y").Value
                          };

            foreach (var planet in planets)
            {
                PlanetaryObject newPlanet;
                Vector2 pos = new Vector2(Convert.ToInt32(planet.X), Convert.ToInt32(planet.Y));
                float radius = Convert.ToSingle(planet.Radius);
                if (planet.Type == "regular")
                {
                    newPlanet = new RegularPlanet(regularPlanetTexture, pos, radius);
                }
                else if (planet.Type == "gaseous")
                {
                    newPlanet = new GaseousPlanet(otherPlanetTexture, pos, radius);
                }
                else if (planet.Type == "lava")
                {
                    newPlanet = new LavaPlanet(otherPlanetTexture, pos, radius);
                }
                else
                {
                    newPlanet = null;
                }
                planetList.Add(newPlanet);
            }

            var gates = from g in xml.Root.Descendants("gate")
                        select new
                        {
                            Planet1 = g.Element("planet1").Value,
                            Planet2 = g.Element("planet2").Value
                        };

            foreach (var gate in gates)
            {
                Gate newGate = new Gate(gateTexture, planetList[Convert.ToInt32(gate.Planet1)], planetList[Convert.ToInt32(gate.Planet2)]);
                gateList.Add(newGate);
            }
        }
        #endregion

    }
}