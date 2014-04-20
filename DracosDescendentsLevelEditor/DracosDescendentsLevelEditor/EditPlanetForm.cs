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
    public partial class EditPlanetForm : Form
    {
        private Planet planet;

        public EditPlanetForm()
        {
            InitializeComponent();
        }

        public EditPlanetForm(Planet planet)
        {
            InitializeComponent();
            this.planet = planet;
            planetComboBox.SelectedItem = planet.type;
            xBox.SelectedText = Convert.ToString(planet.x);
            yBox.SelectedText = Convert.ToString(planet.y);
            radiusBox.SelectedText = Convert.ToString(planet.radius);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            planet.type = planetComboBox.SelectedItem.ToString();
            try
            {
                planet.radius = Convert.ToSingle(radiusBox.Text);
                planet.x = Convert.ToSingle(xBox.Text);
                planet.y = Convert.ToSingle(yBox.Text);
            }
            catch
            {
            }

            this.Dispose();
        }
    }
}
