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
    public partial class EditGateForm : Form
    {
        private Gate gate;
        private List<Gate> gateList;

        public EditGateForm()
        {
            InitializeComponent();
        }

        public EditGateForm(Gate gate, List<Gate> gateList, List<Planet> planetList)
        {
            InitializeComponent();
            this.gate = gate;
            this.gateList = gateList;

            foreach (Planet p in planetList)
            {
                planet1Box.Items.Add(p);
                planet2Box.Items.Add(p);
            }

            foreach (Gate g in gateList)
            {
                if (g != gate)
                {
                    gateBox.Items.Add(g);
                }
            }

            //Telling the boxes what to use as the text
            planet1Box.DisplayMember = "Display";
            planet2Box.DisplayMember = "Display";
            gateBox.DisplayMember = "Display";

            planet1Box.SelectedItem = gate.planet1;
            planet2Box.SelectedItem = gate.planet2;
            gateBox.SelectedIndex = gateBox.Items.Count - 1;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            gate.planet1 = (Planet) planet1Box.SelectedItem;
            gate.planet2 = (Planet)planet2Box.SelectedItem;
            if (gateBox.SelectedItem != null)
            {
                gateList.Remove(gate);
                Gate insertAfterMe = (Gate)gateBox.SelectedItem;
                gateList.Insert(gateList.IndexOf(insertAfterMe), gate);
            }
            this.Dispose();
        }
    }
}
