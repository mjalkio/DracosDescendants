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
        }

        private void planetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Planet newPlanet = new Planet(planetComboBox.SelectedText, Convert.ToSingle(textBox1.Text), Convert.ToSingle(textBox2.Text), Convert.ToSingle(textBox3.Text));
            planetList.Add(newPlanet);
            this.Dispose();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
