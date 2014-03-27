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
        }

        private void addPlanetButton_Click(object sender, EventArgs e)
        {
            NewPlanetForm planetForm = new NewPlanetForm(planetList);
            planetForm.Show();
        }

        private void addGateButton_Click(object sender, EventArgs e)
        {
            NewGateForm gateForm = new NewGateForm(gateList, planetList);
            gateForm.Show();
        }

        private void previewButton_Click(object sender, EventArgs e)
        {
            PreviewLevelForm levelForm = new PreviewLevelForm(planetList, gateList);
            levelForm.Width = (int) (levelWidthUpDown.Value * levelForm.SCALE_FACTOR);
            levelForm.Height = (int) (levelHeightUpDown.Value * levelForm.SCALE_FACTOR);
            levelForm.Show();
            levelForm.Draw();
        }


    }
}
