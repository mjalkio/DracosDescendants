using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DracosDescendentsLevelEditor
{
    public partial class ManagePlanetsForm : Form
    {
        List<Planet> planetList;

        public ManagePlanetsForm()
        {
            InitializeComponent();
        }

        public ManagePlanetsForm(List<Planet> planetList)
        {
            InitializeComponent();
            this.planetList = planetList;

            foreach (Planet p in planetList)
            {
                planetSelectComboBox.Items.Add(p);
            }

            planetSelectComboBox.DisplayMember = "Display";
        }

        private void deletePlanetButton_Click(object sender, EventArgs e)
        {
            Planet p = (Planet)planetSelectComboBox.SelectedItem;

            //Delete the gates which use that planet
            List<Gate> invalidGates = new List<Gate>();
            foreach (Gate g in LevelEditorForm.gateList)
            {
                if (g.planet1 == p || g.planet2 == p)
                {
                    invalidGates.Add(g);
                }
            }
            foreach (Gate g in invalidGates)
            {
                LevelEditorForm.gateList.Remove(g);
            }

            //Delete the actual planet
            planetList.Remove(p);
            this.Dispose();
        }

        private void editPlanetButton_Click(object sender, EventArgs e)
        {
            EditPlanetForm planetForm = new EditPlanetForm((Planet)planetSelectComboBox.SelectedItem);
            planetForm.Show();
            this.Dispose();
        }
    }
}
