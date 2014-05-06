using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Reflection;

namespace DracosDescendentsLevelEditor
{
    public partial class LevelEditorForm : Form
    {
        private List<Dragon> dragonList;
        public static List<Gate> gateList;
        public static List<Planet> planetList;
        public static List<AI> aiList;
        public List<Text> textList;
        PreviewLevelForm levelForm;

        public LevelEditorForm()
        {
            InitializeComponent();
            dragonList = new List<Dragon>();
            gateList = new List<Gate>();
            planetList = new List<Planet>();
            aiList = new List<AI>();
            textList = new List<Text>();
            addDragons();
            addAI();
        }

        private void addPlanetButton_Click(object sender, EventArgs e)
        {
            NewPlanetForm planetForm = new NewPlanetForm(planetList);
            planetForm.Show();
        }

        private void managePlanetsButton_Click(object sender, EventArgs e)
        {
            ManagePlanetsForm planetsForm = new ManagePlanetsForm(planetList);
            planetsForm.Show();
        }

        private void addGateButton_Click(object sender, EventArgs e)
        {
            NewGateForm gateForm = new NewGateForm(gateList, planetList);
            gateForm.Show();
        }

        private void manageGatesButton_Click(object sender, EventArgs e)
        {
            ManageGatesForm gatesForm = new ManageGatesForm(gateList, planetList);
            gatesForm.Show();
        }

        private void manageDragonsButton_Click(object sender, EventArgs e)
        {
            ManageDragonsForm dragonsForm = new ManageDragonsForm(dragonList);
            dragonsForm.Show();
        }

        private void manageAIbutton_Click(object sender, EventArgs e)
        {
            ManageAiForm aiForm = new ManageAiForm(aiList);
            aiForm.Show();
        }

        private void previewButton_Click(object sender, EventArgs e)
        {
            if (levelForm != null)
            {
                levelForm.Dispose();
            }

            levelForm = new PreviewLevelForm(planetList, gateList, aiList, (int)levelWidthUpDown.Value, (int)levelHeightUpDown.Value);

            levelForm.Width = (int) ((float)levelWidthUpDown.Value * levelForm.SCALE_FACTOR);
            levelForm.Height = (int) ((float)levelHeightUpDown.Value * levelForm.SCALE_FACTOR) + SystemInformation.CaptionHeight;

            levelForm.Show();
            levelForm.Draw();
        }

        /// <summary>
        /// Method to add the racers to the game.
        /// </summary>
        private void addDragons()
        {
            dragonList.Add(new Dragon(125, 100, true));
            dragonList.Add(new Dragon(125, 200, false));
            dragonList.Add(new Dragon(125, 300, false));
            dragonList.Add(new Dragon(125, 400, false));
        }

        /// <summary>
        /// Method to add default AI to the game.
        /// </summary>
        private void addAI()
        {
            aiList.Add(new AI());
            aiList.Add(new AI());
            aiList.Add(new AI());
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            saveXML.ShowDialog();
        }

        private void saveXML_FileOk(object sender, CancelEventArgs e)
        {
            string name = saveXML.FileName;
            string xmlString ="<level>\r\n";
            xmlString += "  <levelwidth>" + levelWidthUpDown.Value + "</levelwidth>\r\n";
            xmlString += "  <levelheight>" + levelHeightUpDown.Value + "</levelheight>\r\n\r\n";

            foreach (Dragon dragon in dragonList)
            {
                xmlString += "  <dragon>\r\n";
                xmlString += "    <x>" + dragon.x + "</x>\r\n";
                xmlString += "    <y>" + dragon.y + "</y>\r\n";
                xmlString += "  </dragon>\r\n";
            }
            xmlString += "\r\n";

            foreach (Planet planet in planetList)
            {
                xmlString += "  <planet>\r\n";
                xmlString += "    <type>" + planet.type + "</type>\r\n";
                xmlString += "    <radius>" + planet.radius + "</radius>\r\n";
                xmlString += "    <x>" + planet.x + "</x>\r\n";
                xmlString += "    <y>" + planet.y + "</y>\r\n";
                xmlString += "  </planet>\r\n";
            }
            xmlString += "\r\n";

            foreach (Gate gate in gateList)
            {
                xmlString += "  <gate>\r\n";
                xmlString += "    <planet1>" + planetList.IndexOf(gate.planet1) + "</planet1>\r\n";
                xmlString += "    <planet2>" + planetList.IndexOf(gate.planet2) + "</planet2>\r\n";
                xmlString += "  </gate>\r\n";
            }
            xmlString += "\r\n";

            foreach (AI ai in aiList)
            {
                xmlString += "  <ai>\r\n";
                foreach (Tuple<int, int> waypoint in ai.getWaypoints())
                {
                    xmlString += "  <waypoint>\r\n";
                    xmlString += "      <x>" + waypoint.Item1 + "</x>\r\n";
                    xmlString += "      <y>" + waypoint.Item2 + "</y>\r\n";
                    xmlString += "  </waypoint>\r\n";
                }
                xmlString += "  </ai>\r\n";
            }
            xmlString += "\r\n";

            foreach (Text txt in textList)
            {
                xmlString += "  <text>\r\n";
                xmlString += "    <start>" + txt.startX + "</start>\r\n";
                xmlString += "    <end>" + txt.endX + "</end>\r\n";
                xmlString += "    <content>" + txt.txt + "</content>\r\n";
                xmlString += "  </text>\r\n";
            }
            xmlString += "\r\n";

            xmlString += "</level>";

            File.WriteAllText(name, xmlString);
        }

