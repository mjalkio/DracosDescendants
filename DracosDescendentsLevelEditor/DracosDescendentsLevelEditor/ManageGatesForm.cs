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
    public partial class ManageGatesForm : Form
    {
        private List<Gate> gateList;
        private List<Planet> planetList;

        public ManageGatesForm()
        {
            InitializeComponent();
        }

        public ManageGatesForm(List<Gate> gateList, List<Planet> planetList)
        {
            InitializeComponent();
            this.gateList = gateList;
            this.planetList = planetList;

            foreach (Gate g in gateList)
            {
                gateSelectBox.Items.Add(g);
            }

            gateSelectBox.DisplayMember = "Display";
        }

        private void editGateButton_Click(object sender, EventArgs e)
        {
            EditGateForm gateForm = new EditGateForm((Gate)gateSelectBox.SelectedItem, gateList, planetList);
            gateForm.Show();
            this.Dispose();
        }

        private void deleteGateButton_Click(object sender, EventArgs e)
        {
            gateList.Remove((Gate)gateSelectBox.SelectedItem);
            this.Dispose();
        }
    }
}
