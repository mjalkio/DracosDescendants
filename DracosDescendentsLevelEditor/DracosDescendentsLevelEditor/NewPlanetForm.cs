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
    public partial class NewPlanetForm : Form
    {
        private List<Planet> planetList;

        public NewPlanetForm()
        {
            InitializeComponent();
        }

        public NewPlanetForm(List<Planet> planetList)
        {
            InitializeComponent();
            this.planetList = planetList;
            planetComboBox.SelectedIndex = 0; //So that the default value is regular planet
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            try
            {
                float radius = Convert.ToSingle(radiusBox.Text);
                float x = Convert.ToSingle(xBox.Text);
                float y = Convert.ToSingle(yBox.Text);
                Planet newPlanet = new Planet(planetComboBox.SelectedItem.ToString(), radius, x, y);
                planetList.Add(newPlanet);
            }
            catch
            {
            }
            
            this.Dispose();
        }
    }
}
