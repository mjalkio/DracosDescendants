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
    public partial class NewWaypointForm : Form
    {
        AI ai;

        public NewWaypointForm()
        {
            InitializeComponent();
        }

        public NewWaypointForm(AI ai)
        {
            InitializeComponent();

            this.ai = ai;

            List<Tuple<int, int>> waypoints = ai.getWaypoints();

            foreach (Tuple<int, int> wp in waypoints)
            {
                waypointBox.Items.Add(wp);
            }

            if (waypoints.Count != 0)
            {
                waypointBox.SelectedItem = waypoints[waypoints.Count - 1];
            }

            xBox.SelectedText = "0";
            yBox.SelectedText = "0";
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                int x = Convert.ToInt32(xBox.Text);
                int y = Convert.ToInt32(yBox.Text);

                Tuple<int, int> insertAfterMe = waypointBox.SelectedItem != null ? (Tuple<int, int>)waypointBox.SelectedItem : null;

                ai.addWaypoint(x, y, insertAfterMe);
                this.Dispose();
            }
            catch { }
        }
    }
}
