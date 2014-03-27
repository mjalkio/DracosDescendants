﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DracosDescendentsLevelEditor
{
    public partial class NewGateForm : Form
    {
        private List<Gate> gateList;

        public NewGateForm()
        {
            InitializeComponent();
        }

        public NewGateForm(List<Gate> gateList, List<Planet> planetList)
        {
            InitializeComponent();
            this.gateList = gateList;

            foreach (Planet p in planetList)
            {
                planet1Box.Items.Add(p);
                planet2Box.Items.Add(p);
            }

            //Telling the boxes what to use as the text
            planet1Box.DisplayMember = "Display";
            planet2Box.DisplayMember = "Display";

        }

        private void createButton_Click(object sender, EventArgs e)
        {
            Gate newGate = new Gate((Planet) planet1Box.SelectedItem, (Planet) planet2Box.SelectedItem);
            gateList.Add(newGate);
            this.Dispose();
        }
    }
}
