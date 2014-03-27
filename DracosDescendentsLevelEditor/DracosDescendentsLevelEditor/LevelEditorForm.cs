using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DracosDescendentsLevelEditor
{
    public partial class LevelEditorForm : Form
    {
        private List<Dragon> dragonList;
        private List<Gate> gateList;
        private List<Planet> planetList;


        public LevelEditorForm()
        {
            InitializeComponent();
            dragonList = new List<Dragon>();
            gateList = new List<Gate>();
            planetList = new List<Planet>();
            addDragons();
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

        private void previewButton_Click(object sender, EventArgs e)
        {
            PreviewLevelForm levelForm = new PreviewLevelForm(planetList, gateList);
            levelForm.Width = (int) (levelWidthUpDown.Value * levelForm.SCALE_FACTOR);
            levelForm.Height = (int) (levelHeightUpDown.Value * levelForm.SCALE_FACTOR);
            levelForm.Show();
            levelForm.Draw();
        }

        /// <summary>
        /// Method to add the racers to the game.
        /// </summary>
        private void addDragons()
        {
            dragonList.Add(new Dragon(5, 5, true));
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

            xmlString += "</level>";

            File.WriteAllText(name, xmlString);
        }
    }
}
