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
        private List<List<Vector2>> aiList;
        private List<FloatingText> textList;
        private int levelHeight;
        private int levelWidth;

        private static Texture2D dragonTexture;
        private static Texture2D enemyTexture1;
        private static Texture2D enemyTexture2;
        private static Texture2D enemyTexture3;
        private static Texture2D dragonOnFireTexture;
        private static Texture2D regularPlanetTexture;
        private static Texture2D lavaPlanetTexture;
        private static Texture2D otherPlanetTexture;
        private static Texture2D gateTexture;
        private static Texture2D backgroundTexture;
        private static Texture2D gaseousTexture;
        private static Texture2D igniteTexture;
        private static Texture2D flamingTexture;
        private static Texture2D fireBreath;

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

        public List<List<Vector2>> AIs
        {
            get { return aiList; }
        }
        #endregion

        #region Initialization
        public LevelController()
        {
            racerList = new List<Dragon>();
            gateList = new List<Gate>();
            planetList = new List<PlanetaryObject>();
            aiList = new List<List<Vector2>>();
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
            enemyTexture1 = content.Load<Texture2D>("enemy1");
            enemyTexture2 = content.Load<Texture2D>("enemy2");
            enemyTexture3 = content.Load<Texture2D>("enemy3");
            regularPlanetTexture = content.Load<Texture2D>("planet");
            lavaPlanetTexture = content.Load<Texture2D>("lava planet");
            gateTexture = content.Load<Texture2D>("Gate Band Filmstrip");
            
            // Textures for gas planet
            gaseousTexture = content.Load<Texture2D>("gaseous planet");
            igniteTexture = content.Load<Texture2D>("Igniting Filmstrip");
            flamingTexture = content.Load<Texture2D>("Flaming Filmstrip");


            fireBreath = content.Load<Texture2D>("breath");
            dragonOnFireTexture = content.Load<Texture2D>("DragonFlaming");

            parseLevelFromXML(xmlFile);
        }

        public void parseLevelFromXML(string fileName)
        {
           float worldscale = WorldController.DEFAULT_SCALE;
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

            int temp = 0;
            foreach (var dragon in dragons)
            {
                if (temp == 0)
                {
                    Dragon playerDragon = new Dragon(dragonTexture, new Vector2((Convert.ToInt32(dragon.X) / worldscale),
                            Convert.ToInt32(dragon.Y) / worldscale), new Vector2(dragonTexture.Width / worldscale, dragonTexture.Height / worldscale), fireBreath);
                    playerDragon.OnFireTexture = dragonOnFireTexture; 
                    racerList.Add(playerDragon);
                    temp++;
                }
                else if (temp == 1)
                {
                    Dragon playerDragon = new Dragon(enemyTexture1, new Vector2((Convert.ToInt32(dragon.X) / worldscale),
                        Convert.ToInt32(dragon.Y) / worldscale), new Vector2(dragonTexture.Width / worldscale, dragonTexture.Height / worldscale), fireBreath);
                    playerDragon.OnFireTexture = dragonOnFireTexture; 
                    racerList.Add(playerDragon);
                    temp++;

                }
                else if (temp == 2)
                {
                    Dragon playerDragon = new Dragon(enemyTexture2, new Vector2((Convert.ToInt32(dragon.X) / worldscale),
                        Convert.ToInt32(dragon.Y) / worldscale), new Vector2(dragonTexture.Width / worldscale, dragonTexture.Height / worldscale), fireBreath);
                    playerDragon.OnFireTexture = dragonOnFireTexture;
                    racerList.Add(playerDragon);
                    temp++;

                }
                else
                {
                    Dragon playerDragon = new Dragon(enemyTexture3, new Vector2((Convert.ToInt32(dragon.X) / worldscale),
                        Convert.ToInt32(dragon.Y) / worldscale), new Vector2(dragonTexture.Width / worldscale, dragonTexture.Height / worldscale), fireBreath);
                    playerDragon.OnFireTexture = dragonOnFireTexture; 
                    racerList.Add(playerDragon);
                    temp++;
                }
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
                Vector2 pos = new Vector2(Convert.ToInt32(planet.X) / WorldController.DEFAULT_SCALE, 
                    Convert.ToInt32(planet.Y) / WorldController.DEFAULT_SCALE);
                float radius = Convert.ToSingle(planet.Radius)/WorldController.DEFAULT_SCALE;
                if (planet.Type == "regular")
                {
                    newPlanet = new RegularPlanet(regularPlanetTexture, pos, radius);
                }
                else if (planet.Type == "gaseous")
                {
                    newPlanet = new GaseousPlanet(gaseousTexture, pos, radius, flamingTexture, igniteTexture);
                }
                else if (planet.Type == "lava")
                {
                    newPlanet = new LavaPlanet(lavaPlanetTexture, pos, radius);
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

            IEnumerable<XElement> ais = xml.Root.Descendants("ai");

            foreach (XElement ai in ais)
            {
                //Debug.Print("ai");
                List<Vector2> newAI = new List<Vector2>();

                var waypoints = from wp in ai.Descendants("waypoint")
                                select new
                                {
                                    x = wp.Element("x").Value,
                                    y = wp.Element("y").Value
                                };

                foreach (var waypoint in waypoints)
                {
                    newAI.Add(new Vector2(Convert.ToInt32(waypoint.x), Convert.ToInt32(waypoint.y)));
                }
                newAI.Reverse();
                aiList.Add(newAI);
            }

            var textMessages = from t in xml.Root.Descendants("text")
                               select new
                               {
                                   Content = t.Element("content").Value,
                                   Start = t.Element("start").Value,
                                   End = t.Element("end").Value
                               };

            foreach (var textMessage in textMessages)
            {
                FloatingText t = new FloatingText(textMessage.Content, Convert.ToInt32(textMessage.Start), Convert.ToInt32(textMessage.End));
                textList.Add(t);
            }
        }
        #endregion

    }
}