        private void openXML_FileOk(object sender, CancelEventArgs e)
        {
            dragonList = new List<Dragon>();
            gateList = new List<Gate>();
            planetList = new List<Planet>();
            aiList = new List<AI>();
            textList = new List<Text>();

            var xml = XDocument.Load(openXML.FileName);

            var height = xml.Root.Element("levelheight").Value;
            var width = xml.Root.Element("levelwidth").Value;
            levelHeightUpDown.Value = Convert.ToInt32(height);
            levelWidthUpDown.Value = Convert.ToInt32(width);

            var dragons = from d in xml.Root.Descendants("dragon")
                          select new
                          {
                              X = d.Element("x").Value,
                              Y = d.Element("y").Value
                          };

            foreach (var dragon in dragons)
            {
                bool isPlayer = false;

                if (dragonList.Count == 0)
                {
                    isPlayer = true;
                }

                Dragon d = new Dragon(Convert.ToInt32(dragon.X), Convert.ToInt32(dragon.Y), isPlayer);
                dragonList.Add(d);
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
                Planet p = new Planet(planet.Type, Convert.ToInt32(planet.Radius), Convert.ToInt32(planet.X), Convert.ToInt32(planet.Y));
                planetList.Add(p);
            }

            var gates = from g in xml.Root.Descendants("gate")
                        select new
                        {
                            Planet1 = g.Element("planet1").Value,
                            Planet2 = g.Element("planet2").Value
                        };

            foreach (var gate in gates)
            {
                Gate g = new Gate(planetList[Convert.ToInt32(gate.Planet1)], planetList[Convert.ToInt32(gate.Planet2)]);
                gateList.Add(g);
            }

            IEnumerable<XElement> ais = xml.Root.Descendants("ai");

            foreach (XElement ai in ais)
            {
                AI newAI = new AI();

                var waypoints = from wp in ai.Descendants("waypoint")
                                select new
                                {
                                    x = wp.Element("x").Value,
                                    y = wp.Element("y").Value
                                };

                foreach (var waypoint in waypoints)
                {
                    newAI.addWaypoint(Convert.ToInt32(waypoint.x), Convert.ToInt32(waypoint.y));
                }

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
                Text t = new Text(Convert.ToInt32(textMessage.Start), Convert.ToInt32(textMessage.End), textMessage.Content);
                textList.Add(t);
            }

            if (aiList.Count == 0)
            {
                addAI();
            }
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            openXML.Multiselect = false;
            openXML.ShowDialog();
        }

        private void changeScaleButton_Click(object sender, EventArgs e)
        {
            try
            {
                float scale = Convert.ToInt32(scaleBox.Text);

                levelWidthUpDown.Value = (decimal) ((float)levelWidthUpDown.Value * scale);
                levelHeightUpDown.Value = (decimal)((float)levelHeightUpDown.Value * scale);

                foreach (Dragon d in dragonList)
                {
                    d.x = d.x * scale;
                    d.y = d.y * scale;
                }
                foreach (Planet p in planetList)
                {
                    p.radius = p.radius * scale;
                    p.x = p.x * scale;
                    p.y = p.y * scale;
                }
            }
            catch { }
        }
    }
}
